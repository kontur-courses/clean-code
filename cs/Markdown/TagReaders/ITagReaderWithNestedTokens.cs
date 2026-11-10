using Markdown.Tokens;

namespace Markdown.TagReaders;

internal interface ITagReaderWithNestedTokens : ITagReader
{
    public Token Read(string text, int start, out Token endToken);
}