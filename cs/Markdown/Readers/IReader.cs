using Markdown.Tags;

namespace Markdown.Readers
{
    public interface IReader
    {
        IToken ReadToken(string text, int position);
    }
}
