using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    internal class TokenInfo
    {
        public int StartIndex { get; }
        public int EndIndex { get; }
        public IToken TokenType { get; set; }
        public List<TokenInfo> InnerTokens { get; }
        public bool Closed { get; private set; }
        public bool IsComplex { get; private set; }
        public StringBuilder PlainText { get; private set; }

        public TokenInfo(int startIndex, IToken possibleTokenType)
        {
        }

        public void AddInnerToken(TokenInfo tokenInfo)
        {
        }

        public bool TryClose(Context currentContext, int currentIndex, string closingKeySequence)
        {
            throw new NotImplementedException();
        }
    }
}