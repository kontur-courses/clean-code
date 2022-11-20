using FluentAssertions;

namespace Markdown.Tests;

public class MdRenderer_Should
{
    private MarkdownRenderer markdownRenderer;
    [SetUp]
    public void Setup()
    {
        markdownRenderer = new MarkdownRenderer();
    }

    [Test]
    public void WorkCorrect_WithDifferentTags()
    {
       var htmlText =  markdownRenderer.Render("# Заголовок __с _разными_ символами__");
       htmlText.Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }
}