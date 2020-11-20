namespace Markdown
{
    public class MdHtmlTokenRenderer : TokenRenderer
    {
        public MdHtmlTokenRenderer(string text) : base(text)
        {
            AddToken<EscapedStringToken>((r, t) => t.GetEscapedString(r.Text));
            AddToken<MdDigitToken>((r, t) => text.Substring(t.StartPosition, t.Length));
            AddToken<MdHeaderToken>((r, t) => $"<h1>{r.RenderSubtokens(t)}</h1>");
            AddToken<MdBoldToken>((r, t) => $"<strong>{r.RenderSubtokens(t)}</strong>");
            AddToken<MdItalicToken>((r, t) => $"<em>{r.RenderSubtokens(t)}</em>");
            AddToken<MdLinkToken>((r, t) =>
                $"<a href=\"{r.RenderSubtokens(t.UrlToken)}\">{r.RenderSubtokens(t.TextToken)}</a>");
        }
    }
}