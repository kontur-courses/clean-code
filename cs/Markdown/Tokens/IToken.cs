using System.Collections.Generic;

namespace Markdown
{
    public interface IToken
    {
        public int Length { get; }
        public TokenType Type { get; }
        public string Text { get; }
        public List<IToken> SubTokens { get; set; }

        public bool IsTerminal { get; }
    }
}