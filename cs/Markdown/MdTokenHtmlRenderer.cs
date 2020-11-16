using System;

namespace Markdown
{
    public class MdTokenHtmlRenderer : MdTokenRenderer
    {
        public MdTokenHtmlRenderer(string text) : base(text) {}

        public override string Render(MdToken token)
        {
            if (token is MdRawTextToken raw) return Text.Substring(raw.StartPosition, raw.Length);
            if (token is MdHeaderToken header) return $"<h1>{RenderSubtokens(header)}</h1>";
            if (token is MdBoldToken bold) return $"<strong>{RenderSubtokens(bold)}</strong>";
            if (token is MdItalicToken italic) return $"<em>{RenderSubtokens(italic)}</em>";
            return null;
        }
    }
}