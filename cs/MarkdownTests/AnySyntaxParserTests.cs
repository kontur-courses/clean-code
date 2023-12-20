using FluentAssertions;
using Markdown.Processor;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class AnySyntaxParserTests
{
    private ISyntax syntax;
    private IParser sut;

    [OneTimeSetUp]
    public void Setup()
    {
        sut = new AnySyntaxParser(new MarkdownSyntax());
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.ParseTokenTestCases))]
    public void AnySyntaxParser_Should(string input, IEnumerable<IToken> expectedTokens)
    {
        var tokens = sut.ParseTokens(input);

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }
}