namespace Markdown;

public class MdKeySymbolOperations
{
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
			var mdSymbol = MdKeySymbol.Create("#", position, MdSymbolRelativePosition.Close);
			symbols[0].ClosingSymbol = mdSymbol;
			mdSymbol.OpeningSymbol = symbols[0];
			symbols.Add(mdSymbol);
		}

		return symbols;
	}

	private static IEnumerable<MdKeySymbol> GetMdKeySymbolsInWord(
	ReadOnlyMemory<char> word,
	Stack<MdKeySymbolWithWordNumberStamp> stack,
	int position,
	int wordNumber,
	bool isFirstNonSpace)
	{
		var index = 0;
		var relativePositions = DetermineRelativePosition(word);
		while (index < word.Length)
		{
			var peekSymbol = stack.Count > 0 ? stack.Peek().MdSymbol : null;

			var symbol = word.Span[index];
			if (MdKeySymbol.IsKeySymbol(symbol))
			{
				if (peekSymbol?.Type != MdSymbolType.Backslash || peekSymbol?.Position < position - 1)
				{
					var readSymbol = GetMdKeySymbol(word[index..]);
					var mdSymbol = MdKeySymbol.Create(readSymbol, position + index, relativePositions[index]);
					index += readSymbol.Length - 1;
					var isValidHeader = isFirstNonSpace && word.Length == 1;
					foreach (var resultMdSymbol in ProcessMdKeySymbol(mdSymbol, stack, wordNumber, isValidHeader))
						yield return resultMdSymbol;
				}
				else
					yield return stack.Pop().MdSymbol;
			}

			index++;
		}

		foreach (var mdSymbol in GetPairWithMiddleSymbol(stack, word, wordNumber))
			yield return mdSymbol;
	}

	private static string GetMdKeySymbol(ReadOnlyMemory<char> word)
	{
		var str = word.ToString();
		var index = 0;
		while (index < str.Length - 1 && MdKeySymbol.CountKeySymbolsWithPrefix(str[..(index + 1)]) > 1)
		{
			index++;
		}

		if (MdKeySymbol.CountKeySymbolsWithPrefix(str[..(index + 1)]) == 0)
			index--;

		return str[..(index + 1)];
	}

	private static MdSymbolRelativePosition[] DetermineRelativePosition(ReadOnlyMemory<char> word)
	{
		var halfLength = word.Length / 2 + word.Length % 2;
		var relativePositions = new MdSymbolRelativePosition[word.Length];
		var wordSpan = word.Span;

		var position = MdSymbolRelativePosition.Open;
		for (var i = 0; i < halfLength; i++)
		{
			if (!MdKeySymbol.IsKeySymbol(wordSpan[i]))
				position = MdSymbolRelativePosition.Middle;

			relativePositions[i] = position;
		}

		position = MdSymbolRelativePosition.Close;
		for (var i = word.Length - 1; i >= halfLength; i--)
		{
			if (!MdKeySymbol.IsKeySymbol(wordSpan[i]))
				position = MdSymbolRelativePosition.Middle;

			relativePositions[i] = position;
		}

		return relativePositions;
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
			case MdSymbolType.DoubleUnderscore:
				if (mdSymbol.RelativePosition != MdSymbolRelativePosition.Close)
				{
					stack.Push(new(mdSymbol, wordNumber));
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
				&& currentMdSymbol.WordNumber < wordNumber
				|| currentMdSymbol.MdSymbol.Type == MdSymbolType.Backslash)
				continue;

			if (currentMdSymbol.MdSymbol.Type == mdSymbol.Type)
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

	private static IEnumerable<MdKeySymbol> GetPairWithMiddleSymbol(
		Stack<MdKeySymbolWithWordNumberStamp> stack,
		ReadOnlyMemory<char> word,
		int currentWordNumber)
	{
		while (stack.Count > 0
			&& (stack.Peek().MdSymbol.Type == MdSymbolType.Underscore
				|| stack.Peek().MdSymbol.Type == MdSymbolType.DoubleUnderscore)
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

	private record class MdKeySymbolWithWordNumberStamp(MdKeySymbol MdSymbol, int WordNumber);
}
