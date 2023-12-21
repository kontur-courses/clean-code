using FluentAssertions;
using Markdown.TagConverter;
using Markdown.Syntax;
using Markdown.Token;

namespace MarkdownTests;

public class MarkupConverterTests
{
    private MarkupConverter sut;

    [SetUp]
    public void SetUp()
    {
        sut = new MarkupConverter(new MarkdownToHtnlSyntax());
    }

    [TestCaseSource(typeof(MarkupConverterTestCases), nameof(MarkupConverterTestCases.RenderTestCases))]
    public void MarkupConverter_Should(IList<IToken> tokens, string text, string expectedString)
    {
        var renderedString = sut.ConvertTags(tokens, text);

        renderedString.Should().Be(expectedString);
    }
}