using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface IToken
    {
        public TokenType Type { get; }

        public string Value { get; }

        public IEnumerable<IToken> ChildTokens { get; }
    }
}