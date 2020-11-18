namespace Markdown
{
    public class MdTokenHtmlRenderer : TokenRenderer
    {
        public MdTokenHtmlRenderer(string text) : base(text)
        {
            AddToken<MdHeaderToken>((r, t) => $"<h1>{r.RenderSubtokens(t)}</h1>");
            AddToken<MdBoldToken>((r, t) => $"<strong>{r.RenderSubtokens(t)}</strong>");
            AddToken<MdItalicToken>((r, t) => $"<em>{r.RenderSubtokens(t)}</em>");
        }
    }
}