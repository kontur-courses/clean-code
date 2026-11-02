namespace Markdown.Tests;
using FluentAssertions;
using Markdown;

[TestFixture]
public class MdTests
{
    [Test]
    public void Render_ReplacesUnderscoreWithEmTag()
    {
        var testMd = "a_bc_d";
        var expectedHtml = @"a<em>bc</em>a";

        var actualHtml = Md.Render(testMd);
        actualHtml.Should().Be(expectedHtml);
    }
}