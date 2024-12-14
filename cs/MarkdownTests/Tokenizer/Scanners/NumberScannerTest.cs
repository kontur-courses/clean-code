using FluentAssertions;
using Markdown.Tokenizer.Scanners;
using Markdown.Tokens;

namespace MarkdownTests.Tokenizer.Scanners;

[TestFixture]
[TestOf(typeof(NumberScanner))]
public class NumberScannerTest
{

    [TestCase("1", 0)]
    [TestCase("42", 0)]
    [TestCase("12345", 2)]
    [TestCase("ab1234", 2)]
    [TestCase("123ab", 0)]
    public void Scan_ShouldReturnNumberToken_WhenBeginPointsOnNumber(string text, int begin)
    {
        var scanner = new NumberScanner();
        
        var token = scanner.Scan(GetMemorySlice(text, begin));

        token.Should().NotBeNull();
        token.TokenType.Should().Be(TokenType.Number);
    }

    [TestCase(" 123", 0)]
    [TestCase("_\n", 0)]
    [TestCase("abc", 0)]
    [TestCase("ab1234", 0)]
    [TestCase("123ab", 3)]
    public void Scan_ShouldScanNull_WhenBeginPointsNotOnNumber(string text, int begin)
    {
        var scanner = new NumberScanner();
        
        var token = scanner.Scan(GetMemorySlice(text, begin));
        
        token.Should().BeNull();
    }

    [TestCase("123", 0, 3)]
    [TestCase("a123", 1, 3)]
    [TestCase("123a", 0, 3)]
    public void Scan_ShouldReturnNumberWithRightLength(string text, int begin, int expectedLength)
    {
        var scanner = new NumberScanner();
        
        var token = scanner.Scan(GetMemorySlice(text, begin));
        
        token?.Value.Length.Should().Be(expectedLength);
    }

    private static Memory<char> GetMemorySlice(string text, int begin) => new(text.ToCharArray()[begin..]);
}