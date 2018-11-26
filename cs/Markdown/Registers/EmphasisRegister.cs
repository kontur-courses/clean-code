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

        private bool GetSuffixIndex(string input, int startPos, string suffixDigit, out int endIndex)
        {
            int nestedTagNum = 0;
            endIndex = input.IndexOf(suffixDigit, startPos + suffixLength);

            while (endIndex != -1)
            {


                if (CheckSuffixCorrect(input, endIndex, suffixDigit))
                {
                    if (nestedTagNum > 0)
                    {
                        nestedTagNum--;
                        endIndex = input.IndexOf(suffixDigit, endIndex + suffixLength);
                        continue;
                    }
                    return true;
                }

                if (CheckPrefixCorrect(input, endIndex, suffixDigit))
                {
                    nestedTagNum++;
                    endIndex = input.IndexOf(suffixDigit, endIndex + suffixLength);
                    continue;
                }

                endIndex = input.IndexOf(suffixDigit, endIndex + 1);
            } 

            return false;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string suffixDigit)
        {
            if (suffixDigit == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(suffixDigit, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(suffixDigit, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;

            //if (suffixLength == 1 && shiftRight - shiftLeft == 1)
            //    return false;

            if (shiftRight + suffixLength == input.Length || Char.IsWhiteSpace(input[shiftRight + suffixLength]))
                return false;

            if (Char.IsPunctuation(input[shiftRight + suffixLength])
                && (shiftLeft != 0 && !Char.IsWhiteSpace(input[shiftLeft - 1]) && !Char.IsPunctuation(input[shiftLeft - 1])))
                return false;

            if (startPos > 0 && input[startPos - 1] == '\\')        
                return false;

            if (suffixDigit == suffixes[1] 
                && shiftLeft > 0 && Char.IsLetterOrDigit(input[shiftLeft - 1]) 
                && shiftRight + suffixLength != input.Length && Char.IsLetterOrDigit(input[shiftRight + suffixLength]))
                return false;
            return true;
        }

        private bool CheckSuffixCorrect(string input, int startPos, string suffixDigit)
        {
            if (suffixDigit == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(suffixDigit, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(suffixDigit, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;


            if (suffixLength == 1 && shiftRight - shiftLeft == 1 && shiftLeft == startPos && CheckPrefixCorrect(input, startPos, suffixDigit))
                return false;


            if (shiftLeft == 0 || Char.IsWhiteSpace(input[shiftLeft - 1]))
                return false;

            if (Char.IsPunctuation(input[shiftLeft - 1])
                && (shiftRight + suffixLength != input.Length 
                    && !Char.IsWhiteSpace(input[shiftRight + suffixLength]) 
                    && !Char.IsPunctuation(input[shiftRight + suffixLength])))
                return false;

            if (input[startPos - 1] == '\\')        // TODO подумать над проверкой входящих параметров
                return false;

            //if (suffixDigit == suffixes[1] && (CheckSuffixCorrect(input, startPos, suffixDigit) &&
            //                                   Char.IsPunctuation(input[startPos - 1])))
            //    return false;
            return true;
        }
    }
}
