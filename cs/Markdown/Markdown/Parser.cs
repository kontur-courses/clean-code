using System;
using System.Collections.Generic;


namespace Markdown
{
    class Parser
    {
        public static IEnumerable<StringPart> Parse(string str, char escapeSymbol='/')
        {
            if (str.Length == 0)
                return new List<StringPart> {new StringPart("")};

            var result = new List<StringPart>();
            var subStringParser = new SubstringParser(escapeSymbol);
            for (var i = 0; i < str.Length; i++)
            {
                var subStr = GetSubString(str, i);
                i += subStr.Length - 1;

                result.AddRange(subStringParser.ParseSubString(subStr));
            }

            return result;
        }
        
        public static string GetSubString(string str, int startIndex)
        {
            if (str == null)
                throw new ArgumentNullException();

            if (str.Length == 0)
                return str;

            var onlyWhitespaces = char.IsWhiteSpace(str[startIndex]);

            var result = "";
            for (; startIndex < str.Length; startIndex++)
            {
                if (char.IsWhiteSpace(str[startIndex]) == onlyWhitespaces)
                    result += str[startIndex];
                else
                    break;
            }

            return result;
        }
    }
}
