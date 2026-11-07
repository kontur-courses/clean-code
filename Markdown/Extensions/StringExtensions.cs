using Markdown.Paragraphs;

namespace Markdown.Extensions;

public static class StringExtensions
{
    public static IEnumerable<Paragraph> GetParagraphs(this string source)
        => ParagraphSplitter.GetParagraphs(source);
}
