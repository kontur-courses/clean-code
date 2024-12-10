using Markdown.Tokens.HtmlTokens;
using System.Text;

namespace Markdown.Renderers
{
    internal class RendererHTML : IRenderer
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void RenderBold(BoldToken boldToken)
        {
            ArgumentNullException.ThrowIfNull(boldToken);

            _stringBuilder.Append("<strong>");
            boldToken.InnerItem.Render(this);
            _stringBuilder.Append("</strong>");
        }

        public void RenderHeader(HeaderToken headerToken)
        {
            ArgumentNullException.ThrowIfNull(headerToken);

            _stringBuilder.Append("<h1>");
            headerToken.InnerItem.Render(this);
            _stringBuilder.Append("</h1>");
        }

        public void RenderItalic(ItalicToken italicToken)
        {
            ArgumentNullException.ThrowIfNull(italicToken);

            _stringBuilder.Append("<em>");
            italicToken.InnerItem.Render(this);
            _stringBuilder.Append("</em>");
        }

        public void RenderSet(SetToken setToken)
        {
            ArgumentNullException.ThrowIfNull(setToken);

            foreach (var item in setToken.InnerItems)
            {
                item.Render(this);
            }
        }

        public void RenderText(TextToken text)
        {
            ArgumentNullException.ThrowIfNull(text);

            _stringBuilder.Append(text.Text);
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}
