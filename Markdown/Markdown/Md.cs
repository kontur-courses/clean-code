using System.Text;
using Markdown.MarkdownToHtmlFilters;

namespace Markdown;

public class Md
{
    private readonly List<AbstractFilter> filters;

    public Md()
    {
        filters = new List<AbstractFilter>
        {
            // Order matters
            new NumericWordsFilter(),
            new TagRepeatFilter(),
            new EscapeFilter(),
            new CloseSingleTags(),
            new InWordsTagsFilter(),
            new OpenCloseDetectionFilter(),
            new UnclosedToText(),
            new BoldInsideItalicFilter()
        };
    }

    public string Render(string text)
    {
        var result = new StringBuilder();

        var i = 0;
        var lines = MarkdownParser.Parse(text);
        foreach (var line in lines)
        {
            RenderLine(line, result);
            if (i++ != lines.Count - 1)
                result.Append('\n');
        }

        return result.ToString();
    }

    public StringBuilder RenderLine(List<Token> tokens, StringBuilder? buffer = null)
    {
        buffer ??= new StringBuilder();
        filters.ForEach(filter => filter.Filter(tokens));
        MarkdownToHtmlConverter.Convert(tokens);
        foreach (var token in tokens)
            buffer.Append(token.Text);

        return buffer;
    }
}