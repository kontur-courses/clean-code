using System;
using System.Text;

namespace Markdown
{
    internal class Context
    {
        private TokenContainer _mainToken;
        private TokenContainer _lastToken;
        
        private StringBuilder _currentKeySequence;
        private bool _readingKeySequence;
        private string _sourceText;
        private bool _inComplexToken;
        private bool _inPlainText;

        public Context(string sourceText)
        {
        }

        public void AddChildToken(TokenInfo newTokenInfo)
        {
        }

        public bool TryCloseToken(int currentIndex, string closingKeySequence)
        {
            throw new NotImplementedException();
        }

        public void AddAsPlainText(char newChar)
        {
        }
    }

    internal class TokenContainer
    {
        public TokenContainer _parentToken;
        public TokenInfo TokenInfo;
    }   
}