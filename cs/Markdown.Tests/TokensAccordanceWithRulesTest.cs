using FluentAssertions;
using Markdown.TagInfo;
using Markdown.TokenInfo;

namespace Markdown.Tests;

[TestFixture]
public class TokensAccordanceWithRulesTest
{
    [Test]
    public void Process_WithoutTokens_ReturnsEmpty()
    {
        var tokens = new List<Token>();

        var processedTokens = TokensAccordanceWithRules.Handle(tokens);

        processedTokens.Should().BeEmpty();
    }

    [Test]
    public void Process_UnpairedTokens_ReturnsEmpty()
    {
        var tokens = new List<Token>
        {
            new Token(Tag.Strong, 1, TagType.Open, 2), new Token(Tag.Italic, 3, TagType.Close, 1),
        };

        var processedTokens = TokensAccordanceWithRules.Handle(tokens);

        processedTokens.Should().BeEmpty();
    }

    [Test]
    public void Process_CorrectNesting()
    {
        var tokens = new List<Token>
        {
            new Token(Tag.Strong, 1, TagType.Open, 2),
            new Token(Tag.Italic, 3, TagType.Open, 1),
            new Token(Tag.Italic, 5, TagType.Close, 1),
            new Token(Tag.Strong, 7, TagType.Close, 2),
        };

        var processedTokens = TokensAccordanceWithRules.Handle(tokens);

        processedTokens.Should().HaveCount(4);
    }

    [Test]
    public void Process_InvalidNesting()
    {
        var tokens = new List<Token>
        {
            new Token(Tag.Italic, 1, TagType.Open, 1),
            new Token(Tag.Strong, 2, TagType.Open, 2),
            new Token(Tag.Strong, 5, TagType.Close, 2),
            new Token(Tag.Italic, 7, TagType.Close, 1),
        };

        var processedTokens = TokensAccordanceWithRules.Handle(tokens);

        processedTokens.Should().HaveCount(2);
    }

    [Test]
    public void Process_TagsIntersection_ReturnsEmpty()
    {
        var tokens = new List<Token>
        {
            new Token(Tag.Strong, 1, TagType.Open, 2),
            new Token(Tag.Italic, 3, TagType.Open, 1),
            new Token(Tag.Strong, 5, TagType.Close, 2),
            new Token(Tag.Italic, 7, TagType.Close, 1),
        };

        var processedTokens = TokensAccordanceWithRules.Handle(tokens);

        processedTokens.Should().BeEmpty();
    }
}