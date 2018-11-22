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
        public override Token tryGetToken(ref string input, int startPos)       // TODO объединить с EmRegister
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return null;

            string strongDigits = input.startWith("**", startPos) ? "**" :
                                  input.startWith("__", startPos) ? "__" : null;

            if (strongDigits == null || (startPos + 2 >= input.Length || Char.IsWhiteSpace(input[startPos + 2])))
                return null;

            int endIndex = input.indexOfCloseTag(strongDigits, startPos + 2);

            if (endIndex == -1)
                return null;

            var strValue = input.Substring(startPos + 2, endIndex - 2 - startPos);

            return new Token(strValue, "<strong>", "</strong>", 0, endIndex - startPos + 4, true); 
        }
    }
}
