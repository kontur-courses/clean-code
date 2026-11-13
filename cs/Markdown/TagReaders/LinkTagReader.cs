using Markdown.Tokens;

namespace Markdown.TagReaders;

public class LinkTagReader : ITagReader
{
    public char Opener => '[';

    public char[] TagSymbols => ['[', ']', '(', ')'];
    
    public Token Read(string text, int start, out Token endToken)
    {
        var startToken = new LinkToken("[", start, "")
        {
            Role = TokenRole.Literal
        };

        endToken = null;

        if (!TryParse(text, start, out var link))
        {
            return startToken;
        }

        var rawClosing = text.Substring(start + link.Text.Length + 1, link.Url.Length + 3);

        startToken = new LinkToken("[", start, link.Url)
        {
            Role = TokenRole.Opening
        };

        endToken = new LinkToken(rawClosing, start + link.Text.Length + 1, "")
        {
            Role = TokenRole.Closing
        };

        return startToken;
    }

    private static bool TryParse(string text, int start, out LinkMatch link)
    {
        link = default;

        var textEnd = FindTextEnd(text, start + 1);
        if (textEnd < 0 || textEnd == start + 1)
            return false;

        var urlStart = textEnd + 2;
        if (urlStart > text.Length || text[textEnd + 1] != '(')
            return false;

        var urlEnd = FindUrlEnd(text, urlStart);
        if (urlEnd < 0 || urlEnd == urlStart)
            return false;

        link = new LinkMatch(
            text.Substring(start + 1, textEnd - start - 1),
            text.Substring(urlStart, urlEnd - urlStart));
        return true;
    }

    private static int FindTextEnd(string text, int from)
    {
        for (var i = from; i < text.Length; i++)
        {
            if (text[i] == '[' || text[i] == '\n')
                break;
            
            if (text[i] == ']')
                return i;
            
            if (text[i] == '\\')
                i++;
        }

        return -1;
    }

    private static int FindUrlEnd(string text, int from)
    {
        for (var i = from; i < text.Length; i++)
        {
            var c = text[i];
            if (c == ')')
                return i;
            if (char.IsWhiteSpace(c) || c == '(' || c == '"')
                return -1;
        }

        return -1;
    }


    readonly record struct LinkMatch(string Text, string Url);
}