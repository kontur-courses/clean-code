using System;
using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown
{
    public class HtmlRenderer
    {
        public string Render(List<IToken> tokens)
        {
            var res = new StringBuilder();
            foreach (var token in tokens)
                res.Append(HandleToken(token));
            return res.ToString();
        }

        private string HandleToken(IToken token)
        {
            switch (token.Type)
            {
                case TokenType.Text:
                    return HandleTextToken((TextToken) token);
                case TokenType.PairedTag:
                    return HandlePairedTagToken((PairedTagToken) token);
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected token type: {nameof(token.Type)}");
            }
        }

        private string HandleTextToken(TextToken textToken)
        {
            return textToken.Value.Replace("&", "&amp;").Replace("<", "&lt;");
        }

        private string HandlePairedTagToken(PairedTagToken tagToken)
        {
            var res = new StringBuilder();
            res.Append($"<{tagToken.Name}>");
            foreach (var innerToken in tagToken.Children)
                res.Append(HandleToken(innerToken));
            res.Append($"</{tagToken.Name}>");
            return res.ToString();
        }
    }
}