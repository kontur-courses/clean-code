using Markdown.Maker;
using Markdown.Parser;
using Markdown.Rendered;
using Markdown.Tokens;
using Markdown.Tokens.HtmlToken;

namespace Markdown;

public class Md(IParser parser, IMaker<RootToken> maker, IRenderer<RootToken> renderer)
{
    

    public string Render(string input)
    {
        var tokens = parser.Parse(input);
        var model = maker.MakeFromTokens(tokens);
        return renderer.Render(model);
    }
}