using System;

namespace Markdown
{
    class EmphasisRegister : IReadable
    {
        protected int suffixLength = 1;
        protected string[] suffixes =  {"*", "_"};
        protected int priority = 0;
        protected string[] tags = {"<em>","</em>"};

        public Token tryGetToken(string input, int startPos)
        {
            if (startPos != 0 && input.Length > 0 && input[startPos - 1] == '\\')
                return null;

            string digit = input.startWith(suffixes[0], startPos) ? suffixes[0] :
                (input.startWith(suffixes[1], startPos) && !input.isInsideWord(startPos)) ? suffixes[1] : null;

            if (digit == null || (startPos + suffixLength >= input.Length))
                return null;

            for (int i = startPos + suffixLength; i < input.Length; i++)        // Проверка что нет пробела после префикса TODO вынести в отдельный метод
            {
                if (Char.IsWhiteSpace(input[i]))
                    return null;
                if(input[i] != digit[0])
                    break;
            }

            int endIndex = input.indexOfCloseTag(digit, startPos + suffixLength);

            if (endIndex == -1 || endIndex - startPos  == 1)
                return null;

            for (int i = endIndex; i >= 0; i--)                 // Проверка что нет пробела перед суффиксом TODO вынести в отдельный метод
            {
                if (Char.IsWhiteSpace(input[i]))
                    return null;
                if (input[i] != digit[0])
                    break;
            }

            var strValue = input.Substring(startPos + suffixLength, endIndex - suffixLength - startPos);
            return new Token(strValue, tags[0], tags[1], priority, endIndex - startPos + suffixLength, true);
        }
    }
}
