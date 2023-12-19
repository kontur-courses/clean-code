using FluentAssertions;
using Markdown;

namespace MarkdownTests;

public class StringExtensionsTests
{
    [TestCase(@"\", "")]
    [TestCase(@"\\", @"\")]
    [TestCase(@"\\\", @"\")]
    [TestCase(@"\a", @"a")]
    [TestCase(@"\\a", @"\a")]
    [TestCase(@"\\\a", @"\a")]
    public void ReplaceShieldingChars(string text, string expected)
    {
        text.ReplaceShieldSequences().Should().Be(expected);
    }
}