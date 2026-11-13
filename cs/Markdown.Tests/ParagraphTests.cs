using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class ParagraphTests : MdTestBase
{
    [Test]
    public void Render_PreservesParagraphSeparators()
        => Render("a _b_\n\n# h\r\n\r\nc\n")
            .Should().Be("a <em>b</em>\n\n<h1>h</h1>\r\n\r\nc\n");

    [Test]
    public void Render_EmphasisDoesNotCrossParagraphs()
        => Render("_a\n\nb_").Should().Be("_a\n\nb_");

    [Test]
    public void Render_EmphasisInEachParagraph_WorksIndependently()
        => Render("_a_\n\n__b__").Should().Be("<em>a</em>\n\n<strong>b</strong>");

    [Test]
    public void Render_NullText_Throws()
    {
        var act = () => md.Render(null!);

        act.Should().Throw<ArgumentNullException>();
    }
}
