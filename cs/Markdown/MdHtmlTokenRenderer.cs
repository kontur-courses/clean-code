namespace Markdown
{
    public class MdHtmlTokenRenderer : TokenRenderer
    {
        public MdHtmlTokenRenderer(string text) : base(text)
        {
            AddToken<MdDigitToken>((r, t) => text.Substring(t.StartPosition, t.Length));
            AddToken<MdHeaderToken>((r, t) => $"<h1>{r.RenderSubtokens(t)}</h1>");
            AddToken<MdBoldToken>((r, t) => $"<strong>{r.RenderSubtokens(t)}</strong>");
            AddToken<MdItalicToken>((r, t) => $"<em>{r.RenderSubtokens(t)}</em>");
        }
    }
}