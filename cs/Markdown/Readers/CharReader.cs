using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class CharReader : IReader
    {
        public IToken ReadToken(string text, int position)
        {
            return new TextToken(text[position].ToString(), position);
        }
    }
}
