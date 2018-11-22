using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Markdown
{
    class StrongRegister : BaseRegister
    {
        public override Token tryGetToken(ref string input, int startPos)
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return null;

            string strongDigits = startWith("**", ref input, startPos) ? "**" :
                                  startWith("__", ref input, startPos) ? "__" : null;

            if (strongDigits == null || (startPos + 2 >= input.Length || Char.IsWhiteSpace(input[startPos + 2])))
                return null;

            int endIndex = indexOfCloseBracket(strongDigits, ref input, startPos + 2);

            if (endIndex == -1)
                return null;

            string strOrig, strValue;
            strOrig = input.Substring(startPos, endIndex + 2 - startPos);
            strValue = input.Substring(startPos + 2, endIndex - 2 - startPos);

            return new Token("strong", strOrig, strValue, "<strong>", 0, "</strong>"); 
        }

        public static bool startWith(string word, ref string str, int startPos)
        {
            if (str.Length - startPos < word.Length)
                return false;

            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] != str[i + startPos])
                {
                    return false;
                }
            }

            return true;
        }

        private static int indexOfCloseBracket(string word, ref string str, int startPos)
        {
            int endIndex = -1;
            for (int i = startPos; i <= str.Length - word.Length; i++)
            {
                if (startWith(word, ref str, i))
                {
                    if (!(Char.IsWhiteSpace(str[i - 1]) || str[i - 1] == '\\' ))
                        endIndex = i;
                }
            }
            return endIndex;
        }
    }
}
