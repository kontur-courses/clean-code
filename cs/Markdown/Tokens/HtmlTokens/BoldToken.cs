using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class BoldToken(IRenderable innerItem) : BasicMarkdownToken(innerItem), IRenderable
    {
        public readonly TokenTypes Type = TokenTypes.Bold;
        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderBold(this);
        }
    }
}
