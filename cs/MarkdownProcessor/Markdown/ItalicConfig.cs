namespace MarkdownProcessor.Markdown;

public class ItalicConfig : ITagMarkdownConfig
{
    public TextType TextType => TextType.Italic;
    public string OpeningSign => "_";
    public string ClosingSign => OpeningSign;

    public ITag? CreateOrNull(Token token)
    {
        var afterIsSpace = string.IsNullOrWhiteSpace(token.After.ToString());
        ;

        return afterIsSpace || token.BetweenDigits
            ? null
            : new Italic(token, !string.IsNullOrWhiteSpace(token.Before.ToString()));
    }
}