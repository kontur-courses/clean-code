using FluentAssertions;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers;
using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.ParserTests;

[TestFixture]
public class ParseHeaderTests
{
    [Test]
    public void ParseTokens_SimpleHeader_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocumentDocument = new MarkdownDocumentNode("");
        var headerNode = new HeaderNode("#");
        var textNode = new TextNode("текст");
        expectedDocumentDocument.AddChild(headerNode);
        headerNode.AddChild(textNode);


        var actualNode = parser.ParseTokens([]);

        actualNode.Should().BeEquivalentTo(expectedDocumentDocument);
    }

    [Test]
    public void ParseTokens_TwoHeadersInDifferentLines_ReturnsCorrectTree()
    {
        var tokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("Первый", TokenType.Text),
            new(" ", TokenType.Space),
            new("\n", TokenType.NewLine),
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("Второй", TokenType.Text),
            new("", TokenType.Eof)
        };

        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var headerNode1 = new HeaderNode("#");
        var headerNode2 = new HeaderNode("#");
        expectedDocument.AddChild(headerNode1);
        headerNode1.AddChild(new TextNode("Первый"));
        headerNode1.AddChild(new TextNode(" "));
        headerNode1.AddChild(new TextNode("\n"));
        expectedDocument.AddChild(headerNode2);
        headerNode2.AddChild(new TextNode("Второй"));

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_HeaderWithInlineTextBetween_ReturnsCorrectTree()
    {
        var tokens = new List<Token>
        {
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("Первый", TokenType.Text),
            new(" ", TokenType.Space),
            new("\n", TokenType.NewLine),
            new("просто", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("\n", TokenType.NewLine),
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("Второй", TokenType.Text),
            new("", TokenType.Eof)
        };

        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var h1 = new HeaderNode("#");
        h1.AddChild(new TextNode("Первый"));
        h1.AddChild(new TextNode(" "));
        h1.AddChild(new TextNode("\n"));
        expectedDocument.AddChild(h1);
        expectedDocument.AddChild(new TextNode("просто"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("текст"));
        expectedDocument.AddChild(new TextNode("\n"));
        var h2 = new HeaderNode("#");
        h2.AddChild(new TextNode("Второй"));
        expectedDocument.AddChild(h2);

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_HeaderWithoutSpace_TreatedAsPlainText()
    {
        var tokens = new List<Token>
        {
            new("#текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("#текст"));

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_HeaderWithLeadingSpace_NotAHeader()
    {
        var tokens = new List<Token>
        {
            new(" ", TokenType.Space),
            new("#", TokenType.Text),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("#"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("текст"));

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }
}