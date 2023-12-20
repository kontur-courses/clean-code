using FluentAssertions;
using Markdown.Processor;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class Processor_Should
{
    private ISyntax syntax;

    [OneTimeSetUp]
    public void Setup()
    {
        syntax = new MarkdownSyntax();
    }

    [TestCaseSource(typeof(AnySyntaxParserTestCases), nameof(AnySyntaxParserTestCases.ParseTokenTestCases))]
    public void ParseTokenTest(string input, IEnumerable<IToken> expectedTokens)
    {
        var sut = new AnySyntaxParser(input, syntax);
        var tokens = sut.ParseTokens();

        tokens.Should().BeEquivalentTo(expectedTokens, options => options.Including(token => token.Position));
    }
}