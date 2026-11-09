using Markdown.Tokens;

namespace Markdown.TagReaders;

internal interface ITagReaderWithNestedTokens
{
    public int Read(string text, int start, List<TextToken> addTo, out int nestedStart, out TextToken endTextToken);
}
