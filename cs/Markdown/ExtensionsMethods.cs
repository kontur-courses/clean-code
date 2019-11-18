using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class ExtensionsMethods
    {
        public static Token GetToken(this string str, int start, int end, string tag)
        {
            var length = end - start + 1;
            return new Token(str.Substring(start, length), start, length, tag);
        }

        public static bool IsLinkTag(this char s)
        {
            return s == '[' || s == ']' || s == '(' || s == ')';
        }

        public static bool Is_Tag(this char s)
        {
            return s == '_';
        }
    }
}