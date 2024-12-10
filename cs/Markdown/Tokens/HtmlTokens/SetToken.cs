using Markdown.Renderers;

namespace Markdown.Tokens.HtmlTokens
{
    internal class SetToken(params IRenderable[] innerItems) : IRenderable
    {
        private readonly IRenderable[] _innerItems = innerItems
            ?? throw new ArgumentNullException(nameof(innerItems),
                "Список внутренних токенов не может быть null.");
        public readonly TokenTypes Type = TokenTypes.Set;
        public IRenderable[] InnerItems => _innerItems;

        public void Render(IRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(renderer);

            renderer.RenderSet(this);
        }
    }
}
