using FluentAssertions;

namespace Markdown.Tests;

public class Tests
{
    private Md mdRenderer;
    [SetUp]
    public void Setup()
    {
        mdRenderer = new Md();
    }

    [Test]
    public void WorkCorrect_WithDifferentTags()
    {
       var htmlText =  mdRenderer.Render("# Заголовок __с _разными_ символами__");
       htmlText.Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }
}