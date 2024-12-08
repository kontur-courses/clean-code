namespace Markdown;

public class ItalicTokenHtmlConverter : HtmlTokenConverter
{
    public ItalicTokenHtmlConverter() : base(TokenType.Italic, "<em>", "</em>")
    {
    }
}
