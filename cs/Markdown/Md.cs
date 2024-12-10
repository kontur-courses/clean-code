using Markdown.Generator;
using Markdown.Parser;
using Markdown.Tokenizer;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var tokenizer = new MdTokenizer();
        var htmlGenerator = new HtmlGenerator();
        
        var tokens = tokenizer.Tokenize($"{RemoveCarriageTransfer(text)}\n");
        var root = TokenParser.Parse(tokens);
        
        return htmlGenerator.Render(root, tokens);
    }

    private static string RemoveCarriageTransfer(string text)
    {
        return text.Replace("\r\n", "\n").Replace("\n\r", "\n");
    }
}