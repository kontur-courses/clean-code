using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class StringScanner
    {
        public static bool IsOpeningTag(string paragraph, int index)
        {
            return index + 1 < paragraph.Length && paragraph[index + 1] != ' ' && !char.IsNumber(paragraph, index + 1);
        }

        public static bool IsClosingTag(string paragraph, int index)
        {
            return index > 0 && paragraph[index - 1] != ' ' && !char.IsNumber(paragraph, index - 1);
        }

        public static bool IsEmTag(string paragraph, int index) => paragraph[index] == '_';

        public static bool IsStrongTag(string paragraph, int index)
        {
            return index < paragraph.Length - 1 && paragraph[index] == '_' && paragraph[index + 1] == '_';
        }
    }
}
