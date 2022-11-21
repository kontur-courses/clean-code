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
        public static int FindChar(this string source, char ch, int startIndex, int step)
        {
            var ind = startIndex;
            while (ind > 0 && ind < source.Length)
            {
                if (source[ind] == ch)
                    break;
                ind += step;
            }

            return ind;
        }

        public static bool TryGetChar(this string source, int index, out char ch)
        {
            if (index >= 0 && index < source.Length)
            {
                ch = source[index];
                return true;
            }

            ch = '?';
            return false;
        }

        public static int[] AllIndicesOf(this string source, string substring)
        {
            var indices = new List<int>();
            int index = source.IndexOf(substring, 0);
            while (index > -1)
            {
                indices.Add(index);
                index = source.IndexOf(substring, index + substring.Length);
            }
            return indices.ToArray();
        }

        public static int CountOfCharBeforeIndex(this string source, char ch, int ind)
        {
            var count = 0;
            while (source.TryGetChar(ind - 1, out var actual) && actual == ch)
            {
                count++;
                ind--;
            }

            return count;
        }
    }
}
