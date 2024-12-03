using FluentAssertions;
using Markdown.TagInfo;
using Markdown.MarkdownParsers;

namespace Markdown.Tests;

[TestFixture]
public class SingleTagParserTest
{
    [Test]
    public void Parse_WithEmptyString_ReturnsEmpty()
    {
        var text = string.Empty;

        var result = new SingleTagParser("# ", Tag.FirstLevelHeader).Parse(text);

        result.Should().BeEmpty();
    }

    [Test]
    public void Parse_WithStringWithoutTags_ReturnsEmpty()
    {
        var text = "without tags";

        var result = new SingleTagParser("# ", Tag.FirstLevelHeader).Parse(text);

        result.Should().BeEmpty();
    }

    [TestCase("# hello")]
    public void Parse_WithHeaderTags(string text)
    {
        var result = new SingleTagParser("# ", Tag.FirstLevelHeader).Parse(text);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(token => token.Tag == Tag.FirstLevelHeader);
    }

    [TestCase(@"\# b_c")]
    public void Parse_WithEscapedTags_ShouldNotParsed(string text)
    {
        var result = new SingleTagParser("# ", Tag.FirstLevelHeader).Parse(text);

        result.Should().BeEmpty();
    }
}