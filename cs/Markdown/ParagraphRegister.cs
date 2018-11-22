using System;
using System.CodeDom.Compiler;
using System.Text;

namespace Markdown
{
    class ParagraphRegister : BaseRegister
    {
        public override Token tryGetToken(ref string input, int startPos)
        {
            if (input[startPos] == '\n' || Char.IsWhiteSpace(input[startPos]))
                return new Token("", "", "", 1, 1, false);

            string strValue;

            for (int i = startPos; i < input.Length - 1; i++)
            {
                if (input[i] == '\n' && input[i + 1] == '\n')
                {
                    strValue = input.Substring(startPos, i - startPos);
                    return new Token(strValue, "<p>", "</p>\n", 1, strValue.Length + 1, true);
                }
            }
            StringBuilder result = new StringBuilder();

            strValue = input.Substring(startPos, input.Length - startPos);
            foreach (var str in strValue.Split('\n'))
            {
                result.Append(str.TrimStart(' ', '\r'));
                result.Append('\n');
            }
            result.Remove(result.Length - 1, 1);
            return new Token(result.ToString(), "<p>", "</p>", 1, strValue.Length + 1, true); 
        }
    }
}
