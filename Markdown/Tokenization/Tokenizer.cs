using Markdown.Paragraphs;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace Markdown.Tokenization;

public static class Tokenizer
{
    public static IEnumerable<Token> Tokenize(Paragraph paragraph)
    {
        var (source, start) = (paragraph.Source, paragraph.Start);

        if (paragraph.IsHeading)
        {
            yield return new HeadingToken(false);
            start += 2; // Пропускаем "# "
        }

        foreach (var token in TokenizeRange(source, start, paragraph.End))
            yield return token;

        if (paragraph.IsHeading)
            yield return new HeadingToken(true);
    }

    private static IEnumerable<Token> TokenizeRange(string source, int start, int end)
    {
        var (current, textStart) = (start, start);

        while (current < end)
        {
            var token = TryReadSpecialToken(source, current, end);

            if (token is null)
            {
                current++;
                continue;
            }

            // записываем накопленный обычный текст
            if (textStart < current)
                yield return new TextToken(
                    source, textStart, current - textStart);

            yield return token;

            current += token.Length;
            textStart = current;
        }

        if (textStart < end)
            yield return new TextToken(
                source, textStart, end - textStart);
    }

    private static SourceToken? TryReadSpecialToken(string source, int current, int end)
    {
        if (source[current] == '\\' &&
            current + 1 < end &&
            IsEscapable(source[current + 1]))
            return new EscapedToken(source, current);

        if (source[current] == '\r' &&
            current + 1 < end &&
            source[current + 1] == '\n')
            return new LineBreakToken(source, current);

        if (source[current] == '_')
            return current + 1 < end && source[current + 1] == '_'
                ? new StrongToken(source, current)
                : new EmToken(source, current);

        if (source[current] == '[')
            return TryParseLink(source, current, end);

        return null;
    }

    private static LinkToken? TryParseLink(string source, int start, int end)
    {
        var textStart = start + 1;
        var current = textStart;

        while (current < end && source[current] != ']')
        {
            if (source[current] is '[' or '\r' or '\n')
                return null;

            current++;
        }

        if (current == textStart || current + 1 >= end ||
            source[current] != ']' || source[current + 1] != '(')
            return null;

        var urlStart = current + 2;
        current = urlStart;

        while (current < end)
        {
            if (source[current] == ')')
                return new LinkToken(
                    source, start, current - start + 1, 
                    urlStart, current - urlStart);

            if (char.IsWhiteSpace(source[current]) ||
                source[current] is '(' or '"')
                return null;

            current++;
        }

        return null;
    }

    private static bool IsEscapable(char symbol)
        => symbol is '\\' or '_' or '[';
}
