using FluentAssertions;
using Markdown;

namespace MarkDownTests;

public class MdRenderTests
{
    [TestCaseSource(typeof(MdRendererTestsData), nameof(MdRendererTestsData.TestData))]
    public void MdRendererTests(string input, string expected)
    {
        var mdRenderer = new Md();
        var res = mdRenderer.Render(input);

        res.Should().BeEquivalentTo(expected);
    }
}