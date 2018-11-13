using Markdown.Tags;

namespace Markdown.Readers
{
    interface IReader
    {
        IToken ReadToken(string text, int position);
    }
}
