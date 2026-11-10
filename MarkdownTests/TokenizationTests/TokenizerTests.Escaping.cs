using FluentAssertions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    [Test]
    public void Tokenize_WithEscapedUnderscore_ShouldCreateEscapedToken()
    {
        var source = @"\_текст_";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new EscapedToken(source, 0),
            new TextToken(source, 2, 5),
            new EmToken(source, 7));
    }

    [Test]
    public void Tokenize_WithEscapedBackslash_ShouldNotEscapeNextUnderscore()
    {
        var source = @"\\_a_";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new EscapedToken(source, 0),
            new EmToken(source, 2),
            new TextToken(source, 3, 1),
            new EmToken(source, 4));
    }

    [Test]
    public void Tokenize_WithNonEscapableSymbol_ShouldKeepBackslashAsText()
    {
        var source = @"сим\вол";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, source.Length));
    }
}