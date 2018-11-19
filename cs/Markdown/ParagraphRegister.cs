using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class ParagraphRegister : BaseRegister
    {
        public override Token tryGetToken(ref string input, int startPos)
        {
            int index = input.IndexOf('\n', startPos);
            string strOrig, strValue;

            if (index >= 0)
            {
                strOrig = input.Substring(startPos, index + 1 - startPos);
                strValue = input.Substring(startPos, index - startPos);
            }
            else
            {
                strOrig = input.Substring(startPos, input.Length - startPos);
                strValue = strOrig;
            }

            return new Token("p", strOrig, strValue, "<p>", 1, "<\\p>");   // TODO оптимизировать взятие подстроки
        }
    }
}
