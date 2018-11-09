using System.Collections.Generic;

namespace Markdown
{
    internal class Token
    {
        public int Position { get; private set; }
        public int Length { get; private set; }
        public TokenType Type { get; private set; }
        public string ReadOnlyProperty { get; private set; }
        
        public IEnumerable<Token> InnerTokens { get; private set; }
    }
}