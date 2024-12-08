namespace Markdown;

public class BoldTokenHtmlConverter : HtmlTokenConverter
{
    public BoldTokenHtmlConverter() : base(TokenType.Bold, "<strong>", "</strong>")
    {
    }
}
