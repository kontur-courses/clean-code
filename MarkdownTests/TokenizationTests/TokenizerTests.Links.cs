using FluentAssertions;
using Markdown.Tokenization.Tokens.Implementations;

namespace MarkdownTests.TokenizationTests;

public partial class TokenizationTests
{
    [Test]
    public void Tokenize_WithValidLink_ShouldCreateLinkToken()
    {
        var source = "См. [текст](url)!";
        var linkStart = source.IndexOf('[');
        var urlStart = source.IndexOf('(') + 1;
        var linkEnd = source.IndexOf(')') + 1;

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, linkStart),
            new LinkToken(source, linkStart, linkEnd - linkStart,
                urlStart, 3),
            new TextToken(source, linkEnd, source.Length - linkEnd));
    }

    [Test]
    public void Tokenize_WithEmptyLinkUrl_ShouldCreateLinkToken()
    {
        var source = "[x]()";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new LinkToken(source, 0, source.Length, 4, 0));
    }

    [Test]
    public void Tokenize_WithMarkdownInsideLink_ShouldTreatItAsSimpleText()
    {
        var source = @"[__текст_ \путь](url)";
        var urlStart = source.IndexOf('(') + 1;

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new LinkToken(source, 0, source.Length, urlStart, 3));
    }

    [Test]
    public void Tokenize_WithEscapedOpeningBracket_ShouldNotCreateLink()
    {
        var source = @"\[text](url)";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new EscapedToken(source, 0),
            new TextToken(source, 2, source.Length - 2));
    }

    [TestCase("[](url)")]
    [TestCase("[текст]")]
    [TestCase("[текст] (url)")]
    [TestCase("[текст](bad url)")]
    [TestCase("[текст](a(b)")]
    [TestCase("[текст](a\"b)")]
    [TestCase("[текст](url")]
    [TestCase("[текст\r\n](url)")]
    [TestCase("[текст](a\r\nb)")]
    [TestCase("[тек[ст]url)")]
    public void Tokenize_WithInvalidLink_ShouldNotCreateLinkToken(string source)
    {
        var tokens = Tokenize(source);

        tokens.Should().NotContain(t => t is LinkToken);
    }

    [Test]
    public void Tokenize_WithInvalidLink_ShouldRecognizePairTokens()
    {
        var source = "[_текст_](bad url)";

        var tokens = Tokenize(source);

        tokens.Should().Equal(
            new TextToken(source, 0, 1),
            new EmToken(source, 1),
            new TextToken(source, 2, 5),
            new EmToken(source, 7),
            new TextToken(source, 8, source.Length - 8));
    }
}