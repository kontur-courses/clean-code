using System.Collections.Generic;

namespace Markdown.Tokens
{
    public class HtmlToken : IToken
    {
        public TokenType Type { get; }
        public string Value { get; }

        public IEnumerable<IToken> ChildTokens { get; }

        public HtmlToken(TokenType type, string value, IEnumerable<HtmlToken> childTokens)
        {
            Type = type;
            Value = value;
            ChildTokens = childTokens;
        }
    }
}