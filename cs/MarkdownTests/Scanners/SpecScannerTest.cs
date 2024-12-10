using FluentAssertions;
using Markdown.Tokenizer.Scanners;
using Markdown.Tokens;

namespace MarkdownTests.Scanners;

[TestFixture]
[TestOf(typeof(SpecScanner))]
public class SpecScannerTest
{

    [TestCase(" ", 0, TokenType.Space)]
    [TestCase("*", 0, TokenType.Asterisk)]
    [TestCase("\n", 0, TokenType.Newline)]
    [TestCase("#", 0, TokenType.Octothorpe)]
    [TestCase(@"\", 0, TokenType.Backslash)]
    [TestCase("_", 0, TokenType.Underscore)]
    [TestCase("_abc_", 0, TokenType.Underscore)]
    [TestCase("1234ab_", 6, TokenType.Underscore)]
    public void Scan_ShouldScanValidTokenType_WhenBeginPointsOnSpec
        (string text, int begin, TokenType expectedType)
    {
        var scanner = new SpecScanner();
        
        var token = scanner.Scan(text, begin);
        
        token?.TokenType.Should().Be(expectedType);
    }

    [TestCase("abc", 0)]
    [TestCase(" abc", 1)]
    [TestCase("abc\n", 0)]
    [TestCase("_abc_", 1)]
    public void Scan_ShouldScanNull_WhenBeginPointsNotOnSpec
        (string text, int begin)
    {
        var scanner = new SpecScanner();
        
        var token = scanner.Scan(text, begin);
        
        token.Should().BeNull();
    }
}