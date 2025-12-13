using FluentAssertions;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers;
using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.ParserTests;

[TestFixture]
public class ParseImageTests
{
    [Test]
    public void ParseTokens_SimpleImage_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var imageNode = new ImageNode("!");
        var altNode = new AltNode("[");
        var altText = new TextNode("Cat");
        var urlNode = new UrlNode("https://example.com/image.jpg");
        expectedDocument.AddChild(imageNode);
        imageNode.AddChild(altNode);
        altNode.AddChild(altText);
        imageNode.AddChild(urlNode);


        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_NewlineAfterExclamation_ReturnsPlainText()
    {
        var tokens = new List<Token>
        {
            new("!", TokenType.Text),
            new("\n", TokenType.NewLine),
            new("[", TokenType.LBracket),
            new("Cat", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };

        var parser = new MarkdownParser(tokens);

        var expectedDocument = new MarkdownDocumentNode("");
        var exclamationText = new TextNode("!");
        var newLineText = new TextNode("\n");
        var lBracket = new TextNode("[");
        var text = new TextNode("Cat");
        var rBracket = new TextNode("]");
        var lParenthesis = new TextNode("(");
        var urlText = new TextNode("https://example.com/image.jpg");
        var rParenthesis = new TextNode(")");
        expectedDocument.AddChild(exclamationText);
        expectedDocument.AddChild(newLineText);
        expectedDocument.AddChild(lBracket);
        expectedDocument.AddChild(text);
        expectedDocument.AddChild(rBracket);
        expectedDocument.AddChild(lParenthesis);
        expectedDocument.AddChild(urlText);
        expectedDocument.AddChild(rParenthesis);

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_AltWithNewline_ReturnsPlainText()
    {
        var tokens = new List<Token>
        {
            new("!", TokenType.Exclamation),
            new("[", TokenType.LBracket),
            new("Ca", TokenType.Text),
            new("\n", TokenType.NewLine),
            new("t", TokenType.Text),
            new("]", TokenType.RBracket),
            new("(", TokenType.LParenthesis),
            new("https://example.com/image.jpg", TokenType.Text),
            new(")", TokenType.RParenthesis),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);

        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("!"));
        expectedDocument.AddChild(new TextNode("["));
        expectedDocument.AddChild(new TextNode("Ca"));
        expectedDocument.AddChild(new TextNode("\n"));
        expectedDocument.AddChild(new TextNode("t"));
        expectedDocument.AddChild(new TextNode("]"));
        expectedDocument.AddChild(new TextNode("("));
        expectedDocument.AddChild(new TextNode("https://example.com/image.jpg"));
        expectedDocument.AddChild(new TextNode(")"));

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }
}