using FluentAssertions;
using Markdown.Tokenization.Tokens;
using Markdown.Tokenization.Tokens.Implementations;


namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    [Test]
    public void Tokenize_WithEmAndStrong_ShouldCreateUnresolvedPairTokens()
    {
        var source = "_a_ __b__";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new EmToken(source, 0),
            new TextToken(source, 1, 1),
            new EmToken(source, 2),
            new TextToken(source, 3, 1),
            new StrongToken(source, 4),
            new TextToken(source, 6, 1),
            new StrongToken(source, 7));

        tokens.OfType<PairToken>()
            .Should().OnlyContain(t => t.Role == PairRole.Unresolved);
    }

    [Test]
    public void Tokenize_WithUnpairedUnderscore_ShouldCreateUnresolvedToken()
    {
        var source = "a_b";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, 1),
            new EmToken(source, 1),
            new TextToken(source, 2, 1));
    }
}
