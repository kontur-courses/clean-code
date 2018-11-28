using System;
using System.Collections.Generic;

namespace Markdown.Registers
{
    internal class HeaderRegister : BaseRegister
    {
        protected override int Priority => 1;

        public override bool IsBlockRegister => true;

        private bool skipSpaces(string input, int startIndex, int maxCount, out int index)
        {
            index = startIndex;
            for (int i = startIndex; i < input.Length; i++)
            {
                index = i;
                if (!Char.IsWhiteSpace(input[i]))
                    return true;
                if (maxCount != -1 && i - startIndex == maxCount)
                    return false;
            }

            return true;
        }

        public override Token TryGetToken(string input, int startPos)
        {
            var prefixDigitCount = 0;
            int valueStartIndex, valueEndIndex;

            if (!skipSpaces(input, startPos, 3, out var i))
                return null;

            while (i < input.Length && input[i] == '#')
            {
                prefixDigitCount++;
                i++;
            }

            if (prefixDigitCount == 0 || prefixDigitCount > 6 || i < input.Length && !Char.IsWhiteSpace(input[i]))
                return null;

            skipSpaces(input, i, -1, out i);
            valueStartIndex = valueEndIndex = i;

            for (; i < input.Length; i++)
            {
                if (input[i] == '\n')
                    break;

                if (input[i] == '#' && Char.IsWhiteSpace(input[i - 1]))
                {
                    while (i < input.Length && input[i] == '#')
                    {
                        i++;
                    }

                    i--;
                    continue;
                }

                if (input[i] != ' ')
                    valueEndIndex = i;
            }

            var value = valueEndIndex - valueStartIndex > 0
                ? input.Substring(valueStartIndex, valueEndIndex - valueStartIndex + 1)
                : "";

            return new Token(value, $"<h{prefixDigitCount}>", $"</h{prefixDigitCount}>", Priority, i - startPos, false);
        }
    }
}
