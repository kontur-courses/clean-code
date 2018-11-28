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
                if (input[i] != ' ')
                    return true;
                if (maxCount != -1 && i - startIndex == maxCount)
                    return false;
            }

            return true;
        }

        public override Token TryGetToken(string input, int startPos)
        {
            var isStartSpaces = true;
            int i, prefixDigitCount = 0;
            int valStartIndex, valEndIndex;

            if (!skipSpaces(input, startPos, 3, out i))
                return null;

            while (i < input.Length && input[i] == '#')
            {
                prefixDigitCount++;
                i++;
            }

            if (prefixDigitCount == 0 || prefixDigitCount > 6 || i < input.Length && input[i] != ' ')
                return null;

            skipSpaces(input, i, -1, out i);

            valStartIndex = valEndIndex = i;

            for (; i < input.Length; i++)
            {
                if (input[i] == '\n')
                {
                    break;
                }

                if (input[i] == '#' && input[i-1] == ' ')
                {
                    while (i < input.Length && input[i] == '#')
                    {
                        i++;
                    }

                    i--;
                    continue;
                }

                if (input[i] != ' ')
                    valEndIndex = i;
            }

            if (valEndIndex - valStartIndex <= 0)
                return new Token("", $"<h{prefixDigitCount}>", $"</h{prefixDigitCount}>", Priority, i - startPos, false);

            return new Token(input.Substring(valStartIndex, valEndIndex - valStartIndex + 1), $"<h{prefixDigitCount}>", $"</h{prefixDigitCount}>", Priority, i - startPos, false);
        }
    }
}
