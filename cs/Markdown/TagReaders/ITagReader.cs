using Markdown.Tokens;

namespace Markdown.TagReaders;

public interface ITagReader
{
    public char Opener { get; }

    public char[] TagSymbols {  get; }

    public int Read(string text, int start, List<TextToken> addTo);
}
