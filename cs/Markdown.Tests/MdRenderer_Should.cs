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
    public void WorkCorrect_WithEscapeSymbols()
    {
        var htmlText = markdownRenderer.Render("\\\\spli\\ted \\_text");
        htmlText.Should().BeEquivalentTo("\\spli\\ted _text");
    }

    [TestCase("_or_", "<em>or</em>")]
    [TestCase("_or_  _not or_", "<em>or</em>  <em>not or</em>")]
    [TestCase("\\_or_", "_or_")]
    [TestCase("\\_or_not_", "_or<em>not</em>")]
    public void WorkCorrect_WithItalicTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("__bold__", "<strong>bold</strong>")]
    [TestCase("__bold__ and __bold__", "<strong>bold</strong> and <strong>bold</strong>")]
    public void WorkCorrect_WithBoldTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("__bold__", "<strong>bold</strong>")]
    [TestCase("__bold__ and _italic_", "<strong>bold</strong> and <em>italic</em>")]
    [TestCase("# Заголовок __с _разными_ символами__",
        "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    public void WorkCorrect_WithDifferentTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }
    
    [TestCase("\\\\__bold__", "\\<strong>bold</strong>")]
    [TestCase("\\__bold__", "__bold__")]
    [TestCase("\\_italic_", "_italic_")]
    [TestCase("_italic\\_", "_italic_")]
    public void WorkCorrect_WithEscapeTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("# simple header", "<h1> simple header</h1>")]
    [TestCase("__bold__ and _italic_", "<strong>bold</strong> and <em>italic</em>")]
    [TestCase("# Заголовок __с _разными_ символами__",
        "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    public void WorkCorrect_WithHeaders(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }
    
    [TestCase("# _sequence_ with __a __lot__ of t_a_gs __wo_r_d1 _word2_ _word3_ da__",
        "<h1> <em>sequence</em> with __a <strong>lot</strong> of t<em>a</em>gs __wo<em>r</em>d1 _word2_ _word3_ da__</h1>")]
    public void WorkCorrect_WithHardCases(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }
    
    [TestCase("tags",
        "tags")]
    [TestCase("long text without tags, <tag></tag>",
        "long text without tags, <tag></tag>")]
    public void WorkCorrect_WithTextWOTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void WorkCorrect_WithEmptyString()
    {
        var htmlText = markdownRenderer.Render("");
        htmlText.Should().BeEquivalentTo("");
    }

    [Test]
    public void WorkCorrect_WithNull()
    {
        var htmlText = markdownRenderer.Render(null);
        htmlText.Should().BeEquivalentTo(null);
    }
}