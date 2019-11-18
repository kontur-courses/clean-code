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
        public List<TokenInfo> Blocks { get; } = new List<TokenInfo>();
        public List<TokenInfo> InnerTokens { get; } = new List<TokenInfo>();
        public StringBuilder PlainText { get; } = new StringBuilder();

        public TokenInfo(int startIndex, IToken tokenType)
        {
            var openingSequenceLength = tokenType.OpeningSequence.Length;
            StartIndex = openingSequenceLength > 1 ? startIndex : startIndex - (openingSequenceLength - 1);
            Type = tokenType;
        }

        public bool TryClose(TokenizerContextState contextState)
        {
            if (Type is IComplexTokenBlock block && block.ClosingSequence == block.Parent.ClosingSequence)
                return false;
            if (InnerTokens.Count == 0 && !(Type is IComplexToken) || 
                !Type.IsClosingKeySequence(contextState, this))
                return false;
            Closed = true;
            EndIndex = contextState.CurrentIndex;
            return true;
        }
    }
}