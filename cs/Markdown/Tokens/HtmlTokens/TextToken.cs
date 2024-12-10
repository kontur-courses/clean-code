using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class TextToken(string text) : IRenderable
    {
        public readonly string Text = text
            ?? throw new ArgumentNullException(nameof(text),
                "Список внутренних токенов не может быть null.");
        public readonly TokenTypes Type = TokenTypes.Text;

        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderText(this);
        }
    }
}
