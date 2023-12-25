using System.Text;

namespace MarkdownTests;

[TestFixture]
public class ComplexTests
{
    [Test]
    public void Render_ShouldNotCursiveAndBold_WhenIntersect()
    {
        Markdown.Md.Render("I _go __in_ home__")
            .Should().Be(@"I _go __in_ home__");

        Markdown.Md.Render("_N __i k_i t__ a_")
            .Should().Be(@"_N __i k_i t__ a_");
    }

    [Test]
    public void Render_ShouldNotBold_WhenInCursive()
    {
        Markdown.Md.Render("Me _eat __cool__ meat_")
            .Should().Be(@"Me <em>eat __cool__ meat</em>");
    }

    [Test]
    public void Render_ShouldCursive_WhenInBold()
    {
        Markdown.Md.Render("I __go _to_ home__")
            .Should().Be(@"I <strong>go <em>to</em> home</strong>");
    }
}