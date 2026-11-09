using Markdown.Domain;

namespace Markdown.Parsing;

public static class InlineTokenizer
{
    public static IReadOnlyList<Token> Tokenize(
        string text,
        int startIndex = 0)
    {
        var tokens = new List<Token>();
        var index = startIndex;

        while(index < text.Length)
        {
            if (IsEscape(text, index))
            {
                tokens.Add(new EscapeToken(index));
                index += 2;
                continue;
            }

            if (text[index] != '_')
            {
                index++;
                continue;
            }

            var start = index;

            while(index < text.Length && text[index] == '_')
            {
                index++;
            }

            var canClose = start > startIndex && !char.IsWhiteSpace(text[start - 1]);
            var canOpen = index < text.Length && !char.IsWhiteSpace(text[index]);

            if ((canOpen || canClose)
                && !IsDelimiterBetweenDigits(text, start, index - 1))
            {
                tokens.Add(new DelimiterRunToken(
                    start,
                    index - start,
                    canOpen,
                    canClose));
            }
        }

        return tokens;
    }

    private static bool IsEscape(string text, int index)
    {
        return text[index] == '\\'
               && index + 1 < text.Length
               && (text[index + 1] is '\\' or '_');
    }

    private static bool IsDelimiterBetweenDigits(string text, int startIndex, int endIndex)
    {
        return startIndex > 0 && endIndex < text.Length - 1
               && char.IsDigit(text[startIndex - 1])
               && char.IsDigit(text[endIndex + 1]);
    }
}