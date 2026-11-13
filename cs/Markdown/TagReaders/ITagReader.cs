using Markdown.Tokens;

namespace Markdown.TagReaders;

public interface ITagReader
{
    public char Opener { get; }

    public char[] TagSymbols { get; }

    public Token Read(string text, int start, out Token endToken);
}