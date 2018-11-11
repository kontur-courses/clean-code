using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class CharReader : IReader
    {
        public IToken ReadToken(string text, int position)
        {
            return new Token(text[position].ToString());
        }
    }
}
