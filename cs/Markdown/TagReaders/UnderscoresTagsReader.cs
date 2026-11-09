using Markdown.Tokens;

namespace Markdown.TagReaders;

public class UnderscoresTagsReader : ITagReader
{
    public char Opener => '_';

    public char[] TagSymbols => ['_'];

    public int Read(string text, int start, List<TextToken> tokens)
    {
        var end = start;
        while (end < text.Length && text[end] == '_')
            end++;

        var value = text.Substring(start, end - start);
        TextToken textToken;
        textToken = value.Length switch
        {
            1 => new UnderscoreToken(value, start),
            2 => new DoubleUnderscoreToken(value, start),
            _ => new TextToken(value, start) // "____" и длиннее — просто символы
        };
        tokens.Add(textToken);
        return end;
    }
}
