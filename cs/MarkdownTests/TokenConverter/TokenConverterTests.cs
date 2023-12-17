using FluentAssertions;
using Markdown.Filter;
using Markdown.Lexer;
using Markdown.TokenConverter;
using Markdown.Tokens.Types;

namespace MarkdownTests.TokenConverter;

[TestFixture]
public class TokenConverterTests
{
    private MarkdownTokenConverter converter = null!;
    private MarkdownLexer lexer = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        lexer = new MarkdownLexerBuilder(new MarkdownFilter(), '\\')
            .WithTokenType(new EmphasisToken())
            .WithTokenType(new StrongToken())
            .WithTokenType(new HeaderToken())
            .Build();

        converter = new MarkdownTokenConverter();
    }

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