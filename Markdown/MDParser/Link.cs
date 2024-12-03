using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.MDParser
{
    public class Link : ITokenizer
    {
        public Token TryFindToken(string text, int idx)
        {
            if (idx + 1 == text.Length)
                return new Token("[", TokenProperty.Normal);
            var i = idx + 1;
            var len = text.Length;
            var name = string.Empty;
            var address = string.Empty;

            while (i < len )
            {
                if (text[i] == ']' && text[i - 1] != '\\')
                {
                    name = text[(idx + 1)..i];
                    break;
                }
                i++;
            }

            if (name == string.Empty || i + 1 == len || text[i + 1] != '(' || i+2 == len)
                return new Token("[", TokenProperty.Normal);
            i+=2;
            var mark = i;
            while (i < len)
            {
                if (text[i] == ')' && text[i - 1] != '\\')
                {
                    address = text[(mark)..i];
                    break;
                }
                i++;
            }
            return address == string.Empty 
                ? new Token("[", TokenProperty.Normal) 
                : new Token(name + "+++" + address, TokenProperty.Link);
        }
    }
}
