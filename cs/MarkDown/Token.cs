using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDown
{
    public class Token
    {
        public readonly int Length;
        public readonly int Position;
        public readonly string Value;
        public readonly int SkipCount;
        public readonly bool TokenIsSkipping = false;
        public TokenType Type;
        public int indexNextToToken=> Position + Length;
        public int EndedIndex => Position + Length -1;

        public Token(string value, int position, int length, TokenType type)
        {
            if (length < 0) throw new ArgumentException("Length can't be less than zero");
            if (position < 0) throw new ArgumentException("Position can't be less than zero");
            Position = position;
            Length = length;
            Value = value;
            Type = type;
        }

        //использовал чтобы победить множество подчеркиваний подряд но не вышло
        public Token(int skipCount)
        {
            this.SkipCount = skipCount;
            TokenIsSkipping = true;

        }
    }
}
