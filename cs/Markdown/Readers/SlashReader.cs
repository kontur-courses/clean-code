using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class SlashReader : IReader
    {
        public IToken ReadToken(string text, int position)
        {
            if (position < text.Length - 1 && text[position] == '\\')
                return new Token(text.Substring(position, 2));
            return null;
        }
    }
}
