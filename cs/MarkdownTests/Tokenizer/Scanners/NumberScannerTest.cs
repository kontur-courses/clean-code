using FluentAssertions;
using Markdown.Tokenizer.Scanners;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests.Tokenizer.Scanners;

[TestFixture]
public class NumberScannerTest
{
    [TestCase("1", 0)]
    [TestCase("12345", 0)]
    public void NumberScanner_Scan_TokenShouldHaveNumberType(string text, int begin)
    {
        var scanner = new NumberScanner();
        var memoryText = new Memory<char>(text.ToCharArray());
        
        var token = scanner.Scan(memoryText[begin..]);
        
        token.Should().NotBeNull();
        token.TokenType.Should().Be(TokenType.NUMBER);
    }
    
    [TestCase(" 123", 0)]
    [TestCase("_\n ", 0)]
    [TestCase("abcdifgh", 0)]
    public void NumberScanner_Scan_ShouldScanNullFromText(string text, int begin)
    {
        var scanner = new NumberScanner();
        var memoryText = new Memory<char>(text.ToCharArray());
        
        var token = scanner.Scan(memoryText[begin..]);
        
        token.Should().BeNull();
    }
}