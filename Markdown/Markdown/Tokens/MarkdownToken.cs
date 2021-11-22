using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class MarkdownToken : IToken
    {
        public TokenType Type { get; }
        public string Value { get; }
        public IEnumerable<IToken> ChildTokens { get; }

        public MarkdownToken(TokenType type, string value, IEnumerable<MarkdownToken> childTokens)
        {
            Type = type;
            Value = value;
            ChildTokens = childTokens;
        }
    }
}