using Markdown.TokenFormatter.Renders;

namespace Markdown.Tokens.ConcreteTokens
{
    public class ImageToken : Token
    {
        private readonly string alt;
        private readonly string src;

        public ImageToken(string src, string alt = "") : base(TokenType.Image, src)
        {
            this.src = src;
            this.alt = alt;
        }

        public override string Render(IRenderer renderer) => renderer.RenderImage(src, alt);
    }
}