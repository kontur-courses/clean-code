namespace MarkdownProcessor.Markdown;

public class BoldConfig : ITagMarkdownConfig
{
    public string OpeningSign => "__";
    public string ClosingSign => OpeningSign;
    public TextType TextType => TextType.Bold;

    public ITag? CreateOrNull(Token token)
    {
        var afterIsSpace = string.IsNullOrWhiteSpace(token.After.ToString());

        return afterIsSpace || token.BetweenDigits
            ? null
            : new Bold(token, !string.IsNullOrWhiteSpace(token.Before.ToString()));
    }
}