using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public TagType TagType { get; }
        public int Start { get; }
        public int Finish { get; }
        public List<Token> NestedTokens { get; }

        public Token(TagType type, int start, int finish)
        {
            TagType = type;
            Start = start;
            Finish = finish;
            NestedTokens = new List<Token>();
        }
    }
}
