using FluentAssertions;
using Markdown;
using MarkdownTest.TestData;

namespace MarkdownTest;

public class MdTest
{
    [TestCaseSource(typeof(MdTestData), nameof(MdTestData.Examples))]
    public void Test(string md, string html)
    {
        new Md()
            .Render(md)
            .Should()
            .Be(html);
    }
}