using Markdown.Abstractions;
using Markdown.Models;
using Markdown.Parsers;

namespace Markdown.Extensions;

public static class StringExtensions
{
    public static MarkedText Parse(this string text, ITextParser parser)
    {
        return parser.Parse(text);
    }

    public static MarkedText ParseMarkdown(this string text)
    {
        return text.Parse(new MarkdownParser());
    }
}