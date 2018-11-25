using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class EmphasisRegister : IReadable
    {
        protected int suffixLength = 1;
        protected string[] suffixes = {"*", "_"};
        protected int priority = 0;
        protected string[] tags = {"<em>","</em>"};

        public Token TryGetToken(string input, int startPos)
        {
            if (startPos + suffixLength >= input.Length)
                return null;

            var supposedPrefix = input.Substring(startPos, suffixLength);
            string suffixDigit = suffixes.Contains(supposedPrefix) ? supposedPrefix : null;

            if (CheckPrefixCorrect(input, startPos, suffixDigit) && GetSuffixIndex(input, startPos, suffixDigit, out var endIndex))
            {
                var strValue = input.Substring(startPos + suffixLength, endIndex - suffixLength - startPos);
                return new Token(strValue, tags[0], tags[1], priority, endIndex - startPos + suffixLength);
            }
            return null;            
        }

        private bool GetSuffixIndex(string input, int startPos, string digit, out int endIndex)
        {
            endIndex = input.GetIndexOfCloseTag(digit, startPos + suffixLength);

            if (endIndex == -1 || endIndex - startPos == 1)
                return false;

            for (int i = endIndex; i >= 0; i--)            
            {
                if (Char.IsWhiteSpace(input[i]))
                    return false;
                if (input[i] != digit[0])
                    break;
            }
            return true;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string suffixDigit)
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return false;

            if (suffixDigit == null || suffixDigit == suffixes[1] && input.IsInsideWord(startPos, suffixLength))
                return false;

            for (int i = startPos + suffixLength; i < input.Length; i++) 
            {
                if (Char.IsWhiteSpace(input[i]))
                    return false;
                if (input[i] != suffixDigit[0])
                    break;
            }
            return true;
        }
    }
}
