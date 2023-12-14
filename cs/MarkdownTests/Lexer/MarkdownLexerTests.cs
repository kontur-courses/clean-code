using System.Collections;
using FluentAssertions;
using Markdown.Filter;
using Markdown.Lexer;
using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace MarkdownTests.Lexer;

public class MarkdownLexerTests
{
    private MarkdownLexer emptyLexer = null!;
    private MarkdownLexer lexer = null!;
    private MarkdownFilter filter = null!;

    [SetUp]
    public void SetUp()
    {
        emptyLexer = new MarkdownLexer(filter, '\\');
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        filter = new MarkdownFilter();
        lexer = new MarkdownLexerBuilder(filter, '\\')
            .WithTokenType("_", new EmphasisToken())
            .WithTokenType("__", new StrongToken())
            .WithTokenType("# ", new HeaderToken())
            .Build();
    }

    [Test]
    [TestCaseSource(typeof(RegisterTokenTypeTestCases), nameof(RegisterTokenTypeTestCases.InvalidParametersTestCases))]
    public void RegisterTokenType_ThrowsArgumentException_OnInvalidParameters(RegisterTokenTypeTestData testData)
    {
        Assert.Throws<ArgumentException>(() => emptyLexer.RegisterTokenType(testData.TypeSymbol, testData.TokenType));
    }

    [Test]
    public void RegisterTokenType_CorrectlyRegistersType_OnCorrectInput()
    {
        emptyLexer.RegisterTokenType("_", RegisterTokenTypeTestData.ValidType);

        emptyLexer.RegisteredTokenTypes["_"]
            .Should()
            .Be(RegisterTokenTypeTestData.ValidType);
    }

    [Test]
    public void RegisterTokenType_ThrowsArgumentException_OnDuplicateRegistrations()
    {
        emptyLexer.RegisterTokenType("_", RegisterTokenTypeTestData.ValidType);

        Assert.Throws<ArgumentException>(() => emptyLexer.RegisterTokenType("_", RegisterTokenTypeTestData.ValidType));
    }

    [TestCase(null)]
    [TestCase("")]
    public void Tokenize_ThrowsArgumentException_OnInvalidInputString(string line)
    {
        Assert.Throws<ArgumentException>(() => emptyLexer.Tokenize(line));
    }

    [Test]
    public void Tokenize_ReturnsOnlyLineItself_OnZeroRegisteredTokenTypes()
    {
        var result = emptyLexer.Tokenize("line without tokens");

        EnsureExpectedTokenAt(result, 0, "line without tokens");
        EnsureExpectedCollectionSize(result, 1);
    }

    [Test]
    public void Tokenize_ReturnsOnlyLineItself_WhenNoTokensMatched()
    {
        var result = lexer.Tokenize("line without matching tokens");

        EnsureExpectedTokenAt(result, 0, "line without matching tokens");
        EnsureExpectedCollectionSize(result, 1);
    }

    [Test]
    public void Tokenize_ReturnsCorrectResult_WhenNoValidationRequired()
    {
        var result = lexer.Tokenize("# __text strong__ ordinary_italic_ sometext");

        var expected = new List<Token>
        {
            new(new HeaderToken(), false, 0, 2),
            new(new StrongToken(), false, 2, 2),
            new(new TextToken("text strong"), false, 4, 11),
            new(new StrongToken(), true, 15, 2),
            new(new TextToken(" ordinary"), false, 17, 9),
            new(new EmphasisToken(), false, 26, 1),
            new(new TextToken("italic"), false, 27, 6),
            new(new EmphasisToken(), true, 33, 1),
            new(new TextToken(" sometext"), false, 34, 9)
        };

        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void Tokenize_DoesNotRegisterHeaderTag_WhenHeaderTagIsNotInTheBeginning()
    {
        var result = lexer.Tokenize("asd# fgf");

        var expected = new List<Token>
        {
            new(new TextToken("asd# fgf"), false, 0, 8)
        };

        CollectionAssert.AreEqual(expected, result);
    }

    [Test]
    public void Tokenize_ReturnsCorrectResult_WithEscapeSymbols()
    {
        var result = lexer.Tokenize(@"\\\__sk \_asd_ \df");

        var expected = new List<Token>
        {
            new(new TextToken(@"\\\_"), false, 0, 4),
            new(new EmphasisToken(), false, 4, 1),
            new(new TextToken(@"sk \_asd"), false, 5, 8),
            new(new EmphasisToken(), true, 13, 1),
            new(new TextToken(@" \df"), false, 14, 4)
        };
        
        CollectionAssert.AreEqual(expected, result);
    }
    
    private static void EnsureExpectedTokenAt(IReadOnlyList<Token> tokens, int index, string value)
    {
        tokens[index].GetValue()
            .Should()
            .Be(value);
    }

    private static void EnsureExpectedCollectionSize(ICollection collection, int expectedSize)
    {
        collection.Count
            .Should()
            .Be(expectedSize);
    }
}