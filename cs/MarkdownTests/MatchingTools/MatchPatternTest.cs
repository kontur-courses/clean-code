using FluentAssertions;
using Markdown.Parser.MatchingTools;
using Markdown.Tokens;

namespace MarkdownTests.MatchingTools;

[TestFixture]
[TestOf(typeof(MatchPattern))]
public class MatchPatternTest
{

    [Test]
    public void MatchPattern_BuildsCorrectPattern()
    {
        var pattern = MatchPattern
            .StartWith(TokenType.Text)
            .ContinueWith(TokenType.Space)
            .ContinueWithRepeat([TokenType.Underscore, TokenType.Text], 2)
            .EndWith(TokenType.Newline);
        List<TokenType> expectedPattern = [
            TokenType.Text, TokenType.Space, 
            TokenType.Underscore, TokenType.Text, 
            TokenType.Underscore, TokenType.Text, TokenType.Newline
        ];
        
        pattern.Should().BeEquivalentTo(expectedPattern);

    }
}