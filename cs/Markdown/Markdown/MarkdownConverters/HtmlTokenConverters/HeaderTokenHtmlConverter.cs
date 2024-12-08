namespace Markdown;

public class HeaderTokenHtmlConverter : HtmlTokenConverter
{
    public HeaderTokenHtmlConverter() : base(TokenType.Header, "<h1>", "</h1>")
    {
    }
}
