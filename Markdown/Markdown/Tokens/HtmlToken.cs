using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class HtmlToken : IToken
    {
        public TokenType Type { get; }
        public string Value { get; }

        public HtmlToken(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}