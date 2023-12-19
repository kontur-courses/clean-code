using FluentAssertions;
using Markdown.Builders;
using Markdown.Tags.TextTag;
using Markdown.TagsMappers;
using Markdown.Tokens;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class TagTokensBuilderTests
{
    [SetUp]
    public void Setup()
    {
        _tagsMapper = new MarkdownToHtmlTagsMapper();
        _tagsBuilder = new TagTokensBuilder(_tagsMapper);
    }

    private ITagsMapper _tagsMapper;
    private TagTokensBuilder _tagsBuilder;

    [Test]
    public void Build_ShouldReturnEmptyStringWhenPassedEmptyList()
    {
        var tokens = new List<IToken<Tag>>();
        var text = _tagsBuilder.Build(tokens);

        text.Should().Be(string.Empty);
    }

    [Test]
    public void Build_ShouldNotChangeState()
    {
        var firstTokens = new List<IToken<Tag>>
        {
            new TagToken(0, 5, new Tag("first ", TagType.Ignored)),
            new TagToken(6, 9, new Tag("text", TagType.Ignored))
        };
        var secondTokens = new List<IToken<Tag>>
        {
            new TagToken(0, 6, new Tag("second ", TagType.Ignored)),
            new TagToken(7, 10, new Tag("text", TagType.Ignored))
        };

        var firstTokensToText = _tagsBuilder.Build(firstTokens);
        _tagsBuilder.Build(secondTokens);
        var firstTokensToTextAfterAnotherBuild = _tagsBuilder.Build(firstTokens);

        firstTokensToTextAfterAnotherBuild.Should().Be(firstTokensToText);
    }

    [Test]
    public void Build_ShouldReturnStringOfTokensInThePassedOrder()
    {
        var tokens = new List<IToken<Tag>>
        {
            new TagToken(0, 5, new Tag("right ", TagType.Ignored)),
            new TagToken(6, 10, new Tag("order", TagType.Ignored))
        };
        var text = _tagsBuilder.Build(tokens);

        text.Should().Be("right order");
    }
}