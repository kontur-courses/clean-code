using FluentAssertions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    [Test]
    public void Tokenize_WithEmptyText_ShouldBeEmpty()
    {
        var tokens = Tokenize(string.Empty);

        tokens.Should().BeEmpty();
    }

    [Test]
    public void Tokenize_WithSimpleText_ShouldReturnSingleTextToken()
    {
        var source = "Обычный текст";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, source.Length));
    }
}
