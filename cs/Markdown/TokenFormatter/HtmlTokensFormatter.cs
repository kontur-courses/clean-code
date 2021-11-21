using System;
using System.Collections.Generic;
using System.Text;
using Markdown.SyntaxParser;
using Markdown.Tokens;

namespace Markdown.TokenFormatter
{
    public class HtmlTokensFormatter : ITokensFormatter
    {
        private readonly Dictionary<TokenType, Func<string, string>> wrappers = new()
        {
            {TokenType.Bold, value => $"<strong>{value}</strong>"},
            {TokenType.Italics, value => $"<em>{value}</em>"},
            {TokenType.Header1, value => $"<h1>{value}</h1>"}
        };

        public string Format(IEnumerable<TokenTree> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));
            var result = new StringBuilder();

            foreach (var tokenTree in tokens)
            {
                var tokenType = tokenTree.Token.TokenType;
                if (tokenType == TokenType.Text)
                    result.Append(tokenTree.Token.Value);
                else
                {
                    var wrapper = wrappers[tokenType];
                    result.Append(wrapper(Format(tokenTree.Nodes)));
                }
            }

            return result.ToString();
        }
    }
}