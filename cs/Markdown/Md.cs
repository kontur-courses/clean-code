using Markdown.Generator;
using Markdown.Parser;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var tokenizer = new MdTokenizer();
        var parser = new TokenParser();
        var htmlGenerator = new HtmlGenerator();
        
        var tokens = tokenizer.Tokenize($"{RemoveCarriageTransfer(text)}\n");
        var root = parser.Parse(tokens);
        
        return htmlGenerator.Render(root, tokens);
    }

    private static string RemoveCarriageTransfer(string text)
    {
        return text.Replace("\r\n", "\n").Replace("\n\r", "\n");
    }
}