using FluentAssertions;
using Markdown.TagInfo;
using Markdown.MarkdownParsers;

namespace Markdown.Tests;

[TestFixture]
public class PairedTagsParserTest
{
    [Test]
    public void Parse_WithEmptyString_ReturnsEmpty()
    {
        var text = string.Empty;

        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().BeEmpty();
    }

    [Test]
    public void Parse_WithStringWithoutTags_ReturnsEmpty()
    {
        var text = "without tags";

        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().BeEmpty();
    }

    [TestCase("_hello_")]
    [TestCase("_hello_hello")]
    [TestCase("hello_hello_")]
    [TestCase("he_llohe_llo")]
    public void Parse_WithItalicTags(string text)
    {
        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(token => token.Tag == Tag.Italic);
    }

    [TestCase(@"a\_b_c")]
    [TestCase(@"a\_b\_c")]
    [TestCase(@"a_b\_c")]
    public void Parse_WithEscapedTags_ShouldNotParsed(string text)
    {
        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().BeEmpty();
    }

    [TestCase(@"\\_a_")]
    [TestCase(@"\\_a\\_")]
    [TestCase(@"_a\\_")]
    public void Parse_WithEscapedEscape(string text)
    {
        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(token => token.Tag == Tag.Italic);
    }

    [Test]
    public void Parse_WithMultipleItalicTags()
    {
        var text = "a_b_c_d_";

        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().HaveCount(4);
        result.Should().OnlyContain(token => token.Tag == Tag.Italic);
    }

    [TestCase("a_ b_")]
    [TestCase("_a _b")]
    public void Parse_TagsWithSpace_ReturnsEmpty(string text)
    {
        var result = new PairedTagsParser("_", Tag.Italic).Parse(text);

        result.Should().BeEmpty();
    }
}