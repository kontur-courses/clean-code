using FluentAssertions;
using Markdown.Lexer;
using Markdown.TokenConverter;

namespace MarkdownTests.TokenConverter;

[TestFixture]
public class TokenConverterTests
{
    private readonly MarkdownTokenConverter converter = new();
    private readonly MarkdownLexer lexer = TestDataFactory.Lexer;

    [TestCase(@"\\\__sk \_asd_ \df", @"\__sk _asd_ \df")]
    [TestCase(@"\\_a_", @"\<em>a</em>")]
    [TestCase(@"\\\\_", @"\\_")]
    public void ConvertToString_ReturnsCorrectResult_WithEscapeSymbols(string initial, string expected)
    {
        AssertConvertToStringReturnsCorrectResult(initial, expected);
    }

    [TestCase("asd# fgf", "asd# fgf")]
    [TestCase("# a", "<h1>a</h1>")]
    public void ConvertToString_ReturnsCorrectResult_WithHeaderTag(string initial, string expected)
    {
        AssertConvertToStringReturnsCorrectResult(initial, expected);
    }

    private void AssertConvertToStringReturnsCorrectResult(string initial, string expected)
    {
        var result = converter.ConvertToString(lexer.Tokenize(initial));

        result
            .Should()
            .Be(expected);
    }
}