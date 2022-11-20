using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public readonly struct Token
{
    public Token(ITagMarkdownConfig config, int tagFirstCharIndex, char? before, char? after)
    {
        Config = config;
        TagFirstCharIndex = tagFirstCharIndex;
        Before = before;
        After = after;
    }

    public char? Before { get; }
    public ITagMarkdownConfig Config { get; }
    public int TagFirstCharIndex { get; }
    public char? After { get; }
    private readonly char[] digits = Enumerable.Range(0, 10).Select(n => n.ToString()[0]).ToArray();

    public bool BetweenDigits => Before.HasValue && After.HasValue &&
                                 digits.Contains(Before.Value) && digits.Contains(After.Value);
}