using FluentAssertions;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tests;

[TestFixture]
public class MarkdownTokenizerTests
{
    private MarkdownTokenizer _tokenizer;

    [SetUp]
    public void SetUp()
    {
        _tokenizer = new MarkdownTokenizer();
    }

    [Test]
    public void Tokenize_ShouldReturnEmpty_WhenInputIsEmpty()
    {
        _tokenizer.Tokenize(string.Empty).Should().BeEmpty();
    }

    [Test]
    public void Tokenize_ShouldReturnSingleTextToken_WhenPlainText()
    {
        var tokens = _tokenizer.Tokenize("Привет мир");

        tokens.Should().HaveCount(3);
        tokens[0].Should().Match<Token>(t => t.Type == TokenType.Text && t.Value == "Привет");
        tokens[1].Should().Match<Token>(t => t.Type == TokenType.Whitespace && t.Value == " ");
        tokens[2].Should().Match<Token>(t => t.Type == TokenType.Text && t.Value == "мир");
    }

    [Test]
    public void Tokenize_ShouldIdentifySingleUnderscoreToken()
    {
        var tokens = _tokenizer.Tokenize("_слово_");

        tokens.Should().HaveCount(3);
        tokens[0].Type.Should().Be(TokenType.SingleUnderscore);
        tokens[0].CanOpen.Should().BeTrue();
        tokens[0].CanClose.Should().BeFalse();

        tokens[1].Type.Should().Be(TokenType.Text);
        tokens[1].Value.Should().Be("слово");

        tokens[2].Type.Should().Be(TokenType.SingleUnderscore);
        tokens[2].CanOpen.Should().BeFalse();
        tokens[2].CanClose.Should().BeTrue();
    }

    [Test]
    public void Tokenize_ShouldIdentifyDoubleUnderscoreToken()
    {
        var tokens = _tokenizer.Tokenize("__жирный__");

        tokens.Should().HaveCount(3);
        tokens[0].Type.Should().Be(TokenType.DoubleUnderscore);
        tokens[0].CanOpen.Should().BeTrue();

        tokens[2].Type.Should().Be(TokenType.DoubleUnderscore);
        tokens[2].CanClose.Should().BeTrue();
    }

    [Test]
    public void Tokenize_ShouldMarkTokenAsInsideWord_WhenFlankedByLetters()
    {
        var tokens = _tokenizer.Tokenize("сер_еди_не");

        var underscores = tokens.Where(t => t.Type == TokenType.SingleUnderscore).ToList();
        underscores.Should().HaveCount(2);
        underscores[0].IsInsideWord.Should().BeTrue();
        underscores[1].IsInsideWord.Should().BeTrue();
    }

    [Test]
    public void Tokenize_ShouldEscapeUnderscore_WhenPrecededByBackslash()
    {
        var tokens = _tokenizer.Tokenize(@"\_текст\_");

        tokens.Should().ContainSingle();
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be("_текст_");
    }

    [Test]
    public void Tokenize_ShouldKeepBackslash_WhenFollowedByNonEscapableChar()
    {
        var tokens = _tokenizer.Tokenize(@"сло\во");

        tokens.Should().ContainSingle();
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be(@"сло\во");
    }

    [Test]
    public void Tokenize_ShouldEscapeBackslashItself()
    {
        var tokens = _tokenizer.Tokenize(@"\\_текст_");

        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be(@"\");

        tokens[1].Type.Should().Be(TokenType.SingleUnderscore);
        tokens[1].CanOpen.Should().BeTrue();
    }

    [Test]
    public void Tokenize_ShouldTreatMoreThanTwoConsecutiveUnderscores_AsText()
    {
        var tokens = _tokenizer.Tokenize("____");

        tokens.Should().ContainSingle();
        tokens[0].Type.Should().Be(TokenType.Text);
        tokens[0].Value.Should().Be("____");
    }

    [Test]
    public void Tokenize_ShouldRecognizeBracketsAndParentheses()
    {
        var tokens = _tokenizer.Tokenize("[text](url)");

        tokens.Select(t => t.Type).Should().Equal(
            TokenType.OpenBracket,
            TokenType.Text,
            TokenType.CloseBracket,
            TokenType.OpenParen,
            TokenType.Text,
            TokenType.CloseParen);
    }
}
