using System.Collections;
using FluentAssertions;
using Markdown.Filter;
using Markdown.Lexer;
using Markdown.Tokens;
using Markdown.Tokens.Types;
using MarkdownTests.Lexer.TestCases;

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
    
    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.InvalidParametersTests))]
    public void RegisterTokenType_ThrowsArgumentException_OnInvalidParameters(LexerRegisterTokenTestData registerTokenTestData)
    {
        Assert.Throws<ArgumentException>(() 
            => emptyLexer.RegisterTokenType(registerTokenTestData.TypeSymbol, registerTokenTestData.TokenType));
    }

    [Test]
    public void RegisterTokenType_CorrectlyRegistersType_OnCorrectInput()
    {
        emptyLexer.RegisterTokenType("_", LexerRegisterTokenTestData.ValidType);

        emptyLexer.RegisteredTokenTypes["_"]
            .Should()
            .Be(LexerRegisterTokenTestData.ValidType);
    }

    [Test]
    public void RegisterTokenType_ThrowsArgumentException_OnDuplicateRegistrations()
    {
        emptyLexer.RegisterTokenType("_", LexerRegisterTokenTestData.ValidType);

        Assert.Throws<ArgumentException>(() => emptyLexer.RegisterTokenType("_", LexerRegisterTokenTestData.ValidType));
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

        EnsureExpectedTokenAt(result.Tokens, 0, "line without tokens");
        EnsureExpectedCollectionSize(result.Tokens, 1);
    }

    [Test]
    public void Tokenize_ReturnsOnlyLineItself_WhenNoTokensMatched()
    {
        var result = lexer.Tokenize("line without matching tokens");

        EnsureExpectedTokenAt(result.Tokens, 0, "line without matching tokens");
        EnsureExpectedCollectionSize(result.Tokens, 1);
    }
    
    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.NoValidationTests))]
    public void Tokenize_ReturnsCorrectResult_WhenNoValidationRequired(LexerLogicTestData testData)
    {
        AssertTokenizeReturnsCorrectResult(testData);
    }

    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.EscapeSymbolsTests))]
    public void Tokenize_ReturnsCorrectResult_WithEscapeSymbols(LexerLogicTestData testData)
    {
        AssertTokenizeReturnsCorrectResult(testData);
    }

    [TestCaseSource(typeof(LexerTestCases), nameof(LexerTestCases.TextTokenTests))]
    public void Tokenize_ReturnsCorrectTextTokens_WhenValidationIsRequired(LexerLogicTestData testData)
    {
        AssertTokenizeReturnsCorrectResult(testData);
    }
    
    private static void EnsureExpectedTokenAt(IReadOnlyList<Token> tokens, int index, string value)
    {
        tokens[index].GetRepresentation()
            .Should()
            .Be(value);
    }

    private static void EnsureExpectedCollectionSize(ICollection collection, int expectedSize)
    {
        collection.Count
            .Should()
            .Be(expectedSize);
    }
    
    private void AssertTokenizeReturnsCorrectResult(LexerLogicTestData testData)
    {
        var result = lexer.Tokenize(testData.Line);
        CollectionAssert.AreEqual(testData.Expected, result.Tokens);
    }
}