namespace Markdown.Paragraphs;

public class ParagraphSplitter
{
    private readonly string _source;

    public ParagraphSplitter(string source)
    {
        _source = source;
    }

    public IEnumerable<Paragraph> GetParagraphs()
    {
        var start = SkipEmptyLines();
        var current = start;

        while (current + 3 < _source.Length)
        {
            if (_source[current] == '\r' && _source[current + 1] == '\n' &&
                _source[current + 2] == '\r' && _source[current + 3] == '\n')
            {
                current += 4;

                while (current + 1 < _source.Length &&
                    _source[current] == '\r' && _source[current + 1] == '\n')
                    current += 2;

                yield return new Paragraph(
                    _source, start, current, IsHeading(start));

                start = current;
            }
            else current++;
        }

        //последний параграф, дозаписать
        if (start < _source.Length)
            yield return new Paragraph(
                _source, start, _source.Length, IsHeading(start));
    }

    private int SkipEmptyLines(int current = 0)
    {
        while (current + 1 < _source.Length &&
           _source[current] == '\r' && _source[current + 1] == '\n')
            current += 2;

        return current;
    }

    private bool IsHeading(int start)
        => start + 1 < _source.Length &&
            _source[start] == '#' &&
            _source[start + 1] == ' ';
}
