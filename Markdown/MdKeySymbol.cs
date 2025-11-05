namespace Markdown;

public class MdKeySymbol
{
    private static readonly string keySymbols = @"_#\";
	private static readonly Dictionary<MdKeySymbol, string> mdSymbolToHtmlTag = new()
	{
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Open, MdSymbolQuantity.Single, 0)] = "<em>",
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Close, MdSymbolQuantity.Single, 0)] = "</em>",
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Open, MdSymbolQuantity.Double, 0)] = "<strong>",
		[new(MdSymbolType.Underscore, MdSymbolRelativePosition.Close, MdSymbolQuantity.Double, 0)] = "</strong>",
		[new(MdSymbolType.NumberSign, MdSymbolRelativePosition.Open, MdSymbolQuantity.Single, 0)] = "<h1>",
		[new(MdSymbolType.NumberSign, MdSymbolRelativePosition.Close, MdSymbolQuantity.Single, 0)] = "</h1>"
	};

	private readonly List<MdKeySymbol> validationRelatedSymbols = [];

	public MdSymbolType Type { get; set; }
    public MdSymbolRelativePosition RelativePosition { get; set; }
    public MdSymbolQuantity Quantity { get; set; }
    public int Position { get; set; }
    public bool IsValid { get; private set; } = true;

    public MdKeySymbol? OpeningSymbol { get; set; }
    public MdKeySymbol? ClosingSymbol { get; set; }

    public static bool IsKeySymbol(char symbol) => keySymbols.Contains(symbol);

    public static MdKeySymbol Create(string symbol, int position, MdSymbolRelativePosition relativePosition)
        => symbol switch
        {
            "_" => new(
                MdSymbolType.Underscore,
                relativePosition,
                MdSymbolQuantity.Single,
                position),
            "__" => new(
                MdSymbolType.Underscore,
                relativePosition,
                MdSymbolQuantity.Double,
                position),
            @"\" => new(
                MdSymbolType.Backslash,
                relativePosition,
                MdSymbolQuantity.Single,
                position),
            "#" => new(
                MdSymbolType.NumberSign,
                relativePosition,
                MdSymbolQuantity.Single,
                position),
            _ => throw new ArgumentException("Invalid symbol")
        };

	public static void Validate(IEnumerable<MdKeySymbol> symbols)
	{
		var banDoubleUnderscore = false;
		foreach (var symbol in symbols)
		{
			if (symbol.validationRelatedSymbols.Count > 0
				|| symbol.GetContentLength() == 0
				|| symbol.Type == MdSymbolType.Underscore
				    && symbol.Quantity == MdSymbolQuantity.Double
				    && banDoubleUnderscore)
			{
				symbol.SetInvalid();
			}

			if (symbol.Type == MdSymbolType.Underscore
				&& symbol.Quantity == MdSymbolQuantity.Single)
            {
				banDoubleUnderscore = !banDoubleUnderscore;
			}
		}
	}

	public static List<MdKeySymbol> GetMdKeySymbolsInLine(IEnumerable<ReadOnlyMemory<char>> line)
	{
		var symbols = new List<MdKeySymbol>();
		var stack = new Stack<MdKeySymbolWithWordNumberStamp>();
		var isFirstNonSpace = true;
		var position = 0;
		var wordNumber = 0;

		foreach (var word in line)
		{
			symbols.AddRange(GetMdKeySymbolsInWord(word, stack, position, wordNumber, isFirstNonSpace));

			position += word.Length;
			wordNumber++;
			isFirstNonSpace = isFirstNonSpace && IsWhiteSpace(word);
		}

		symbols.Sort((x, y) => x.Position - y.Position);
		if (symbols.Count > 0 && symbols[0].Type == MdSymbolType.NumberSign)
		{
			var mdSymbol = Create("#", position, MdSymbolRelativePosition.Close);
			symbols[0].ClosingSymbol = mdSymbol;
			mdSymbol.OpeningSymbol = symbols[0];
			symbols.Add(mdSymbol);
		}

		return symbols;
	}

	public MdKeySymbol(
        MdSymbolType type,
        MdSymbolRelativePosition relativePosition,
        MdSymbolQuantity quantity,
        int position)
    {
        Type = type;
        RelativePosition = relativePosition;
        Quantity = quantity;
        Position = position;
    }

	public MdKeySymbol(MdKeySymbol MdSymbol) : this(
		MdSymbol.Type,
		MdSymbol.RelativePosition,
		MdSymbol.Quantity,
		MdSymbol.Position) { }

	public int CalculateSymbolLength()
	{
		if (Type == MdSymbolType.NumberSign)
		{
			return RelativePosition == MdSymbolRelativePosition.Open
				? 2
				: 0;
		}

		if (Quantity == MdSymbolQuantity.Single)
			return 1;

		return 2;
	}

	public string ToHtmlTag()
	{
		if (Type == MdSymbolType.Backslash)
			return "";

		var newSymbol = new MdKeySymbol(this);
		if (newSymbol.RelativePosition == MdSymbolRelativePosition.Middle)
			newSymbol.RelativePosition = OpeningSymbol is null
				? MdSymbolRelativePosition.Open
				: MdSymbolRelativePosition.Close;

		return mdSymbolToHtmlTag[newSymbol];
	}

	public void AddValidationRelatedSymbol(MdKeySymbol symbol) => validationRelatedSymbols.Add(symbol);

    public override bool Equals(object? obj)
    {
        return obj is MdKeySymbol symbol
            && Type == symbol.Type
            && RelativePosition == symbol.RelativePosition
            && Quantity == symbol.Quantity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, RelativePosition, Quantity);
    }

	private static IEnumerable<MdKeySymbol> GetMdKeySymbolsInWord(
		ReadOnlyMemory<char> word,
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		int position,
		int wordNumber,
		bool isFirstNonSpace)
	{
		for (var i = 0; i < word.Length; i++)
		{
			var peekSymbol = stack.Count > 0 ? stack.Peek().MdSymbol : null;

			var symbol = word.Span[i];
			if (IsKeySymbol(symbol))
			{
				if (peekSymbol?.Type != MdSymbolType.Backslash)
				{
					var mdSymbol = Create(symbol.ToString(), position, DetermineRelativePosition(i, word.Length));
					var isValidHeader = isFirstNonSpace && word.Length == 1;
					foreach (var resultMdSymbol in ProcessMdKeySymbol(mdSymbol, stack, wordNumber, isValidHeader))
						yield return resultMdSymbol;
				}
				else
					yield return stack.Pop().MdSymbol;
			}
			else if (peekSymbol?.Type == MdSymbolType.Backslash)
				stack.Pop();

			position++;
		}

		foreach (var mdSymbol in GetPairWithMiddleSymbol(stack, word, wordNumber))
			yield return mdSymbol;
	}

	private static MdSymbolRelativePosition DetermineRelativePosition(int positionInWord, int wordLength)
	{
		if (positionInWord == 0)
			return MdSymbolRelativePosition.Open;

		if (positionInWord == wordLength - 1)
			return MdSymbolRelativePosition.Close;

		return MdSymbolRelativePosition.Middle;
	}

	private static IEnumerable<MdKeySymbol> ProcessMdKeySymbol(
		MdKeySymbol mdSymbol,
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		int wordNumber,
		bool isValidHeader)
	{
		switch (mdSymbol.Type)
		{
			case MdSymbolType.NumberSign when isValidHeader:
				yield return mdSymbol;
				break;
			case MdSymbolType.Backslash:
				stack.Push(new(mdSymbol, wordNumber));
				break;
			case MdSymbolType.Underscore:
				var peekSymbol = stack.Count > 0 ? stack.Peek().MdSymbol : null;
				if (TryUnionTwoUnderscore(mdSymbol, peekSymbol, out var unionSymbol))
				{
					mdSymbol = unionSymbol!;
					stack.Pop();
				}

				if (mdSymbol.RelativePosition != MdSymbolRelativePosition.Close)
				{
					UpdateStackTop(mdSymbol, stack, wordNumber);
				}
				else if (TryRemovePair(mdSymbol, stack, wordNumber, out var pair))
				{
					mdSymbol.OpeningSymbol = pair;
					pair.ClosingSymbol = mdSymbol;
					yield return pair;
					yield return mdSymbol;
				}
				break;
			default:
				break;
		}
	}

	private static void UpdateStackTop(
		MdKeySymbol mdSymbol,
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		int wordNumber)
	{
		// А нужно ли это?
		//var peekSymbol = stack.Count > 0 ? stack.Peek().MdSymbol : null!;
		//if (mdSymbol.RelativePosition == MdSymbolRelativePosition.Open
		//	&& mdSymbol.Equals(peekSymbol))
		//{
		//	stack.Pop();
		//}

		stack.Push(new(mdSymbol, wordNumber));
	}

	private static bool TryRemovePair(
		MdKeySymbol mdSymbol,
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		int wordNumber,
		out MdKeySymbol pair)
	{
		var tempStack = new Stack<MdKeySymbolWithWordNumberStamp>();
		while (stack.Count > 0)
		{
			var currentMdSymbol = stack.Pop();

			if (mdSymbol.RelativePosition == MdSymbolRelativePosition.Middle
				&& currentMdSymbol.WordNumber < wordNumber)
			{
				tempStack.Push(currentMdSymbol);
				break;
			}

			if (currentMdSymbol.MdSymbol.RelativePosition == MdSymbolRelativePosition.Middle
				&& currentMdSymbol.WordNumber < wordNumber)
				continue;

			if (currentMdSymbol.MdSymbol.Type == mdSymbol.Type
				&& currentMdSymbol.MdSymbol.Quantity == mdSymbol.Quantity)
			{
				pair = currentMdSymbol.MdSymbol;
				MoveElementsFromStackToStack(tempStack, stack);
				return true;
			}
			else
			{
				currentMdSymbol.MdSymbol.AddValidationRelatedSymbol(mdSymbol);
				tempStack.Push(currentMdSymbol);
			}
		}

		pair = null!;
		MoveElementsFromStackToStack(tempStack, stack);
		return false;
	}

	private static void MoveElementsFromStackToStack(
		Stack<MdKeySymbolWithWordNumberStamp> from,
		Stack<MdKeySymbolWithWordNumberStamp> to)
	{
		while (from.Count > 0)
			to.Push(from.Pop());
	}

	private static bool IsWhiteSpace(ReadOnlyMemory<char> word)
	{
		foreach (var symbol in word.Span)
			if (!char.IsWhiteSpace(symbol))
				return false;

		return true;
	}

	private static bool DoesContainsDigits(ReadOnlyMemory<char> word)
	{
		foreach (var symbol in word.Span)
			if (char.IsDigit(symbol))
				return true;

		return false;
	}

	private static bool TryUnionTwoUnderscore(
		MdKeySymbol? first,
		MdKeySymbol? second,
		out MdKeySymbol? unionSymbol)
	{
		if (first == null
			|| second == null
			|| first.Type != MdSymbolType.Underscore
			|| first.Quantity != MdSymbolQuantity.Single
			|| second.Type != MdSymbolType.Underscore
			|| second.Quantity != MdSymbolQuantity.Single
			|| Math.Abs(first.Position - second.Position) != 1
			|| first.RelativePosition != MdSymbolRelativePosition.Middle
				&& second.RelativePosition != MdSymbolRelativePosition.Middle)
		{
			unionSymbol = null;
			return false;
		}

		unionSymbol = new(
			MdSymbolType.Underscore,
			(first.RelativePosition == MdSymbolRelativePosition.Middle ? second : first)
				.RelativePosition,
			MdSymbolQuantity.Double,
			Math.Min(first.Position, second.Position));

		return true;
	}

	private static IEnumerable<MdKeySymbol> GetPairWithMiddleSymbol(
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		ReadOnlyMemory<char> word,
		int currentWordNumber)
	{
		while (stack.Count > 0
			&& stack.Peek().MdSymbol.Type == MdSymbolType.Underscore
			&& stack.Peek().MdSymbol.RelativePosition == MdSymbolRelativePosition.Middle)
		{
			var mdSymbol = stack.Pop().MdSymbol;
			if (!DoesContainsDigits(word) && TryRemovePair(mdSymbol, stack, currentWordNumber, out var pair))
			{
				mdSymbol.OpeningSymbol = pair;
				pair.ClosingSymbol = mdSymbol;
				yield return pair;
				yield return mdSymbol;
			}
		}
	}

	private void SetInvalid()
	{
		if (!IsValid) return;

		IsValid = false;

		OpeningSymbol?.SetInvalid();
		ClosingSymbol?.SetInvalid();
		foreach (var symbol in validationRelatedSymbols)
			symbol?.SetInvalid();
	}

	// не учитывает другие теги (они могут быть внутри этого тега и могут быть пустые)
	private int GetContentLength()
	{
		if (Type == MdSymbolType.Backslash)
			return 1;

		return Math.Abs(Position - (OpeningSymbol?.Position ?? ClosingSymbol!.Position))
			- (Quantity == MdSymbolQuantity.Single ? 1 : 2);
	}

	private record class MdKeySymbolWithWordNumberStamp(MdKeySymbol MdSymbol, int WordNumber);
}
