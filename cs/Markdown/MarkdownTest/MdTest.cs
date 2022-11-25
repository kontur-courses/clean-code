using FluentAssertions;
using Markdown;

namespace MarkdownTest;

[TestFixture]
public class MdTest
{
    private Md md;

    [SetUp]
    public void SetUp()
    {
        md = new Md();
    }

    [TestCase("_a_", "<em>a</em>")]
    [TestCase("_a_ _b_", "<em>a</em> <em>b</em>")]
    public void EmTagTest(string mdstring, string result)
    {
        md.Render(mdstring)
            .Should()
            .Be(result);
    }
}