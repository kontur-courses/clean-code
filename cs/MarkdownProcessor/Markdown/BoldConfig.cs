namespace MarkdownProcessor.Markdown;

public class BoldConfig : ITagMarkdownConfig
{
    private readonly char[] digits = Enumerable.Range(0, 10).Select(n => n.ToString()[0]).ToArray();

    public string Sign => "__";
    public TextType TextType => TextType.Bold;

    public ITag? TryCreate(Token token)
    {
        var afterIsSpace = token.After is null or ' ';

        return afterIsSpace || token.BetweenDigits ? null : new Bold(token.TagFirstCharIndex);
    }

    public bool IsClosingToken(Token token)
    {
        var beforeIsSpace = token.Before is null or ' ';

        return !beforeIsSpace && !token.BetweenDigits;
    }
}