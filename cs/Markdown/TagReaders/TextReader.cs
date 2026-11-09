using Markdown.Tokens;

namespace Markdown.TagReaders;

internal class TextReader : ITagReader
{
    public char Opener { get; }

    public char[] TagSymbols { get; }

    public int Read(string text, int start, List<TextToken> tokens)
    {
        var end = start;
        while (end < text.Length && text[end] != '\\' && text[end] != '\n' && text[end] != '_' && text[end] != '['&& text[end] != ']')
            end++;

        tokens.Add(new TextToken(text.Substring(start, end - start), start));
        return end;
    }
}
