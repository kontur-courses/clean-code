using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Token
    {
        public TagType TagType { get; }
        public int Start { get; }
        public int End { get; }
        public List<Token> NestedTokens { get; }

        public Token(TagType type, int start, int end)
        {
            TagType = type;
            Start = start;
            End = end;
            NestedTokens = new List<Token>();
        }
    }
}
