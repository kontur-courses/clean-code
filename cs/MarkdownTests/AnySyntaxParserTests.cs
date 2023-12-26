using FluentAssertions;
using Markdown.Parser;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class AnySyntaxParserTests
{
    private AnySyntaxParser sut;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        sut = new AnySyntaxParser(new MarkdownToTokenSyntax());
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.ParseTokenTestCases))]
    public void ParseTokens_Should(string input, IEnumerable<IToken> expectedTokens)
    {
        var tokens = sut.ParseTokens(input);

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.FindAllTagsTestCases))]
    public void FindAllTags_Should(string input, IEnumerable<IToken> expectedTokens)
    {
        sut.ParseTokens(input);

        var tokens = sut.FindAllTags();

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.RemoveEscapedTagsTestCases))]
    public void RemoveEscapedTags_Should(string input, IList<IToken> tokensToParse, IEnumerable<IToken> expectedTokens)
    {
        sut.ParseTokens(input);

        var tokens = sut.RemoveEscapedTags(tokensToParse);

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.ValidateTagPositioningTestCases))]
    public void ValidateTagPositioning_Should(string input, IList<IToken> tokensToParse,
        IEnumerable<IToken> expectedTokens)
    {
        sut.ParseTokens(input);

        var tokens = sut.ValidateTagPositioning(tokensToParse);

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }
}