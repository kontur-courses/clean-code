using System.Collections.Generic;

namespace Markdown.Tokens
{
    public interface IToken
    {
        public string Value { get; }
        public TokenType Type { get; }
        public int Length => Value.Length;
        public int Position { get; }
        public bool IsOpening { get; }
        public bool ShouldShowValue { get; }
        public bool ShouldBeIgnored { get; }
        public string OpeningTag { get; }
        public string ClosingTag { get; }
        public int SkipLength { get; }
        public bool ShouldBeClosed { get; }

        public void Validate(string markdown, IEnumerable<IToken> tokens);
        public void SetIsOpening(TokenBuilder tokenBuilder, string markdown, HashSet<TokenType> tokens);
    }
}
