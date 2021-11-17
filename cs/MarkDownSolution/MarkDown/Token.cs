using System.Collections.Generic;

namespace MarkDown
{
    public class Token
    {
        int start { get; }
        int length { get; }
        public readonly List<Token> nestedTokens = new List<Token>();
        public Token(int start, int length)
        {
            this.start = start;
            this.length = length;
        }
    }
}
