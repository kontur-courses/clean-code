using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class MarkdownToken : IToken
    {
        public TokenType Type { get; }
        public string Value { get; }

        public MarkdownToken(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}