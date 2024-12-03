using FluentAssertions;
using Markdown.HtmlTools;

namespace Markdown.Tests;

[TestFixture]
public class HtmlLinkTagBuilderTest
{
    [TestCase("[]()", "<a href=></a>")]
    [TestCase("[label](link)", "<a href=link>label</a>")]
    public void Build_ShouldReturnHTMLLink(string linkMarkdown, string expected)
    {
        var result = HtmlLinkTagBuilder.Build(linkMarkdown);
        result.Should().Be(expected);
    }

    [TestCase("[()", "")]
    public void Build_WithUnCorrectMdLink(string linkMarkdown, string expected)
    {
        var result = HtmlLinkTagBuilder.Build(linkMarkdown);
        result.Should().Be(expected);
    }
}