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

            string strongDigits = input.startWith("**", startPos) ? "**" :
                                  input.startWith("__", startPos) ? "__" : null;

            if (strongDigits == null || (startPos + 2 >= input.Length || Char.IsWhiteSpace(input[startPos + 2])))
                return null;

            int endIndex = input.indexOfCloseBracket(strongDigits, startPos + 2);

            if (endIndex == -1)
                return null;

            string strOrig, strValue;
            strOrig = input.Substring(startPos, endIndex + 2 - startPos);
            strValue = input.Substring(startPos + 2, endIndex - 2 - startPos);

            return new Token("strong", strOrig, strValue, "<strong>", 0, "</strong>"); 
        }
    }
}
