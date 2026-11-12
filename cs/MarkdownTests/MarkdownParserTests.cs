using FluentAssertions;
using Markdown.Parsers;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkdownTests;

[TestFixture]
public class MarkdownParserTests
{
    private readonly MarkdownParser parser = new([
        new HeadingTag(),
        new UnorderedListTag(),
        new ParagraphTag()
    ]);

    [Test]
    public void Parse_BlankLine_SplitsTextIntoBlocks()
    {
        var blocks = parser.Parse("# Заголовок\r\n\r\n- один\r\n- два\r\n\r\nабзац").ToList();

        blocks.Should().HaveCount(3);
        blocks[0].Tag.Should().BeOfType<HeadingTag>();
        blocks[0].Content.Should().Be("# Заголовок");
        blocks[1].Tag.Should().BeOfType<UnorderedListTag>();
        blocks[1].Content.Should().Be("- один\n- два");
        blocks[2].Tag.Should().BeOfType<ParagraphTag>();
        blocks[2].Content.Should().Be("абзац");
    }

    [Test]
    public void Parse_SingleLineBreak_KeepsOneParagraph()
    {
        var blocks = parser.Parse("первая\nвторая").ToList();

        blocks.Should().ContainSingle();
        blocks[0].Tag.Should().BeOfType<ParagraphTag>();
        blocks[0].Content.Should().Be("первая\nвторая");
    }

    [Test]
    public void Parse_EmptyText_ReturnsNoBlocks()
    {
        parser.Parse(string.Empty).Should().BeEmpty();
    }
}