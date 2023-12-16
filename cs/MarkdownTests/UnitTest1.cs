using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class Tests
{
    [TestCase("_aa_aa", "<em>aa</em>aa")]
    public void Render(string input, string expectedOutput)
    {
        Md.Render(input).Should().Be(expectedOutput);
    }
}