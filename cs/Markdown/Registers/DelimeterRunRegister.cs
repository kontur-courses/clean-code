namespace Markdown.Registers
{
    abstract class DelimeterRunRegister : BaseRegister
    {
        protected abstract int DelimLen { get; }
        protected abstract int Priority { get; }
        protected abstract string Prefix { get; }
        protected abstract string Suffix { get; }

        public override bool IsBlockRegister => false;

        public override Token TryGetToken(string input, int startPos)
        {
            if (startPos + DelimLen >= input.Length)
                return null;

            var supposedPrefix = input.Substring(startPos, DelimLen);
            var suffixDelimiter = supposedPrefix == new string('*', DelimLen)
                                  || supposedPrefix == new string('_', DelimLen)
                ? supposedPrefix
                : null;

            if (CheckPrefixCorrect(input, startPos, suffixDelimiter) &&
                GetSuffixIndex(input, startPos, suffixDelimiter, out var suffIndex))
            {
                var strValue = input.Substring(startPos + DelimLen, suffIndex - DelimLen - startPos);
                return new Token(strValue, Prefix, Suffix, Priority, suffIndex - startPos + DelimLen);
            }

            return null;
        }

        private bool GetSuffixIndex(string input, int startPos, string suffixDelimiter, out int suffIndex)
        {
            var nestedTagCount = 0;
            suffIndex = input.IndexOf(suffixDelimiter, startPos + DelimLen);

            while (suffIndex != -1)
            {
                if (CheckSuffixCorrect(input, suffIndex, suffixDelimiter))
                {
                    if (nestedTagCount > 0)
                    {
                        nestedTagCount--;
                        suffIndex = input.IndexOf(suffixDelimiter, suffIndex + DelimLen);
                        continue;
                    }

                    return true;
                }

                if (CheckPrefixCorrect(input, suffIndex, suffixDelimiter))
                {
                    nestedTagCount++;
                    suffIndex = input.IndexOf(suffixDelimiter, suffIndex + DelimLen);
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

            if (shiftRight + DelimLen == input.Length || char.IsWhiteSpace(input[shiftRight + DelimLen]))
                return false;

            if (char.IsPunctuation(input[shiftRight + DelimLen]) && shiftLeft != 0 &&
                !char.IsWhiteSpace(input[shiftLeft - 1]) && !char.IsPunctuation(input[shiftLeft - 1]))
                return false;

            if (startPos > 0 && input[startPos - 1] == '\\')
                return false;

            if (suffixDelimiter == new string('_', DelimLen)
                && shiftLeft > 0 && char.IsLetterOrDigit(input[shiftLeft - 1])
                && shiftRight + DelimLen != input.Length &&
                char.IsLetterOrDigit(input[shiftRight + DelimLen]))
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

            if (DelimLen == 1 && shiftRight - shiftLeft == 1 && shiftLeft == startPos &&
                CheckPrefixCorrect(input, startPos, suffixDelimiter))
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
    }
}