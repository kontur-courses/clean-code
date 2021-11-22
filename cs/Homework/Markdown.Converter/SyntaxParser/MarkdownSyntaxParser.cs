using System;
using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown.SyntaxParser
{
    public class MarkdownSyntaxParser : ISyntaxParser
    {
        public IEnumerable<TokenTree> Parse(IEnumerable<Token> lexedTokens)
        {
            var tokens = lexedTokens ?? throw new ArgumentNullException(nameof(lexedTokens));
            return new ParseContext(tokens).Parse();
        }
    }
}