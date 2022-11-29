using System;
using FluentAssertions;
using Markdown.Extensions;
using Markdown.Primitives;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class HtmlRendererTests
{
    private HtmlRenderer sut;

    [SetUp]
    public void SetUp()
    {
        sut = new HtmlRenderer();
    }

    [Test]
    public void Render_ShouldThrowException_WhenTokensIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => sut.Render(null));
    }

    [Test]
    public void Render_ShouldReturnEmptyString_WhenTokensIsEmpty()
    {
        var html = sut.Render(Array.Empty<TagNode>());

        html.Should().Be(string.Empty);
    }

    [Test]
    public void Render_ShouldReturnText_OnTextTagNode()
    {
        var node = Tags.Text("Text").ToTagNode();

        var html = sut.Render(new[] { node });

        html.Should().Be("Text");
    }

    [Test]
    public void Render_ShouldReturnItalicText_OnItalicTagNode()
    {
        var node = new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("Text").ToTagNode());

        var html = sut.Render(new[] { node });

        html.Should().Be("<em>Text</em>");
    }

    [Test]
    public void Render_ShouldReturnBoldText_OnBoldTagNode()
    {
        var node = new TagNode(Tags.Bold(Tokens.Bold.Value), Tags.Text("Text").ToTagNode());

        var html = sut.Render(new[] { node });

        html.Should().Be("<strong>Text</strong>");
    }

    [Test]
    public void Render_ShouldReturnHeader1Text_OnHeaderTagNode()
    {
        var node = new TagNode(Tags.Header1(Tokens.Header1.Value), Tags.Text("Text").ToTagNode());

        var html = sut.Render(new[] { node });

        html.Should().Be("<h1>Text</h1>");
    }

    [Test]
    public void Render_ShouldReturnInnerItalicText_OnInnerItalicTagNode()
    {
        var node = new TagNode(Tags.Header1(Tokens.Header1.Value), new[]
        {
            Tags.Text("A").ToTagNode(),
            new TagNode(Tags.Italic(Tokens.Italic.Value), Tags.Text("Italic").ToTagNode()),
            Tags.Text("C").ToTagNode()
        });

        var html = sut.Render(new[] { node });

        html.Should().Be("<h1>A<em>Italic</em>C</h1>");
    }

    [Test]
    public void Render_ShouldReturnLink_OnLinkTagNode()
    {
        var node = new TagNode(Tags.Link("link"), Tags.Text("Text").ToTagNode());

        var html = sut.Render(new[] { node });

        html.Should().Be("<a href=\"link\">Text</a>");
    }
}