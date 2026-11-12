using FluentAssertions;
using Markdown.Parsing.Parsers.InlineParser;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class PairFinderTests
{
    private Lexer lexer;
    private PairFinder pairFinder;

    [SetUp]
    public void Setup()
    {
        lexer = new Lexer();
        pairFinder = new PairFinder();
    }

    [Test]
    public void FindValidPairs_SimpleItalic_ReturnsPair()
    {
        var text = "_текст_";
        var tokens = lexer.Tokenize(text);
        var pairs = pairFinder.FindValidPairs(tokens, text);

        pairs.Should().Contain((0, true)).And.Contain((2, false));
    }

    [Test]
    public void FindValidPairs_BoldInsideItalic_ReturnsOnlyItalicPair()
    {
        var text = "_a __b__ c_";
        var tokens = lexer.Tokenize(text);
        var pairs = pairFinder.FindValidPairs(tokens, text);

        pairs.Should().HaveCount(2);
        pairs.Should().Contain((0, true)).And.Contain((6, false)); // 6, а не 4!
    }

    [Test]
    public void FindValidPairs_UnpairedItalic_ReturnsEmpty()
    {
        var text = "_незакрытый";
        var tokens = lexer.Tokenize(text);
        var pairs = pairFinder.FindValidPairs(tokens, text);

        pairs.Should().BeEmpty();
    }

    [Test]
    public void FindValidPairs_IntersectingMarkers_ReturnsEmpty()
    {
        var text = "__пересечения _двойных__ и одинарных_";
        var tokens = lexer.Tokenize(text);
        var pairs = pairFinder.FindValidPairs(tokens, text);

        pairs.Should().BeEmpty();
    }

    [Test]
    public void FindValidPairs_ComplexIntersectionWithOuterBold_ReturnsEmpty()
    {
        var text = "__a _b__ c_ d__";
        var tokens = lexer.Tokenize(text);
        var pairs = pairFinder.FindValidPairs(tokens, text);

        pairs.Should().BeEmpty();
    }
}