namespace MarkdownTests.TagTests;

using FluentAssertions;
using Markdown;
using Markdown.Tags;
using NUnit.Framework;
using System.Collections.Generic;

[TestFixture]
public class EscapeTests
{
    private Md mdRenderer;

    [OneTimeSetUp]
    public void Setup()
    {
        var supportedTags = new List<ITag>
            { new HeaderTag(), new ItalicTag(), new StrongTag() };
        mdRenderer = new Md(supportedTags);
    }

    [Test]
    public void WhenEscapedUnderscore_ItalicNotConverted()
    {
        var text = @"\_Вот это\_, не должно выделиться тегом";
        var expected = "_Вот это_, не должно выделиться тегом";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenEscapeCharactersWithoutNearTag_RemainInOutput()
    {
        var text = @"Здесь сим\волы экранирования\ \должны остаться.\";
        var expected = @"Здесь сим\волы экранирования\ \должны остаться.\";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }

    [Test]
    public void WhenEscapedEscapeCharacter_ConvertsCorrectly()
    {
        var text = @"\\_вот это будет выделено тегом_";
        var expected = @"\<em>вот это будет выделено тегом</em>";

        var result = mdRenderer.Render(text);

        result.Should().Be(expected);
    }
}