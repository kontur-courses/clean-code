using Markdown.Lexer;
using Markdown.Parser;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var tokens = MdLexer.Tokenize(text);
        var parser = new TokenParser(tokens);
        var rootNode = parser.Parse();
        var html = rootNode.ToHtml();

        return html;
    }
}
