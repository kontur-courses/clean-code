using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDParser
{
    public class Italic : ITokenizer
    {
        public Token? TryFindToken(string text, int idx)
        {
            if (idx + 1 == text.Length)
                return new Token("_", TokenProperty.Normal);

            if (text[idx + 1] == '_')
                return new Bold().TryFindToken(text, idx);

            var i = idx + 1;
            var len = text.Length;
            var hasDigit = false;
            while (i < len)
            {
                if (int.TryParse(text[i].ToString(), out _))
                {
                    hasDigit = true;
                    i++;
                    continue;
                }

                if (text[i] == ' ')
                    return new Token(text[idx..i], TokenProperty.Normal);

                if (text[i] != '_')
                {
                    i++;
                    continue;
                }

                if (text[i - 1] == '\\')
                    return new Token(text[idx..(i - 1)], TokenProperty.Normal);

                return hasDigit
                    ? new Token(text[idx..i], TokenProperty.Normal)
                    : new Token(text[(idx + 1)..i], TokenProperty.Italic);
            }
            return new Token(text[idx..], TokenProperty.Normal);
        }
    }
}
