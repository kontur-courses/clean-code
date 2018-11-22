using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class EmRegister : BaseRegister
    {
        public override Token tryGetToken(ref string input, int startPos)
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return null;

            string emDigit = startWith("*", ref input, startPos) ? "*" :
                (startWith("_", ref input, startPos) && !isInsideWord(startPos, ref input)) ? "_" : null;

            if (emDigit == null || (startPos + 1 >= input.Length) || Char.IsWhiteSpace(input[startPos + 1]))
                return null;

            int endIndex = indexOfCloseBracket(emDigit, ref input, startPos + 1);

            if (endIndex == -1)
                return null;

            string strOrig, strValue;
            strOrig = input.Substring(startPos, endIndex + 1 - startPos);
            strValue = input.Substring(startPos + 1, endIndex - 1 - startPos);

            return new Token("em", strOrig, strValue, "<em>", 1, "</em>");
        }

        private static bool startWith(string word, ref string str, int startPos)
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
                    if (!(Char.IsWhiteSpace(str[i - 1]) || str[i - 1] == '\\' || isInsideWord(i, ref str)))
                        endIndex = i;
                }
            }
            return endIndex;
        }

        private static bool isInsideWord(int indexOfDigit, ref string text)
        {
            return (indexOfDigit != 0 && Char.IsLetterOrDigit(text[indexOfDigit - 1])) && 
                   (indexOfDigit != text.Length-1 && Char.IsLetterOrDigit(text[indexOfDigit + 1]));
        }
    }
}
