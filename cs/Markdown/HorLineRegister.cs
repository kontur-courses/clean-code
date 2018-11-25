using System.Linq;

namespace Markdown
{
    class HorLineRegister : IReadable
    {
        readonly char[] ableDigits = { '*', '-', '_' };

        public Token tryGetToken(string input, int startPos)
        {
            bool isStartSpaces = true;
            
            char currDigit = '\0';
            int digitCount = 0, i;

            for (i = startPos; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    if (isStartSpaces && i == 3)
                    {
                        return null;
                    }
                    continue;
                }
                isStartSpaces = false;

                if (currDigit == '\0')
                {
                    if (ableDigits.Contains(input[i]))
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

            return new Token("", "<hr>", "", 0, i - startPos); 

        }

    }
}
