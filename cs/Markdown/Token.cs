using System;

namespace Markdown
{
    public class Token
    {
        public readonly int Length;
        public readonly int StartIndex;
        public readonly TagInfo TagInfo;

        public Token(int startIndex, int length, TagInfo tagInfo)
        {
            throw new NotImplementedException();
        }

        public static Token[] ParseStringToTokens(string sourceText)
        {
            throw new NotImplementedException();
        }

        public string ApplyTokensToString(string sourceText, Token[] tokens)
        {
            throw new NotImplementedException();
        }

        private void IncreaseTokensLength(Token[] tokens, Token lastAppliedToken)
        {
            throw new NotImplementedException();
        }
    }
}