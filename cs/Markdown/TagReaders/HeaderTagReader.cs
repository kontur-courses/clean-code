using Markdown.Tokens;

namespace Markdown.TagReaders;

internal class HeaderTagReader : ITagReaderWithNestedTokens
{
    public char Opener => '#';

    public char[] TagSymbols => ['#'];

    public int Read(string text, int start, List<TextToken> addTo, out string nestedTextToRead, List<TextToken> nestedTokensToAdd)
    {
        nestedTextToRead = "";

        if (!TryParse(text, start, out var end))
        {
            addTo.Add(new TextToken("#", start));
            return start + 1;
        }

        var raw = text.Substring(start, end - start);
        
        
        nestedTextToRead = text.Substring(start + 2, end - start - 2);

        var token = new HeaderToken(raw, start, nestedTextToRead, nestedTokensToAdd);

        addTo.Add(token);
        return end;
    }

    public int Read(string text, int start, List<TextToken> addTo)
    {
        return start;
    }

    private static bool TryParse(string text, int start, out int contentEnd)
    {
        contentEnd = 0;
        if (!((start == 0 || (start > 0 && text[start - 1] == '\n')) && start < text.Length - 2 && text[start + 1] == ' ' && text[start + 2] != ' '))
                return false;
        contentEnd = FindContentEnd(text, start);
        return true;

    }

    private static int FindContentEnd(string text, int start)
    {
        var lineFeed = text.IndexOf('\n', start);
        if (lineFeed < 0)
        {
            return text.Length;
        }

        return lineFeed > start && text[lineFeed - 1] == '\r' ? lineFeed - 1 : lineFeed;
    }
}
