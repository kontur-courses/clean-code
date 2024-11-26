using Markdown.Scanners;
using Markdown.Tokens;
using FluentAssertions;
namespace MarkdownTests.Scanners;

[TestFixture]
[TestOf(typeof(TextScanner))]
public class TextScannerTest
{

    [TestCase("a", 0)]
    [TestCase("_a_", 1)]
    [TestCase("a\n", 0)]
    [TestCase(" a ", 1)]
    public void Scan_ShouldReturnTextToken_WhenBeginPointsOnText(string text, int begin)
    {
        var scanner = new TextScanner();
        
        var token = scanner.Scan(text, begin);
        
        token.Should().NotBeNull();
        token.Type.Should().Be(TokenType.Text);
    }

    [TestCase("_a", 0)]
    [TestCase("\na", 0)]
    [TestCase("#a", 0)]
    [TestCase(" a", 0)]
    [TestCase("a ", 1)]
    public void Scan_ShouldScanNull_WhenBeginPointsNotOnText(string text, int begin)
    {
        var scanner = new TextScanner();
        
        var token = scanner.Scan(text, begin);
        
        token.Should().BeNull();
    }

    [TestCase("abc", 0, 3)]
    [TestCase("abc ", 0, 3)]
    [TestCase(" abc", 1, 3)]
    [TestCase("_abc_", 1, 3)]
    public void Scan_ShouldReturnTextWithRightLength(string text, int begin, int expectedLength)
    {
        var scanner = new TextScanner();
        
        var token = scanner.Scan(text, begin);
        
        token!.GetValue().Length.Should().Be(expectedLength);
    }
    
}