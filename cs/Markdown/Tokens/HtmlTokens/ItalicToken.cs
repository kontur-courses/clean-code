using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class ItalicToken(IRenderable innerItem) : BasicMarkdownToken(innerItem), IRenderable
    {
        public readonly TokenTypes Type = TokenTypes.Italic;
        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderItalic(this);
        }
    }
}
