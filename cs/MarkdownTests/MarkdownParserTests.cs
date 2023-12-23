using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

public class MarkdownParserTests
{
    private static MarkdownParser sut = new();

    [TestCaseSource(typeof(MarkdownParserTestData), nameof(MarkdownParserTestData.TestData))]
    public void ParseText_WithDifferentCases_ReturnsCorrectTokens(string text, IEnumerable<Token> expected)
    {
        var result = sut.ParseText(text);
        result.Should().BeEquivalentTo(expected);
    }
}