using FluentAssertions;
using Markdown.MarkdownParsers;
using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown.Tests;

[TestFixture]
public class LinkTagParserTest
{
    [Test]
    public void Parse_WithLinkTags()
    {
        var text = "hello [label](link) world";

        var expected = new List<Token>
        {
            new Token(Tag.Link, 6, TagType.Open, 13), new Token(Tag.Link, 18, TagType.Close, 0)
        };

        var result = new LinkTagParser().Parse(text).ToList();

        result[0].Should().BeEquivalentTo(expected[0]);
        result[1].Should().BeEquivalentTo(expected[1]);
    }

    [Test]
    public void Parse_WithUnCorrectLinkTags_ReturnsNoTokens()
    {
        var text = "hello [label] world";

        var result = new LinkTagParser().Parse(text).ToList();

        result.Should().BeEmpty();
    }

    [Test]
    public void Parse_WithoutLinkTags()
    {
        var text = "hello";

        var result = new LinkTagParser().Parse(text).ToList();

        result.Should().BeEmpty();
    }
}