namespace Markdown;

public class MdKeySymbol
{
    private static readonly HashSet<char> keySymbols = [];
	private static readonly Dictionary<string, int> keySymbolPrefixesCount = [];
	private static readonly Dictionary<MdSymbolType, string> keySymbolToString = new()
	{
		[MdSymbolType.Underscore] = "_",
		[MdSymbolType.DoubleUnderscore] = "__",
		[MdSymbolType.NumberSign] = "# ",
		[MdSymbolType.Backslash] = @"\"
	};

	private readonly List<MdKeySymbol> validationRelatedSymbols = [];

	public MdSymbolType Type { get; }
    public MdSymbolRelativePosition RelativePosition { get; }
    public int Position { get; set; }
    public bool IsValid { get; private set; } = true;

    public MdKeySymbol? OpeningSymbol { get; set; }
    public MdKeySymbol? ClosingSymbol { get; set; }

    public static bool IsKeySymbol(char symbol) => keySymbols.Contains(symbol);

	public static int CountKeySymbolsWithPrefix(string prefix)
	{
		return keySymbolPrefixesCount.TryGetValue(prefix, out var count) ? count : 0;
	}

    public static MdKeySymbol Create(string symbol, int position, MdSymbolRelativePosition relativePosition)
        => symbol switch
        {
            "_" => new(
                MdSymbolType.Underscore,
                relativePosition,
                position),
            "__" => new(
                MdSymbolType.DoubleUnderscore,
                relativePosition,
                position),
            @"\" => new(
                MdSymbolType.Backslash,
                relativePosition,
                position),
            "#" => new(
                MdSymbolType.NumberSign,
                relativePosition,
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
				|| symbol.Type == MdSymbolType.DoubleUnderscore
					&& banDoubleUnderscore)
			{
				symbol.SetInvalid();
			}

			if (symbol.Type == MdSymbolType.Underscore)
			{
				banDoubleUnderscore = !banDoubleUnderscore;
			}
		}
	}

	static MdKeySymbol()
	{
		foreach (var symbol in keySymbolToString.Values)
		{
			for (var i = 0; i < symbol.Length; i++)
			{
				if (!char.IsWhiteSpace(symbol[i]))
					keySymbols.Add(symbol[i]);

				var prefix = symbol[..(i + 1)];
				keySymbolPrefixesCount[prefix] =
					keySymbolPrefixesCount.TryGetValue(prefix, out var count) ? count + 1 : 1;
			}
		}
	}

	public MdKeySymbol(
        MdSymbolType type,
        MdSymbolRelativePosition relativePosition,
        int position)
    {
        Type = type;
        RelativePosition = relativePosition;
        Position = position;
    }

	public string Render(IReadOnlyDictionary<MdKeySymbol, string> mapping)
	{
		if (Type == MdSymbolType.Backslash)
			return "";

		if (RelativePosition == MdSymbolRelativePosition.Middle)
		{
			var newSymbol = new MdKeySymbol(
				Type,
				OpeningSymbol is null
					? MdSymbolRelativePosition.Open
					: MdSymbolRelativePosition.Close,
				Position
			);

			return mapping[newSymbol];
		}

		return mapping[this];
	}

	public void AddValidationRelatedSymbol(MdKeySymbol symbol) => validationRelatedSymbols.Add(symbol);

    public override bool Equals(object? obj)
    {
        return obj is MdKeySymbol symbol
            && Type == symbol.Type
            && RelativePosition == symbol.RelativePosition;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, RelativePosition);
    }

	public override string ToString()
	{
		return keySymbolToString[Type];
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

	private int GetContentLength()
	{
		if (Type == MdSymbolType.Backslash)
			return 1;

		return Math.Abs(Position - (OpeningSymbol?.Position ?? ClosingSymbol!.Position))
			- (OpeningSymbol ?? this).ToString().Length;
	}
}
