namespace MarkdownProcessor;

public readonly struct Token
{
    public Token(int tagFirstCharIndex, string value, char? before, char? after)
    {
        TagFirstCharIndex = tagFirstCharIndex;
        Value = value;
        Before = before;
        After = after;
    }

    public int TagFirstCharIndex { get; }
    public string Value { get; }

    public char? Before { get; }
    public char? After { get; }
    private readonly char[] digits = Enumerable.Range(0, 10).Select(n => n.ToString()[0]).ToArray();

    public bool BetweenDigits => Before.HasValue && After.HasValue &&
                                 digits.Contains(Before.Value) && digits.Contains(After.Value);

    public bool BeforeIsSpace => string.IsNullOrWhiteSpace(Before.ToString());

    public bool AfterIsSpace => string.IsNullOrWhiteSpace(After.ToString());

    public override string ToString()
    {
        return $"Token{(TagFirstCharIndex, Before, Value, After)}";
    }
}