using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;
using System.Net;

namespace Markdown
{
    public class HtmlRenderer
    {
        public string Render(List<IToken> tokens)
        {
            var res = new StringBuilder();
            foreach (var token in tokens)
                res.Append(RenderToken(token));
            return res.ToString();
        }

        private string RenderToken(IToken token)
        {
            switch (token)
            {
                case TextToken textToken:
                    return RenderTextToken(textToken);
                case PairedTagToken pairedTagToken:
                    return RenderPairedTagToken(pairedTagToken);
                case LinkToken linkToken:
                    return RenderLinkToken(linkToken);
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected token type: {token.GetType()}");
            }
        }

        private string RenderTextToken(TextToken textToken)
        {
            return textToken.Value.Replace("&", "&amp;").Replace("<", "&lt;");
        }

        private string RenderPairedTagToken(PairedTagToken tagToken)
        {
            var res = new StringBuilder();
            res.Append($"<{tagToken.Name}>");
            foreach (var innerToken in tagToken.Children)
                res.Append(RenderToken(innerToken));
            res.Append($"</{tagToken.Name}>");
            return res.ToString();
        }

        private string RenderLinkToken(LinkToken linkToken)
        {
            var res = new StringBuilder();
            res.Append($"<a href=\"{linkToken.Link}\">");
            foreach (var innerToken in linkToken.Description)
                res.Append(RenderToken(innerToken));
            res.Append("</a>");
            return res.ToString();
        }
    }
}