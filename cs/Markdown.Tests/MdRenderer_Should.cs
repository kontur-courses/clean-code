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

    [TestCase("# _sequence_ with __a __lot__ of t_a_gs __wo_r_d_1_ _word2_ _word3_ da__",
        "<h1> <em>sequence</em> with __a <strong>lot</strong> of t<em>a</em>gs <strong>wo<em>r</em>d_1_ <em>word2</em> <em>word3</em> da</strong></h1>")]
    public void WorkCorrect_WithHardCases(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("_an_\n_lm_", "<em>an</em>\n<em>lm</em>")]
    [TestCase("_an_ \n _lm_", "<em>an</em> \n <em>lm</em>")]
    [TestCase("_an_b\nm_lm_", "<em>an</em>b\nm<em>lm</em>")]
    public void WorkCorrect_WithManyStrings(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("__123__", "<strong>123</strong>")]
    [TestCase("_12_ __34__", "<em>12</em> <strong>34</strong>")]
    public void WorkCorrect_WithLinesWithOnlyDigits(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("tags", "tags")]
    [TestCase("long text without tags, <tag></tag>", "long text without tags, <tag></tag>")]
    public void WorkCorrect_WithTextWOTags(string line, string expected)
    {
        var htmlText = markdownRenderer.Render(line);
        htmlText.Should().BeEquivalentTo(expected);
    }

    [TestCase("* tags", "<ul>\n<li> tags</li>\n</ul>")]
    [TestCase("* long text in marked list item ", "<ul>\n<li> long text in marked list item </li>\n</ul>")]
    [TestCase("* tags\n*", "<ul>\n<li> tags</li>\n</ul>\n*")]
    [TestCase("* tags \n* tags1 \n* tags2 ", "<ul>\n<li> tags </li>\n<li> tags1 </li>\n<li> tags2 </li>\n</ul>")]
    [TestCase("* item \n _not_ an item \n* __item__ ",
        "<ul>\n<li> item </li>\n</ul>\n <em>not</em> an item \n<ul>\n<li> <strong>item</strong> </li>\n</ul>")]
    public void WorkCorrect_WithMarkedLists(string line, string expected)
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