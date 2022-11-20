namespace MarkdownProcessor.Markdown;

public class ItalicConfig : ITagMarkdownConfig
{
    public TextType TextType => TextType.Italic;
    public string Sign => "_";

    public ITag? TryCreate(Token token)
    {
        var afterIsSpace = token.After is null or ' ';

        return afterIsSpace || token.BetweenDigits ? null : new Italic(token.TagFirstCharIndex);
    }

    public bool IsClosingToken(Token token)
    {
        var beforeIsSpace = token.Before is null or ' ';

        return !beforeIsSpace && !token.BetweenDigits;
    }
}