using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Readers
{
    class SlashReader : IReader
    {
        public IToken ReadToken(string text, int position)
        {
            if (position < text.Length - 1 && text[position] == '\\')
                return new TextToken(text[position+1].ToString(), position + 1);
            return null;
        }
    }
}
