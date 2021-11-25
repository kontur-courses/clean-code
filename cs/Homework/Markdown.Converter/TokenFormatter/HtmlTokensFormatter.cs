using System;
using System.Collections.Generic;
using System.Text;
using Markdown.SyntaxParser;
using Markdown.TokenFormatter.Renders;
using Markdown.Tokens;

namespace Markdown.TokenFormatter
{
    public class HtmlTokensFormatter : ITokensFormatter
    {
        private readonly IRenderer renderer;

        private readonly Dictionary<TokenType, Func<string, string>> wrappers = new()
        {
            {TokenType.Bold, value => $"<strong>{value}</strong>"},
            {TokenType.Italics, value => $"<em>{value}</em>"},
            {TokenType.Header1, value => $"<h1>{value}</h1>"}
        };

        public HtmlTokensFormatter() => renderer = new HtmlRenderer();

        public string Format(IEnumerable<TokenTree> tokens)
        {
            if (tokens == null)
                throw new ArgumentNullException(nameof(tokens));

            var result = new StringBuilder();

            foreach (var tokenTree in tokens)
                result.Append(ProcessTree(tokenTree));

            return result.ToString();
        }

        private string ProcessTree(TokenTree tokenTree)
        {
            var tokenType = tokenTree.Token.TokenType;
            var formattedNodes = new StringBuilder(Format(tokenTree.Nodes));

            return formattedNodes.Length == 0
                ? tokenTree.Token.Render(renderer)
                : wrappers[tokenType](formattedNodes.ToString());
        }
    }
}