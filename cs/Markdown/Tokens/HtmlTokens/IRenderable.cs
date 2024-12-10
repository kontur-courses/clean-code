using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal interface IRenderable
    {
        void Render(IRenderer renderer);
    }
}
