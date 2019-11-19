using System;
using System.Collections.Generic;
using System.Text;
using MarkDown.TokenParsers;

namespace MarkDown
{
    public class Token
    {
        public readonly Tag openTag;
        public readonly Tag closingTag;
        public TokenType Type;
        public int indexNextToToken => closingTag.indexNextToTag;
        public int EndedIndex => closingTag.EndedIndex;
        public int StartIndex => openTag.StartIndex;
        public int ValueLength=> closingTag.StartIndex-openTag.indexNextToTag;

        public Token(Tag open, Tag close)
        {
            if (open.Type != close.Type) throw new ArgumentException("Tag should have same types");
            openTag = open;
            closingTag = close;
            Type = open.Type;
        }
    }
}