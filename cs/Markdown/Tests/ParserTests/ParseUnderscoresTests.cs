using FluentAssertions;
using Markdown.Nodes.Internal;
using Markdown.Nodes.Leaf;
using Markdown.Parsers;
using Markdown.Tokenizer;
using NUnit.Framework;

namespace Markdown.Tests.ParserTests;

[TestFixture]
public class ParseUnderscoresTests
{
    [Test]
    public void ParseTokens_SimpleBold_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var boldNode = new BoldNode("__");
        expectedDocument.AddChild(boldNode);
        boldNode.AddChild(new TextNode("текст"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_SimpleItalic_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var italicNode = new ItalicNode("_");
        expectedDocument.AddChild(italicNode);
        italicNode.AddChild(new TextNode("текст"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_SingleInsideDouble_ReturnsMarkdownDocument()
    {
        var tokens = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("двойное", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("одинарное", TokenType.Text),
            new("_", TokenType.Underscore),
            new(" ", TokenType.Space),
            new("тоже", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var bold = new BoldNode("__");
        expectedDocument.AddChild(bold);
        bold.AddChild(new TextNode("двойное"));
        bold.AddChild(new TextNode(" "));
        var italic = new ItalicNode("_");
        italic.AddChild(new TextNode("одинарное"));
        bold.AddChild(italic);
        bold.AddChild(new TextNode(" "));
        bold.AddChild(new TextNode("тоже"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_ItalicWithBoldInside_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("начало", TokenType.Text),
            new(" ", TokenType.Space),
            new("__", TokenType.DoubleUnderscore),
            new("внутри", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new(" ", TokenType.Space),
            new("конец", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };

        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var italic = new ItalicNode("_");
        expectedDocument.AddChild(italic);
        italic.AddChild(new TextNode("начало"));
        italic.AddChild(new TextNode(" "));
        italic.AddChild(new TextNode("__"));
        italic.AddChild(new TextNode("внутри"));
        italic.AddChild(new TextNode("__"));
        italic.AddChild(new TextNode(" "));
        italic.AddChild(new TextNode("конец"));

        var actual = parser.ParseTokens([]);

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_UnpairedUnderscores_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };

        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode("текст"));
        expectedDocument.AddChild(new TextNode("_"));

        var actual = parser.ParseTokens();

        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_ItalicAtStartWord_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var italicNode = new ItalicNode("_");
        expectedDocument.AddChild(italicNode);
        italicNode.AddChild(new TextNode("те"));
        expectedDocument.AddChild(new TextNode("кст"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_ItalicInMiddleWord_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("т", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("екс", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("т", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("т"));
        var italic = new ItalicNode("_");
        italic.AddChild(new TextNode("екс"));
        expectedDocument.AddChild(italic);
        expectedDocument.AddChild(new TextNode("т"));
        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_ItalicInEndWord_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("тек", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("ст.", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("тек"));
        var italic = new ItalicNode("_");
        italic.AddChild(new TextNode("ст."));
        expectedDocument.AddChild(italic);
        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_SeparateWordsUnderscores_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new(" ", TokenType.Space),
            new("те", TokenType.Text),
            new("_", TokenType.WordUnderscore),
            new("кст", TokenType.Text),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("те"));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("кст"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("те"));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("кст"));
        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_OnlyFourUnderscores_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("__", TokenType.DoubleUnderscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode("__"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_X_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("\n", TokenType.NewLine),
            new("_", TokenType.Underscore),
            new("текст", TokenType.Text),
            new("_", TokenType.Underscore),
            new("\n", TokenType.NewLine),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        var italic = new ItalicNode("_");
        expectedDocument.AddChild(new TextNode("\n"));
        italic.AddChild(new TextNode("текст"));
        expectedDocument.AddChild(italic);
        expectedDocument.AddChild(new TextNode("\n"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_X1_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("__", TokenType.DoubleUnderscore),
            new("пересечения", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("двойных", TokenType.Text),
            new("__", TokenType.DoubleUnderscore),
            new(" ", TokenType.Space),
            new("и", TokenType.Text),
            new(" ", TokenType.Space),
            new("одинарных", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode("пересечения"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("двойных"));
        expectedDocument.AddChild(new TextNode("__"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("и"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("одинарных"));
        expectedDocument.AddChild(new TextNode("_"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_X2_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("_", TokenType.Underscore),
            new("подчерки", TokenType.Text),
            new(" ", TokenType.Space),
            new("_", TokenType.Underscore),
            new("не", TokenType.Text),
            new(" ", TokenType.Space),
            new("считаются", TokenType.Text),
            new("_", TokenType.Underscore),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("подчерки"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("не"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("считаются"));
        expectedDocument.AddChild(new TextNode("_"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public void ParseTokens_X3_ReturnsDocument()
    {
        var tokens = new List<Token>
        {
            new("эти", TokenType.Text),
            new("_", TokenType.Underscore),
            new(" ", TokenType.Space),
            new("подчерки", TokenType.Text),
            new("_", TokenType.Underscore),
            new("\n", TokenType.NewLine),
            new("", TokenType.Eof)
        };
        var parser = new MarkdownParser(tokens);
        var expectedDocument = new MarkdownDocumentNode("");
        expectedDocument.AddChild(new TextNode("эти"));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode(" "));
        expectedDocument.AddChild(new TextNode("подчерки"));
        expectedDocument.AddChild(new TextNode("_"));
        expectedDocument.AddChild(new TextNode("\n"));

        var actual = parser.ParseTokens([]);
        
        actual.Should().BeEquivalentTo(expectedDocument);
    }
}