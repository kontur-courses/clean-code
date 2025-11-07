namespace Markdown;

public class Md
{
    public string Render(string markdownText)
    {
        var tokenizer = new MarkdownTokenizer(markdownText);
        var tokens = tokenizer.Tokenize();
        var parser = new MarkdownParser(tokens);
        var markdownDocument = parser.ParseTokens();
        var htmlText = markdownDocument.ToHtml();
        
        return htmlText;
    }
}