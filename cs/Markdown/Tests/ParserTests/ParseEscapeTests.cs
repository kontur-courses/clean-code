using FluentAssertions;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers;
using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.ParserTests;

[TestFixture]
public class ParseEscapeTests
{
    [Test]
    public void ParseTokens_EscapeHeader_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("\\", TokenType.Escape),
            new("#", TokenType.Hash),
            new(" ", TokenType.Space),
            new("текст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("#"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("текст"));

        var actualNode = parser.ParseTokens([]);

        actualNode.Should().BeEquivalentTo(expectedDocument);
    }
    
    [Test]
    public void ParseTokens_EscapeUnderscores_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {   
            new ("\\", TokenType.Escape),
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode("текст"));
        expectedDocument.AddChild(new TextNode("__"));

        var actualNode = parser.ParseTokens([]);

        actualNode.Should().BeEquivalentTo(expectedDocument);
    }
    
    [Test]
    public void ParseTokens_EscapeWordUnderscores_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("те", TokenType.Text),
            new("\\", TokenType.Escape),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("те"));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("кст"));

        var actualNode = parser.ParseTokens([]);

        actualNode.Should().BeEquivalentTo(expectedDocument);
    }
    
    [Test]
    public void ParseTokens_EscapeImage_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {   
            new ("\\", TokenType.Escape),
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode("текст"));
        expectedDocument.AddChild(new TextNode("__"));

        var actualNode = parser.ParseTokens([]);

        actualNode.Should().BeEquivalentTo(expectedDocument);
    }
}