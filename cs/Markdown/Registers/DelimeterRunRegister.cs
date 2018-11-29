using System.Collections.Generic;

namespace Markdown.Registers
{
    internal abstract class DelimeterRunRegister : BaseRegister
    {
        protected abstract int DelimLen { get; }
        protected abstract string Prefix { get; }
        protected abstract string Suffix { get; }
        public override bool IsBlockRegister => false;

        protected HashSet<string> Delimeters;

        public override Token TryGetToken(string input, int startPos)
        {
            if (startPos + DelimLen >= input.Length)
                return null;

            var delimiter = GetDelimeter(input, startPos);

            if (!CheckPrefixCorrect(input, startPos, delimiter) ||
                !GetSuffixIndex(input, startPos, delimiter, out var suffIndex))
                return null;

            var strValue = input.Substring(startPos + DelimLen, suffIndex - DelimLen - startPos);
            return new Token(strValue, Prefix, Suffix, Priority, suffIndex - startPos + DelimLen, true);
        }

        private string GetDelimeter(string input, int startPos)
        {
            var supposedDelimiter = input.Substring(startPos, DelimLen);
            return Delimeters.Contains(supposedDelimiter) ? supposedDelimiter : null;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            var leftBorder = GetLeftBorder(input, startPos, delimiter);
            var rightBorder = GetRightBorder(input, startPos, delimiter);

            return !IsEscapeSymbol(input, startPos) &&
                   !(delimiter.Contains("_") && IsInsideWord(input, delimiter, leftBorder, rightBorder)) &&
                   IsNoSpaceAfter(input, rightBorder) &&
                   !IsPunctuationAfter(input, leftBorder, rightBorder);
        }

        private bool CheckSuffixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            var leftBorder = GetLeftBorder(input, startPos, delimiter);
            var rightBorder = GetRightBorder(input, startPos, delimiter);

            return !IsEscapeSymbol(input, startPos) && 
                   IsNoSpaceBefore(input, leftBorder) &&
                   !IsEmptyValueTag(input, delimiter, startPos, leftBorder, rightBorder) &&
                   !IsPunctuationBefore(input, leftBorder, rightBorder);
        }
        
        private bool GetSuffixIndex(string input, int startPos, string delimiter, out int suffIndex)
        {
            var nestedTagCount = 0;
            suffIndex = input.IndexOf(delimiter, startPos + DelimLen);

            while (suffIndex != -1)
            {
                if (CheckSuffixCorrect(input, suffIndex, delimiter))
                {
                    if (nestedTagCount == 0) 
                        return true;
                    
                    nestedTagCount--;
                    suffIndex = input.IndexOf(delimiter, suffIndex + DelimLen);
                    continue;
                }

                if (CheckPrefixCorrect(input, suffIndex, delimiter))
                {
                    nestedTagCount++;
                    suffIndex = input.IndexOf(delimiter, suffIndex + DelimLen);
                    continue;
                }

                suffIndex = input.IndexOf(delimiter, suffIndex + 1);
            }

            return false;
        }
        
        private bool IsInsideWord(string input, string delimiter, int leftBorder, int rightBorder)
        {
            return leftBorder > 0 && char.IsLetterOrDigit(input[leftBorder - 1])
                   && rightBorder + DelimLen != input.Length && char.IsLetterOrDigit(input[rightBorder + DelimLen]);
        }

        private bool IsEmptyValueTag(string input, string delimiter, int startPos, int leftBorder, int rightBorder)
        {
            return rightBorder - leftBorder == DelimLen && 
                   leftBorder == startPos &&
                   IsInsideWord(input, delimiter, leftBorder, rightBorder);
        }

        private bool IsNoSpaceAfter(string input, int rightBorder)
        {
            return rightBorder + DelimLen != input.Length && !char.IsWhiteSpace(input[rightBorder + DelimLen]);
        }

        private bool IsNoSpaceBefore(string input, int leftBorder)
        {
            return leftBorder != 0 && !char.IsWhiteSpace(input[leftBorder - 1]);
        }

        private bool IsPunctuationAfter(string input, int leftBorder, int rightBorder)
        {
            return char.IsPunctuation(input[rightBorder + DelimLen]) && leftBorder != 0 &&
                   !char.IsWhiteSpace(input[leftBorder - 1]) && !char.IsPunctuation(input[leftBorder - 1]);
        }
        
        private bool IsPunctuationBefore(string input, int leftBorder, int rightBorder)
        {
            return char.IsPunctuation(input[leftBorder - 1]) && rightBorder + DelimLen != input.Length &&
                   !char.IsWhiteSpace(input[rightBorder + DelimLen]) &&
                   !char.IsPunctuation(input[rightBorder + DelimLen]);
        }

        private bool IsEscapeSymbol(string input, int pos)
        {
            return pos > 0 && input[pos - 1] == '\\';
        }
        
        private int GetLeftBorder(string input, int startPos, string delimiter)
        {
            var leftBorder = startPos;
            while (leftBorder >= 0 && input.IndexOf(delimiter, leftBorder) == leftBorder)
                leftBorder--;
            leftBorder++;

            return leftBorder;
        }

        private int GetRightBorder(string input, int startPos, string delimiter)
        {
            var rightBorder = startPos;
            while (input.IndexOf(delimiter, rightBorder) == rightBorder)
                rightBorder++;
            rightBorder--;

            return rightBorder;
        }
    }
}