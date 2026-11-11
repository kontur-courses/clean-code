using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Parser.Inline
{
    public static class Escaper
    {
        public static HashSet<int> GetEscapedIndex(string text)
        {
            var result = new HashSet<int>();

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == '\\' && !result.Contains(i))
                {
                    result.Add(i + 1);
                }
            }

            return result;
        }
    }
}
