using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class CharReader : IReader
    {
        public IToken ReadToken(string text, int position)
        {
            return new TextToken(text[position].ToString(), position);
        }
    }
}
