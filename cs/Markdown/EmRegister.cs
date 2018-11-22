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

            string emDigit = input.startWith("*", startPos) ? "*" :
                (input.startWith("_", startPos) && !input.isInsideWord(startPos)) ? "_" : null;

            if (emDigit == null || (startPos + 1 >= input.Length) || Char.IsWhiteSpace(input[startPos + 1]))
                return null;

            int endIndex = input.indexOfCloseBracket(emDigit, startPos + 1);

            if (endIndex == -1)
                return null;

            string strOrig, strValue;
            strOrig = input.Substring(startPos, endIndex + 1 - startPos);
            strValue = input.Substring(startPos + 1, endIndex - 1 - startPos);

            return new Token("em", strOrig, strValue, "<em>", 1, "</em>");
        }
    }
}
