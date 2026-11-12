using FluentAssertions;
using Markdown.Nodes;
using Markdown.Nodes.HtmlTags;
using Markdown.Parsing.Parsers;

namespace Markdown.Tests;

[TestFixture]
public class BlockParserTests
{
    private BlockParser parser;

    [SetUp]
    public void Setup() => parser = new BlockParser();

    [Test]
    public void ParseText_Header_ReturnsH1()
    {
        var text = new List<INode> { new TextNode("# Заголовок") };
        var nodes = parser.Parse(text);
        nodes.Should().HaveCount(1);
        nodes[0].Should().BeOfType<ContainerNode>()
            .Which.Tag.Should().Be(HtmlTag.H1);
    }

    [Test]
    public void ParseText_TwoParagraphs_ReturnsTextNodes()
    {
        var text = new List<INode> { new TextNode("Первый\n\nВторой") };
        var nodes = parser.Parse(text);
        nodes.Should().HaveCount(2);
        nodes[0].Should().BeOfType<TextNode>().Which.Text.Should().Be("Первый");
        nodes[1].Should().BeOfType<TextNode>().Which.Text.Should().Be("Второй");
    }

    [Test]
    public void Parse_HeaderInSecondParagraph_ReturnsH1AfterText()
    {
        var text = new List<INode> { new TextNode("Текст\n\n# Заголовок") };
        var nodes = parser.Parse(text);

        nodes.Should().HaveCount(2);
        nodes[0].Should().BeOfType<TextNode>().Which.Text.Should().Be("Текст");
        nodes[1].Should().BeOfType<ContainerNode>()
            .Which.Tag.Should().Be(HtmlTag.H1);
    }
}