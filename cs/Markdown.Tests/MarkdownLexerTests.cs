namespace Markdown.Tests;

public class MarkdownLexerTests
{
    private readonly MarkdownLexer sut = new MarkdownLexer();

    private static IEnumerable<object> ShouldResolveCases()
    {
        yield return new object[] {"_123 2_", new List<Token>
        {
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "2"),
            new Token(TokenType.ItalicSymbol)
        }};
        
        yield return new object[] {"__123 33__", new List<Token>
        {
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "33"),
            new Token(TokenType.BoldSymbol)
        }};
        
        yield return new object[] {"# 123 123", new List<Token>
        {
            new Token(TokenType.Header),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123")
        }};
        
        yield return new object[] {"qwe______123", new List<Token>
        {
            new Token(TokenType.Word, "qwe"),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.BoldSymbol),
            new Token(TokenType.Word, "123"),
        }};
        
        yield return new object[] {"# 123 123\r\n123", new List<Token>
        {
            new Token(TokenType.Header),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.Space),
            new Token(TokenType.Word, "123"),
            new Token(TokenType.NewLine),
            new Token(TokenType.Word, "123")
        }};
        
        yield return new object[] {@"\__123", new List<Token>
        {
            new Token(TokenType.Word, "_"),
            new Token(TokenType.ItalicSymbol),
            new Token(TokenType.Word, "123"),
        }};
    }
    
    [TestCaseSource(nameof(ShouldResolveCases))]
    public void GetTokens_ShouldResolveTextStyleSymbols(string text, List<Token> expected)
    {
        Assert.That(sut.GetTokens(text), Is.EqualTo(expected));
    }
    
}