using System.Collections.Generic;

namespace Markdown
{
    public interface IToken
    {
        public int Position { get; }
        public string Value { get; }
        public int EndPosition { get; }
        public TokenType Type { get; }
        public List<IToken> ChildTokens { get; }
        public bool CanHaveChildTokens { get; }
    }
}