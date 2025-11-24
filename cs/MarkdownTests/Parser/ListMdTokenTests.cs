using FluentAssertions;
using Markdown.Domains;
using Markdown.Parser;

namespace MarkdownTest.Parser;

[TestFixture]
public class ListMdTokenTests
{
    [Test]
    public void IsUnderscoreInDifferentWord_InDifferentWords_ReturnsTrue()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Word, "раз"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Word, "ных"),
            new(TokenType.Space, " "),
            new(TokenType.Word, "словах"),
            new(TokenType.Underscore, "_")
        };

        tokens.IsUnderscoreInDifferentWord(1, 5, 1)
            .Should().BeTrue();
    }

    [Test]
    public void IsUnderscoreInDifferentWord_InSameWord_ReturnsFalse()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Word, "под"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Word, "черки"),
            new(TokenType.Underscore, "_")
        };

        tokens.IsUnderscoreInDifferentWord(1, 3, 1)
            .Should().BeFalse();
    }

    [Test]
    public void IsUnderscoreInDifferentWord_AcrossWords_ReturnsTrue()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Word, "раз"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Word, "ных"),
            new(TokenType.Space, " "),
            new(TokenType.Word, "словах"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Underscore, "_")
        };

        tokens.IsUnderscoreInDifferentWord(1, 5, 2)
            .Should().BeTrue();
    }

    [Test]
    public void IsUnderscoreInWordWithNumbers_WordWithNumbersInsideUnderscores_ReturnsTrue()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Word, "слов"),
            new(TokenType.Underscore, "_"),
            new(TokenType.Word, "о"),
            new(TokenType.Number, "123"),
            new(TokenType.Underscore, "_")
        };

        tokens.IsUnderscoreInWordWithNumbers(1, 3, 1)
            .Should().BeTrue();
    }

    [Test]
    public void IsUnderscoreInWordWithNumbers_OnlyNumbersInsideUnderscores_ReturnsFalse()
    {
        var tokens = new List<MdToken>
        {
            new(TokenType.Underscore, "_"),
            new(TokenType.Number, "123"),
            new(TokenType.Underscore, "_")
        };

        tokens.IsUnderscoreInWordWithNumbers(0, 2, 1)
            .Should().BeFalse();
    }
}