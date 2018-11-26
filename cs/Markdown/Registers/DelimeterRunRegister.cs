namespace Markdown.Registers
{
    abstract class DelimeterRunRegister : BaseRegister
    {
        protected abstract int DelimLen { get; }
        protected abstract string Prefix { get; }
        protected abstract string Suffix { get; }
        public override bool IsBlockRegister => false;

        public override Token TryGetToken(string input, int startPos)
        {
            if (startPos + DelimLen >= input.Length)
                return null;

            var supposedDelimiter = input.Substring(startPos, DelimLen);
            var delimiter = supposedDelimiter == new string('*', DelimLen)
                                  || supposedDelimiter == new string('_', DelimLen)
                ? supposedDelimiter
                : null;

            if (CheckPrefixCorrect(input, startPos, delimiter) &&
                GetSuffixIndex(input, startPos, delimiter, out var suffIndex))
            {
                var strValue = input.Substring(startPos + DelimLen, suffIndex - DelimLen - startPos);
                return new Token(strValue, Prefix, Suffix, Priority, suffIndex - startPos + DelimLen);
            }

            return null;
        }

        private bool CheckPrefixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(delimiter, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(delimiter, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;

            if (shiftRight + DelimLen == input.Length || char.IsWhiteSpace(input[shiftRight + DelimLen]))
                return false;

            if (char.IsPunctuation(input[shiftRight + DelimLen]) && shiftLeft != 0 &&
                !char.IsWhiteSpace(input[shiftLeft - 1]) && !char.IsPunctuation(input[shiftLeft - 1]))
                return false;

            if (startPos > 0 && input[startPos - 1] == '\\')
                return false;

            if (delimiter == new string('_', DelimLen)
                && shiftLeft > 0 && char.IsLetterOrDigit(input[shiftLeft - 1])
                && shiftRight + DelimLen != input.Length &&
                char.IsLetterOrDigit(input[shiftRight + DelimLen]))
                return false;
            return true;
        }

        private bool CheckSuffixCorrect(string input, int startPos, string delimiter)
        {
            if (delimiter == null)
                return false;

            var shiftLeft = startPos;
            while (shiftLeft >= 0 && input.IndexOf(delimiter, shiftLeft) == shiftLeft)
                shiftLeft--;
            shiftLeft++;

            var shiftRight = startPos;
            while (shiftLeft < input.Length && input.IndexOf(delimiter, shiftRight) == shiftRight)
                shiftRight++;
            shiftRight--;

            if (DelimLen == 1 && shiftRight - shiftLeft == 1 && shiftLeft == startPos &&
                CheckPrefixCorrect(input, startPos, delimiter))
                return false;

            if (shiftLeft == 0 || char.IsWhiteSpace(input[shiftLeft - 1]))
                return false;

            if (char.IsPunctuation(input[shiftLeft - 1]) && shiftRight + DelimLen != input.Length &&
                !char.IsWhiteSpace(input[shiftRight + DelimLen]) &&
                !char.IsPunctuation(input[shiftRight + DelimLen]))
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
    }
}