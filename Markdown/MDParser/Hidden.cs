using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDParser
{
    public class Hidden : ITokenizer
    {
        public Token TryFindToken(string text, int idx)
        {
            if (idx + 1 == text.Length)
                return new Token("\\", TokenProperty.Normal);

            return Markers.Chars.Contains(text[idx + 1])
                ? new Token(text[idx + 1].ToString(), TokenProperty.Normal)
                : new Token(string.Empty, TokenProperty.Normal);
        }
    }

}
