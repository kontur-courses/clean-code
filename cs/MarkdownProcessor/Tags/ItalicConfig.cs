namespace MarkdownProcessor.Tags;

public class ItalicConfig : ITagMarkdownConfig
{
    public string OpeningSign => "_";
    public string ClosingSign => OpeningSign;
    public TextType TextType => TextType.Italic;

    public ITag? CreateOrNull(Token token)
    {
        return token.AfterIsSpace || token.BetweenDigits ? null : new Italic(token);
    }
}