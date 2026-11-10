using Markdown.Tokens;

namespace Markdown.TagReaders;

public class HeaderTagReader : ITagReaderWithNestedTokens, ITagReader
{
    public char Opener => '#';

    public char[] TagSymbols => ['#'];

    public int Read(string text, int start, List<Token> addTo, out int nestedStart, out Token endToken)
    {
        var startToken = new HeaderToken("#", start)
        {
            Role = TokenRole.Literal
        };

        endToken = null;
        nestedStart = start + 2;

        if (!TryParse(text, start, out var contentEnd))
        {
            addTo.Add(startToken);
            return start + 1;
        }

        var raw = text.Substring(start, contentEnd - start);

        startToken = new HeaderToken(raw, start)
        {
            Role = TokenRole.Opening
        };

        endToken = new HeaderToken(raw, contentEnd)
        {
            Role = TokenRole.Closing
        };
        
        addTo.Add(startToken);
        return contentEnd;
    }

    public int Read(string text, int start, List<Token> addTo)
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
