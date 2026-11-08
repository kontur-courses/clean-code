using Markdown.Tokens;

namespace Markdown.TagReaders;

internal class LinkTagReader : ITagReaderWithNestedTokens
{
    public char Opener => '[';

    public char[] TagSymbols => ['[', ']', '(', ')'];


    //Пока не придумал, как скрыть этот метод Read для токенов с вложенными токенами
    public int Read(string text, int start, List<TextToken> tokens)
    {
        return start;
    }

    public int Read(string text, int start, List<TextToken> tokens, out string nestedTextToRead, List<TextToken> nestedTokensToAdd)
    {
        nestedTextToRead = "";

        if (!TryParse(text, start, out var link))
        {
            tokens.Add(new TextToken("[", start));
            return start + 1;
        }

        var raw = text.Substring(start, link.End - start);

        nestedTextToRead = link.Text;

        tokens.Add(new LinkToken(raw, start, link.Text, link.Url, nestedTokensToAdd));
        return link.End;
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
            text.Substring(urlStart, urlEnd - urlStart),
            urlEnd + 1);
        return true;
    }
    private static int FindTextEnd(string text, int from)
    {
        for (var i = from; i < text.Length; i++)
        {
            switch (text[i])
            {
                case '\\':
                    i++;
                    break;
                case '[':
                    return -1;
                case ']':
                    return i;
            }
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




    readonly record struct LinkMatch(string Text, string Url, int End);
}
