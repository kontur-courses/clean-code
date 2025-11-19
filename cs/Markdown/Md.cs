namespace Markdown;

public class Md
{
    public string Render(string markdownText)
    {
        var tokenizer = new MarkdownTokenizer(markdownText);
        var tokens = tokenizer.Tokenize();
        var context = new ParserContext(tokens);
        var parserSelector = new ParseSelector(context);
        var parser = new MarkdownParser(context, parserSelector);
        var markdownDocument = parser.ParseTokens();
        var htmlText = markdownDocument.ToHtml();
        
        return htmlText;
    }
}