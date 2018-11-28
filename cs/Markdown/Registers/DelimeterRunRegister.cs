using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Markdown.Registers
{
    abstract class DelimeterRunRegister : BaseRegister
    {
        protected abstract int DelimLen { get; }
        protected abstract string Prefix { get; }
        protected abstract string Suffix { get; }
        public override bool IsBlockRegister => false;

        private readonly HashSet<string> delimeters;

        protected DelimeterRunRegister()
        {
            delimeters = new HashSet<string>(new[] { new string('*', DelimLen), new string('_', DelimLen) });
        }

        public override Token TryGetToken(string input, int startPos)
        {
            if (startPos + DelimLen >= input.Length)
                return null;

            var supposedDelimiter = input.Substring(startPos, DelimLen);
            var delimiter = delimeters.Contains(supposedDelimiter) ? supposedDelimiter : null;

            if (CheckPrefixCorrect(input, startPos, delimiter) &&
                GetSuffixIndex(input, startPos, delimiter, out var suffIndex))
            {
                var strValue = input.Substring(startPos + DelimLen, suffIndex - DelimLen - startPos);
                return new Token(strValue, Prefix, Suffix, Priority, suffIndex - startPos + DelimLen, true);
            }

            return null;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            getBorders(input, startPos, delimiter, out var leftBorder, out var rightBorder);

            if (rightBorder + DelimLen == input.Length || char.IsWhiteSpace(input[rightBorder + DelimLen]))
                return false;

            if (char.IsPunctuation(input[rightBorder + DelimLen]) && leftBorder != 0 &&
                !char.IsWhiteSpace(input[leftBorder - 1]) && !char.IsPunctuation(input[leftBorder - 1]))
                return false;

            if (startPos > 0 && input[startPos - 1] == '\\')
                return false;

            if (delimiter == new string('_', DelimLen)
                && leftBorder > 0 && char.IsLetterOrDigit(input[leftBorder - 1])
                && rightBorder + DelimLen != input.Length &&
                char.IsLetterOrDigit(input[rightBorder + DelimLen]))
                return false;
            return true;
        }

        private bool CheckSuffixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            getBorders(input, startPos, delimiter, out var leftBorder, out var rightBorder);

            if (DelimLen == 1 && rightBorder - leftBorder == 1 && leftBorder == startPos &&
                CheckPrefixCorrect(input, startPos, delimiter))
                return false;

            if (leftBorder == 0 || char.IsWhiteSpace(input[leftBorder - 1]))
                return false;

            if (char.IsPunctuation(input[leftBorder - 1]) && rightBorder + DelimLen != input.Length &&
                !char.IsWhiteSpace(input[rightBorder + DelimLen]) &&
                !char.IsPunctuation(input[rightBorder + DelimLen]))
                return false;

            if (input[startPos - 1] == '\\')
                return false;
            return true;
        }

        private bool GetSuffixIndex(string input, int startPos, string delimiter, out int suffIndex)
        {
            var nestedTagCount = 0;
            suffIndex = input.IndexOf(delimiter, startPos + DelimLen);

            while (suffIndex != -1)
            {
                if (CheckSuffixCorrect(input, suffIndex, delimiter))
                {
                    if (nestedTagCount > 0)
                    {
                        nestedTagCount--;
                        suffIndex = input.IndexOf(delimiter, suffIndex + DelimLen);
                        continue;
                    }

                    return true;
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

        private void getBorders(string input, int startPos, string delimiter, out int leftBorder, out int rightBorder)
        {
            leftBorder = startPos;
            while (leftBorder >= 0 && input.IndexOf(delimiter, leftBorder) == leftBorder)
                leftBorder--;
            leftBorder++;

            rightBorder = startPos;
            while (leftBorder < input.Length && input.IndexOf(delimiter, rightBorder) == rightBorder)
                rightBorder++;
            rightBorder--;
        }
    }
}