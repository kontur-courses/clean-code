using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class TagsPair
    {
        public readonly TagType PairType;
        public int StartPosition => StartToken.PositionInText;
        public int EndPosition => EndToken.PositionInText;
        public readonly Token StartToken;
        public readonly Token EndToken;

        public TagsPair(TagType type, Token startToken, Token endToken)
        {
            PairType = type;
            StartToken = startToken;
            EndToken = endToken;
        }
    }
}
