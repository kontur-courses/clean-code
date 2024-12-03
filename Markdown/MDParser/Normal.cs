using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDParser
{
    public class Normal : ITokenizer
    {
        public Token TryFindToken(string text, int idx)
        {
            var i = idx;
            var len = text.Length;
            while (i < len)
            {
                if (Markers.Chars.Contains(text[i]))
                    return new Token(text[idx..i], TokenProperty.Normal);
                else
                {
                    i++;
                    continue;
                }
            }
            return new Token(text[idx..], TokenProperty.Normal);
        }
    }
}
