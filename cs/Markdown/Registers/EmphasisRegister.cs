using System;
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
            string suffixDelimiter = suffixes.Contains(supposedPrefix) ? supposedPrefix : null;

            if (CheckPrefixCorrect(input, startPos, suffixDelimiter) && GetSuffixIndex(input, startPos, suffixDelimiter, out var suffIndex))
            {
                var strValue = input.Substring(startPos + suffixLength, suffIndex - suffixLength - startPos);
                return new Token(strValue, tags[0], tags[1], priority, suffIndex - startPos + suffixLength);
            }
            return null;            
        }

        private bool GetSuffixIndex(string input, int startPos, string suffixDelimiter, out int suffIndex)
        {
            int nestedTagCount = 0;
            suffIndex = input.IndexOf(suffixDelimiter, startPos + suffixLength);

            while (suffIndex != -1)
            {
                if (CheckSuffixCorrect(input, suffIndex, suffixDelimiter))
                {
                    if (nestedTagCount > 0)
                    {
                        nestedTagCount--;
                        suffIndex = input.IndexOf(suffixDelimiter, suffIndex + suffixLength);
                        continue;
                    }
                    return true;
                }

                if (CheckPrefixCorrect(input, suffIndex, suffixDelimiter))
                {
                    nestedTagCount++;
                    suffIndex = input.IndexOf(suffixDelimiter, suffIndex + suffixLength);
                    continue;
                }
                suffIndex = input.IndexOf(suffixDelimiter, suffIndex + 1);
            } 
            return false;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string suffixDelimiter)
        {
            if (suffixDelimiter == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(suffixDelimiter, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(suffixDelimiter, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;

            if (shiftRight + suffixLength == input.Length || Char.IsWhiteSpace(input[shiftRight + suffixLength]))
                return false;

            if (Char.IsPunctuation(input[shiftRight + suffixLength])
                && (shiftLeft != 0 && !Char.IsWhiteSpace(input[shiftLeft - 1]) && !Char.IsPunctuation(input[shiftLeft - 1])))
                return false;

            if (startPos > 0 && input[startPos - 1] == '\\')        
                return false;

            if (suffixDelimiter == suffixes[1] 
                && shiftLeft > 0 && Char.IsLetterOrDigit(input[shiftLeft - 1]) 
                && shiftRight + suffixLength != input.Length && Char.IsLetterOrDigit(input[shiftRight + suffixLength]))
                return false;
            return true;
        }

        private bool CheckSuffixCorrect(string input, int startPos, string suffixDelimiter)
        {
            if (suffixDelimiter == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(suffixDelimiter, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(suffixDelimiter, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;


            if (suffixLength == 1 && shiftRight - shiftLeft == 1 && shiftLeft == startPos && CheckPrefixCorrect(input, startPos, suffixDelimiter))
                return false;


            if (shiftLeft == 0 || Char.IsWhiteSpace(input[shiftLeft - 1]))
                return false;

            if (Char.IsPunctuation(input[shiftLeft - 1])
                && (shiftRight + suffixLength != input.Length 
                    && !Char.IsWhiteSpace(input[shiftRight + suffixLength]) 
                    && !Char.IsPunctuation(input[shiftRight + suffixLength])))
                return false;

            if (input[startPos - 1] == '\\')       
                return false;

            return true;
        }
    }
}
