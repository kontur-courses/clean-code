using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool TryGetChar(this string source, int index, out char ch)
        {
            if (index >= 0 && index < source.Length)
            {
                ch = source[index];
                return true;
            }

            ch = default(char);
            return false;
        }
        
        public static bool ContainsItOnIndex(this string source, string substring, int index)
        {
            if (index + substring.Length > source.Length)
                return false;

            var i = 0;
            while (index < source.Length && i < substring.Length)
            {
                if (source[index++] != substring[i++])
                    return false;
            }

            return true;
        }
    }
}
