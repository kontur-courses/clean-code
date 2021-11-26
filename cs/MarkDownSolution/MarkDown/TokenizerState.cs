using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public class TokenizerState
    {
        public bool isEscaping = false;
        public bool isSplittingWord = false;
        public bool wasIntersected = false;
        public Dictionary<Type, bool> statesDict = new Dictionary<Type, bool>()
        {
            { typeof(BoldToken), false },
            { typeof(ItalicToken), false }
        };
        public Token currentToken;
    }
}
