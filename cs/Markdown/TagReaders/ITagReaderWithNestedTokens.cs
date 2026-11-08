using Markdown.Tokens;

namespace Markdown.TagReaders;

internal interface ITagReaderWithNestedTokens : ITagReader
{
    public int Read(string text, int start, List<TextToken> addTo, out string nestedTextToRead, List<TextToken> nestedTokensToAdd);
}
