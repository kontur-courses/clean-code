using System.Collections;
using FluentAssertions;
using Markdown.Lexer;
using Markdown.Tokens;
using Markdown.Tokens.Types;
using Markdown.Validator;

namespace MarkdownTests.Lexer;

public class MarkdownLexerTests
{
    private MarkdownLexer emptyLexer = null!;
    private MarkdownLexer lexer = null!;
    private MarkdownValidator validator = null!;
    
    [SetUp]
    public void SetUp()
    {
        emptyLexer = new MarkdownLexer(validator);
    }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        validator = new MarkdownValidator();
        lexer = new MarkdownLexerBuilder(validator)
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
    public void Tokenize_ReturnsEmphasizedResult_OnCorrectEmTag()
    {
        var result = lexer.Tokenize("text _em text_ text");

        var expected = new List<Token>
        {
            new(new TextToken("text "), false, 0, 5),
            new(new EmphasisToken(), false, 5, 1),
            new(new TextToken("em text"), false,6, 7),
            new(new EmphasisToken(), true, 13, 1),
            new(new TextToken(" text"), false, 14, 5)
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