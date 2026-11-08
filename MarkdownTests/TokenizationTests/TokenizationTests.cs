using FluentAssertions;
using Markdown.Extensions;
using Markdown.Paragraphs;
using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Abstractions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.TokenizationTests;

public class TokenizationTests
{
    private static Token[] Tokenize(string source, bool isHeading = false)
    {
        var paragraph = new Paragraph(source, 0, source.Length, isHeading);
        return paragraph.Tokenize().ToArray();
    }
}