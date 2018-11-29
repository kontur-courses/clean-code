using System.Collections.Generic;


namespace Markdown.Registers
{

    internal class HorLineRegister : BaseRegister
    {
        private readonly HashSet<char> aviableCharacters = new HashSet<char>(new[] {'*', '-', '_'});
        protected override int Priority => 1;

        public override Token TryGetToken(string input, int startPos)
        {
            var isStartSpaces = true;

            var currDigit = '\0';
            int i, digitCount = 0;

            for (i = startPos; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (isStartSpaces && i == 3) return null;
                    continue;
                }

                isStartSpaces = false;

                if (currDigit == '\0')
                {
                    if (aviableCharacters.Contains(input[i]))
                    {
                        currDigit = input[i];
                        digitCount += 1;
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (input[i] == currDigit)
                {
                    digitCount += 1;
                }
                else
                {
                    if (input[i] == '\n')
                        break;

                    return null;
                }
            }

            if (digitCount < 3)
                return null;

            return new Token("", "<hr />", "", Priority, i - startPos, false);
        }
    }
}