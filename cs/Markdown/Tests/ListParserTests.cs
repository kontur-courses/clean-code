using FluentAssertions;
using Markdown.Nodes;
using Markdown.Nodes.HtmlTags;
using Markdown.Parsing.Parsers;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class ListParserTests
{
    private ListParser parser;

    [SetUp]
    public void Setup() => parser = new ListParser();

    [Test]
    public void Parse_UnorderedList_ReturnsUlWithLi()
    {
        var text = new List<INode> { new TextNode("* Элемент 1\n* Элемент 2") };
        var nodes = parser.Parse(text);

        nodes.Should().HaveCount(1);
        var ul = nodes[0].Should().BeOfType<ContainerNode>().Which;
        ul.Tag.Should().Be(HtmlTag.Ul);
        ul.Children.Should().HaveCount(2);
        ul.Children[0].Should().BeOfType<ContainerNode>().Which.Tag.Should().Be(HtmlTag.Li);
    }

    [Test]
    public void Parse_ListThenParagraph_ReturnsUlAndText()
    {
        var text = new List<INode> { new TextNode("* Элемент\nОбычный текст") };
        var nodes = parser.Parse(text);

        nodes.Should().HaveCount(2);
        nodes[0].Should().BeOfType<ContainerNode>().Which.Tag.Should().Be(HtmlTag.Ul);
        nodes[1].Should().BeOfType<TextNode>().Which.Text.Should().Be("Обычный текст");
    }

    [Test]
    public void Parse_ParagraphThenList_ReturnsTextAndUl()
    {
        var text = new List<INode> { new TextNode("Обычный текст\n* Элемент") };
        var nodes = parser.Parse(text);

        nodes.Should().HaveCount(2);
        nodes[0].Should().BeOfType<TextNode>().Which.Text.Should().Be("Обычный текст");
        nodes[1].Should().BeOfType<ContainerNode>().Which.Tag.Should().Be(HtmlTag.Ul);
    }
}