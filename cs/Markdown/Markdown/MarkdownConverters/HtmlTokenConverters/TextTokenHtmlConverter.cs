namespace Markdown;

public class TextTokenHtmlConverter : HtmlTokenConverter
{
    public TextTokenHtmlConverter() : base(TokenType.SimpleText, string.Empty, string.Empty)
    {
    }
}
