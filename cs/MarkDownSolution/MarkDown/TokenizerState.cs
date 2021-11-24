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
        public Dictionary<CaseType, bool> statesDict = new Dictionary<CaseType, bool>()
            {
                { CaseType.Italic, false },
                { CaseType.Bold, false }
            };
        public Token currentToken;
    }
}
