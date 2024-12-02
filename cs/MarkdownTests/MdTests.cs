using FluentAssertions;
using Markdown;
using Markdown.Renderers;
using Markdown.Tokenizers;

namespace MarkdownTests;

public class MdTests
{
    [TestCaseSource(typeof(MdTestsData), nameof(MdTestsData.TestData))]
    public void ShouldConvertMdToHtmlCorrectly(string input, string expected)
    {
        var mdRenderer = Md.Create(new Tokenizer(), new HtmlTokenConverter());
        var result = mdRenderer.Render(input);
        result.Should().BeEquivalentTo(expected);
    }
}