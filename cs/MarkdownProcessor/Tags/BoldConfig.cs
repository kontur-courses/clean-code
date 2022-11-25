namespace MarkdownProcessor.Tags;

public class BoldConfig : ITagMarkdownConfig
{
    public string OpeningSign => "__";
    public string ClosingSign => OpeningSign;
    public TextType TextType => TextType.Bold;

    public ITag? CreateOrNull(Token token)
    {
        return token.AfterIsSpace || token.BetweenDigits ? null : new Bold(token);
    }
}