using System;
using FluentAssertions;
using Markdown.Primitives;
using Markdown.Tests.TestCaseSources;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
[Parallelizable(ParallelScope.All)]
public class HtmlRendererTests
{
    private HtmlRenderer renderer;

    [SetUp]
    public void SetUp()
    {
        renderer = new HtmlRenderer();
    }

    [Test]
    public void Render_ShouldThrowException_WhenTokensIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => renderer.Render(null));
    }
    
    [Test]
    public void Render_ShouldReturnEmptyString_WhenTokensIsEmpty()
    {
        var html = renderer.Render(Array.Empty<TagNode>());

        html.Should().Be(string.Empty);
    }

    [TestCaseSource(typeof(HtmlRendererSources),nameof(HtmlRendererSources.HtmlRendererSource))]
    public string Render_ShouldReturnCorrectHtml_On(TagNode node)
    {
        var html = renderer.Render(new [] {node});

        return html;
    }
}