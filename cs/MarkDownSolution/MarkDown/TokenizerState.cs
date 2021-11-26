using System;
using System.Collections.Generic;

namespace MarkDown
{
    public class TokenizerState
    {
        public Token currentToken;
        public bool isEscaping = false;
        public bool isSplittingWord = false;
        public bool wasIntersected = false;

        public Dictionary<Type, bool> statesDict = new()
        {
            { typeof(BoldToken), false },
            { typeof(ItalicToken), false },
            { typeof(ListElementToken), false },
            { typeof(HeaderToken), false }
        };
    }
}
