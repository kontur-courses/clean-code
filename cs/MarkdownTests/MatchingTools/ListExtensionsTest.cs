using FluentAssertions;
using Markdown;
using Markdown.Parser.MatchingTools;
using Markdown.Tokens;

namespace MarkdownTests.MatchingTools;

[TestFixture]
[TestOf(typeof(ListExtensions))]
public class ListExtensionsTest
{
    private readonly MdTokenizer tokenizer = new();
    
    [Test]
    public void SingleMatch_ShouldReturnZeroMatch_WhenGotEmptyPattern()
    {
        var tokens = tokenizer.Tokenize("abc");
        var match = tokens.SingleMatch(new Pattern([]));

        match.Should().BeEquivalentTo(Match<Token>.ZeroMatch);
    }

    [Test]
    public void SingleMatch_ShouldReturnZeroMatch_WhenGotTooLongPattern()
    {
        var tokens = tokenizer.Tokenize("abc");
        var pattern = MatchPattern
            .StartWith(TokenType.Text)
            .ContinueWith(TokenType.Newline)
            .End();
        
        var match = tokens.SingleMatch(pattern);

        match.Should().BeEquivalentTo(Match<Token>.ZeroMatch);
    }

    [Test]
    public void SingleMatch_ShouldReturnZeroMatch_WhenThereIsNoMatch()
    {
        var tokens = tokenizer.Tokenize("abc");
        var pattern = MatchPattern
            .StartWith(TokenType.Newline)
            .End();

        var match = tokens.SingleMatch(pattern);
        
        match.Should().BeEquivalentTo(Match<Token>.ZeroMatch);
    }

    [Test]
    public void SingleMatch_ShouldReturnMatch_WhenThereIsMatch()
    {
        var tokens = tokenizer.Tokenize("abc");
        var pattern = MatchPattern
            .StartWith(TokenType.Text)
            .End();
        
        var match = tokens.SingleMatch(pattern);

        match.Should().NotBeEquivalentTo(Match<Token>.ZeroMatch);
        match.Start.Should().Be(0);
        match.Length.Should().Be(tokens.Count);
    }

    [Test]
    public void SingleMatch_ShouldReturnMatchFromGivenBeginning()
    {
        var tokens = tokenizer.Tokenize("abc def ghi");
        var pattern = MatchPattern
            .StartWith(TokenType.Text)
            .End();
        
        var match = tokens.SingleMatch(pattern, 2);

        match.Start.Should().Be(2);
    }

    [Test]
    public void KleeneStarMatch_ShouldReturnMultipleMatches()
    {
        var tokens = tokenizer.Tokenize("abc def ghi ");
        var pattern = MatchPattern
            .StartWith(TokenType.Text)
            .EndWith(TokenType.Space);
        
        var matches = tokens.KleeneStarMatch(pattern);
        
        matches.Should().HaveCount(3);
        matches.Should().OnlyContain(m => m.Length == 2);
    }

    [Test]
    public void FirstSingleMatch_ShouldReturnFirstMatch()
    {
        var tokens = tokenizer.Tokenize("abc def ghi jkl");
        var twoWordsPattern = MatchPattern
            .Start()
            .ContinueWith([TokenType.Text, TokenType.Space, TokenType.Text])
            .End();
        var wordPattern = MatchPattern
            .StartWith(TokenType.Text)
            .End();

        var match = tokens.FirstSingleMatch([wordPattern, twoWordsPattern]);
        
        match.Should().NotBeEquivalentTo(Match<Token>.ZeroMatch);
        match.Start.Should().Be(0);
    }
}