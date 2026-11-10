using Markdown.Tokens;

namespace Markdown.TagReaders;

internal interface ITagReaderWithNestedTokens
{
    public int Read(string text, int start, List<Token> addTo, out int nestedStart, out Token endToken);
}
