namespace Markdown;

public class Md
{
    public string Render(string textMd)
    {
        var parserFromMarkdown = new MarkdownParser(new MarkdownLexer());
        
        var parserToHtml = new HtmlParser(parserFromMarkdown);
        
        return parserToHtml.Parse(textMd);
    }
    
}
