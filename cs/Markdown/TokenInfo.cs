using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class TokenInfo
    {
        public int StartIndex { get; }
        public int EndIndex { get; private set; }
        public IToken Type { get; set; }
        public bool Closed { get; private set; }
        public List<TokenInfo> InnerTokens { get; } = new List<TokenInfo>();
        public StringBuilder PlainText { get; } = new StringBuilder();

        public TokenInfo(int startIndex, IToken tokenType)
        {
            StartIndex = startIndex;
            Type = tokenType;
        }

        public bool TryClose(TokenizerContextState contextState)
        {
            if (InnerTokens.Count == 0 || !Type.IsClosingKeySequence(contextState, this))
                return false;
            Closed = true;
            EndIndex = contextState.CurrentIndex;
            return true;
        }
    }
}