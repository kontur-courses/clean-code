using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;

namespace Markdown;

public class Md(ILexer lexer, IParser parser, IRenderer renderer)
{
    public string Render(string text)
    {
        var tokens = lexer.Tokenize(text.AsMemory());
        var document = parser.Parse(tokens);
        return renderer.Render(document);
    }

}