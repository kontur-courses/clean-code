using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class EmRegister : BaseRegister
    {
        protected int suffixLength = 1;
        protected string[] suffixes =  {"*", "_"};
        protected int priority = 0;
        protected string[] tags = {"<em>","</em>"};

        public override Token tryGetToken(ref string input, int startPos)
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return null;

            string digit = input.startWith(suffixes[0], startPos) ? suffixes[0] :
                (input.startWith(suffixes[1], startPos) && !input.isInsideWord(startPos)) ? suffixes[1] : null;

            if (digit == null || (startPos + suffixLength >= input.Length) 
                                || Char.IsWhiteSpace(input[startPos + suffixLength]) || input.startWith(digit, startPos+1))
                return null;

            int endIndex = input.indexOfCloseTag(digit, startPos + suffixLength);

            if (endIndex == -1)
                return null;

            var strValue = input.Substring(startPos + suffixLength, endIndex - suffixLength - startPos);
            return new Token(strValue, tags[0], tags[1], priority, endIndex - startPos + suffixLength, true);
        }
    }
}
