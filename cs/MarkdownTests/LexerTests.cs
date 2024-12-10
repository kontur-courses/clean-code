using FluentAssertions;
using Markdown;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

public class LexerTests
{
    private MarkdownLexer lexer;

    [SetUp]
    public void Setup() => lexer = new MarkdownLexer();

    [Test]
    public void Tokenize_WorksCorrect_WhenItalic()
    {
        const string text = "_italic_";
        var expected = new Token[] { new ItalicToken(0), new TextToken(1, "italic"), new ItalicToken(7) };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Test]
    public void Tokenize_WorksCorrect_WhenBold()
    {
        const string text = "__bold__";
        var expected = new Token[] { new BoldToken(0), new TextToken(2, "bold"), new BoldToken(6) };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Test]
    public void Tokenize_WorksCorrect_WhenHeadingWithoutCloseTag()
    {
        const string text = "# heading";
        var expected = new Token[] { new HeadingToken(0), new TextToken(2, "heading") };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Test]
    public void Tokenize_WorksCorrect_WhenHeadingWithCloseTag()
    {
        const string text = "# heading\ntext";
        var expected = new Token[]
        {
            new HeadingToken(0), new TextToken(2, "heading"), new NewLineToken(9), new TextToken(10, "text")
        };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }

    [Test]
    public void Tokenize_WorksCorrect_WithItalicInBold()
    {
        const string text = "__bold _italic___";
        var expected = new Token[]
        {
            new BoldToken(0), new TextToken(2, "bold"), new SpaceToken(6), new ItalicToken(7),
            new TextToken(8, "italic"), new ItalicToken(14), new BoldToken(15)
        };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }
    
    [Test]
    public void Tokenize_WorksCorrect_WithAllTags()
    {
        const string text = "# a b\\_c _d_ __e__\n___f___ 1_234";
        var expected = new Token[]
        {
            new HeadingToken(0),
            new TextToken(2, "a"),
            new SpaceToken(3),
            new TextToken(4, "b"),
            new TextToken(5, @"_"),
            new TextToken(7, @"c"),
            new SpaceToken(8),
            new ItalicToken(9),
            new TextToken(10, "d"),
            new ItalicToken(11),
            new SpaceToken(12),
            new BoldToken(13),
            new TextToken(15, "e"),
            new BoldToken(16),
            new NewLineToken(18),
            new BoldToken(19),
            new ItalicToken(21),
            new TextToken(22, "f"),
            new ItalicToken(23),
            new BoldToken(24),
            new SpaceToken(26),
            new NumberToken(27, "1_234")
        };
        var actual = lexer.Tokenize(text);
        actual.Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
    }
}