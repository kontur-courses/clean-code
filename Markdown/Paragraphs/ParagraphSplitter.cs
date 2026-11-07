namespace Markdown.Paragraphs;

public static class ParagraphSplitter
{
    public static IEnumerable<Paragraph> GetParagraphs(this string source)
    {
        var start = SkipEmptyLines(source, 0);
        var current = start;

        while (current + 3 < source.Length)
        {
            if (source[current] == '\r' && source[current + 1] == '\n' &&
                source[current + 2] == '\r' && source[current + 3] == '\n')
            {
                current += 4;

                while (current + 1 < source.Length &&
                    source[current] == '\r' && source[current + 1] == '\n')
                    current += 2;

                yield return new Paragraph(
                    source, start, current, IsHeading(source, start));

                start = current;
            }
            else current++;
        }

        //последний параграф, дозаписать
        if (start < source.Length)
            yield return new Paragraph(
                source, start, source.Length, IsHeading(source, start));
    }

    private static int SkipEmptyLines(string source, int current)
    {
        while (current + 1 < source.Length &&
           source[current] == '\r' && source[current + 1] == '\n')
            current += 2;

        return current;
    }

    private static bool IsHeading(string source, int start)
        => start + 1 < source.Length &&
            source[start] == '#' &&
            source[start + 1] == ' ';
}
