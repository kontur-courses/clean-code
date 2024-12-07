using Markdown.Domain.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Extensions
{
    public static class StringExtensions
    {
        public static bool SubstringContainsAny(this string s, int start, int length, Func<char, bool> check)
        {
            for (var i = 0; i < length; i++)
                if (check(s[start+i]))
                    return true;

            return false;
        }
    }
}
