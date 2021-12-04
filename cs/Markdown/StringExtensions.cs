using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool ContainsDigit(this string str)
        {
            return str.Any(c => Char.IsDigit(c));
        }
    }
}
