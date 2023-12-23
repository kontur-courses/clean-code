using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokens;

namespace MarkDownTests;

public class TokenizerTests
{
    private Tokenizer tokenizer;

    [SetUp]
    public void Setup()
    {
        tokenizer = new Tokenizer();
    }

    [TestCaseSource(nameof(TokenizerResultCountTestCaseData))]
    public void TokenizerResultTest(string input, IEnumerable<Token> expectedTokens)
    {
        var tokens = tokenizer.Tokenize(input);
        tokens.Should().BeEquivalentTo(expectedTokens);
    }

    private static IEnumerable<TestCaseData> TokenizerResultCountTestCaseData()
    {
        yield return new TestCaseData("привет мир", new List<Token>()
        { 
            new LiteralToken(0, 9, "привет мир")
        }).SetName("Tokenizer_Should_Return_Single_Token_When_Input_Without_Marks");

        yield return new TestCaseData("_привет мир_", new List<Token>()
        {
            new LiteralToken(1, 9, "привет мир"),
            new ItalicsToken(0, 10)
        }).SetName("Tokenizer_Should_Find_Italic_Token_In_Input");

        yield return new TestCaseData("__привет мир__", new List<Token>()
        {
            new LiteralToken(2, 10, "привет мир"),
            new BoldToken(0, 12)
        }).SetName("Tokenizer_Should_Find_Bold_Token_In_Input");

        yield return new TestCaseData("#привет мир", new List<Token>()
        {
            new LiteralToken(1, 10, "привет мир"),
            new ParagraphToken(0, 10)
        }).SetName("Tokenizer_Should_Find_Paragraph_Token_In_Input");

        yield return new TestCaseData("\\#привет мир", new List<Token>()
        {
            new LiteralToken(1, 11, "#привет мир")
        }).SetName("Tokenizer_Should_Ignore_Token_If_It_Screening");

        yield return new TestCaseData("\\\\#привет мир", new List<Token>()
        {
            new LiteralToken(1,1,"\\"),
            new ParagraphToken(3,12),
            new LiteralToken(2,12,"привет мир")
        }).SetName("Tokenizer_Should_Ignore_Screening_When_It_Screen_Yourself");

        yield return new TestCaseData("\\привет мир", new List<Token>()
        {
            new LiteralToken(1, 10, "привет мир"),
            new LiteralToken(0, 0, "\\")
        }).SetName("Tokenizer_Should_Display_Inactive_Screening_Symbol");

        yield return new TestCaseData("_привет__мир", new List<Token>()
        {
            new LiteralToken(1,6,"привет"),
            new LiteralToken(0,0,"_"),
            new LiteralToken(9,11,"мир"),
            new LiteralToken(7,8,"__")
            
        }).SetName("Tokenizer_Should_Display_Unclosing_Tokens");
    }
}