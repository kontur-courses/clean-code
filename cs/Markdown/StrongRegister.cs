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
            string strongDigits;

            if (startWith("**", ref input, startPos) && (startPos == 0 || input[startPos - 1] != '\\'))
            {
                strongDigits = "**";
            }
            else if (startWith("__", ref input, startPos) && (startPos == 0 || input[startPos - 1] != '\\'))
            {
                strongDigits = "__";
            }
            else
            {
                return null;
            }

            if (Char.IsWhiteSpace(input[startPos + 2]))
                return null;

            int endIndex = input.IndexOf(strongDigits, startPos + 2);

            if (endIndex == -1 || Char.IsWhiteSpace(input[endIndex - 1]) || input[endIndex - 1] == '\\')
                return null;       

            string strOrig, strValue;

            strOrig = input.Substring(startPos, endIndex + 2 - startPos);
            strValue = input.Substring(startPos + 2, endIndex - 2 - startPos);

            return new Token("strong", strOrig, strValue, "<strong>", 0, "<\\strong>"); 
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
    }
}
