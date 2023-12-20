using FluentAssertions;
using Markdown.Converter;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class MarkupConverter_Should
{
    private HtmlConverter sut;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        sut = new HtmlConverter(new MarkdownSyntax());
    }

    [TestCaseSource(typeof(MarkupConverterTestCases), nameof(MarkupConverterTestCases.RenderTestCases))]
    public void ParseTokenTest(IList<IToken> tokens, string text, string expectedString)
    {
        var renderedString = sut.ConvertTags(tokens, text);

        renderedString.Should().Be(expectedString);
    }
}