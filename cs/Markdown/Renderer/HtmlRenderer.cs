using Markdown.Rendered;
using Markdown.Tokens.HtmlToken;
using Markdown.Tokens.StringToken;

namespace Markdown;

public class HtmlRenderer : IRenderer<RootToken>
{
    public string Render(RootToken root)
    {
        return root.ToString() ?? string.Empty;
    }
}