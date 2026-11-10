using Markdown.Tokens;

namespace Markdown.TagReaders;

internal interface ITagReaderWithNestedTokens
{
    public Token Read(string text, int start, out int nestedStart, out Token endToken);
}
