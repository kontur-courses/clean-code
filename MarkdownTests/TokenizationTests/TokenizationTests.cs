using Markdown.Extensions;
using Markdown.Paragraphs;
using Markdown.Tokenization.Tokens.Abstractions;

namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    private static Token[] Tokenize(string source, bool isHeading = false)
    {
        var paragraph = new Paragraph(source, 0, source.Length, isHeading);
        return paragraph.Tokenize().ToArray();
    }
}