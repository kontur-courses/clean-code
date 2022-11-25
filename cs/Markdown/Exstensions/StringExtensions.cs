using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool HasElementAt(this string str,int index)
        {
            return index >= 0 && index < str.Length;
        }

        public static bool IsOpenTag(this string str, int index)
        {
            return str.HasElementAt(index+1) && !char.IsWhiteSpace(str[index + 1]) && !char.IsDigit(str[index + 1]);
        }

        public static bool IsCloseTag(this string str, int index)
        {
            return str.HasElementAt(index - 1) && !char.IsWhiteSpace(str[index - 1]) && !char.IsDigit(str[index - 1]);
        }

        public static bool CharInMiddleOfWord(this string str, int index)
        {
            if (!str.HasElementAt(index-1) || !str.HasElementAt(index + 1))
                return false;

            var left = str[index - 1];
            var right = str[index + 1];

            return right != ' ' && left != ' ' && !Char.IsDigit(right) && !Char.IsDigit(left);
        }

        public static bool UntilEndOfWordHasChar(this string line, int start, char stopChar, bool reverse = false)
        {
            Func<int, bool> predicate = reverse ? (int a) => a >= 0 : (int a) => a < line.Length;
            var step = reverse ? -1 : 1;
            for (int i = start; predicate(i) && !Char.IsWhiteSpace(line[i]); i+=step)
            {
                if (line[i] == stopChar && (line.HasElementAt(i-1) && line[i - 1] != '\\'))
                    return true;
            }
            return false;
        }
    }
}
