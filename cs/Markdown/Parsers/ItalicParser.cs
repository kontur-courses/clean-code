using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class ItalicParser : ITokenParser
    {
        public Token ParseToken(IEnumerable<string> text, int position)
        {
            var tokenValue = new StringBuilder();
            foreach (var part in text)
            {
                tokenValue.Append(part);
            }
            var token = new Token(position, tokenValue.ToString(), TokenType.Italic);
            return token;
        }
    }
}
