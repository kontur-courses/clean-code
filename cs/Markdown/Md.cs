namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var tokenizer = new MdTokenizer();
        var parser = new TokenParser();
        var htmlGenerator = new HtmlGenerator();
        
        var tokens = tokenizer.Tokenize(text);
        var root = parser.Parse(tokens);
        return htmlGenerator.GenerateHtml(root);

    }
}