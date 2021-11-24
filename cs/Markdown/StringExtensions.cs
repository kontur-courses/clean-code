using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public static class StringExtensions
    {
        public static bool ContainsDigit(this string str)
            => str.Any(char.IsDigit);
        
    }
}
