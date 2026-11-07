namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {

    }

    private static IEnumerable<Paragraph> GetParagraphs(string source)
    {
        var (start, current) = (0, 0);

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

    private static bool IsHeading(string source, int start)
    {
        return start + 1 < source.Length &&
               source[start] == '#' &&
               source[start + 1] == ' ';
    }
}