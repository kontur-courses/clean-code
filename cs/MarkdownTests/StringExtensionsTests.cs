using FluentAssertions;
using Markdown;
using Markdown.Tags;

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

    [TestCase("hello", 0, 5)]
    [TestCase("hello\nworld", 0, 5)]
    [TestCase("hello\nworld", 5, 11)]
    public void CloseIndexOfParagraph(string text, int idx, int expected)
    {
        text.CloseIndexOfParagraph(idx).Should().Be(expected);
    }

    [TestCase(@"\a", 1, true)]
    [TestCase(@"\\a", 2, false)]
    [TestCase(@"\\\a", 3, true)]
    public void IsShielded(string text, int idx, bool expected)
    {
        text.IsShielded(idx).Should().Be(expected);
    }

    [TestCase("some string", 0, true)]
    [TestCase("a\na", 2, true)]
    [TestCase("aaa", 2, false)]
    public void IsOpenOfParagraph(string text, int idx, bool expected)
    {
        text.IsOpenOfParagraph(idx).Should().Be(expected);
    }

    [TestCaseSource(typeof(StringExtensionsDataSet), nameof(StringExtensionsDataSet.IsTagCases))]
    public void ShouldReturn_(string text, ITag tag, int idx, bool isOpen, bool expected)
    {
        text.IsTag(tag, idx, isOpen).Should().Be(expected);
    }
}