using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdRenderTests
{
    private readonly Md md = new();

    [TestCase("_текст_", "<p><em>текст</em></p>")]
    [TestCase("__текст__", "<p><strong>текст</strong></p>")]
    [TestCase("_a_ _b_", "<p><em>a</em> <em>b</em></p>")]
    [TestCase("_a\nb_", "<p><em>a\nb</em></p>")]
    public void Render_WrapsEmphasisInTags(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
    [TestCase("# ", "<h1></h1>")]
    [TestCase(
        "# Заголовок __с _разными_ символами__",
        "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    [TestCase("a\n# b", "<p>a\n# b</p>")]
    public void Render_RendersHeaderOnlyAtBlockStart(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("__двойное _одинарное_ тоже__", "<p><strong>двойное <em>одинарное</em> тоже</strong></p>")]
    [TestCase("_одинарного __двойное__ не_", "<p><em>одинарного __двойное__ не</em></p>")]
    public void Render_RespectsEmphasisNesting(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("цифрами_12_3", "<p>цифрами_12_3</p>")]
    [TestCase("и в _нач_але", "<p>и в <em>нач</em>але</p>")]
    [TestCase("сер_еди_не", "<p>сер<em>еди</em>не</p>")]
    [TestCase("кон_це._", "<p>кон<em>це.</em></p>")]
    [TestCase("ра_зных сл_овах", "<p>ра_зных сл_овах</p>")]
    [TestCase("__Непарные_ символы", "<p>__Непарные_ символы</p>")]
    [TestCase("эти_ подчерки_", "<p>эти_ подчерки_</p>")]
    [TestCase(
        "Иначе эти _подчерки _не считаются_ окончанием",
        "<p>Иначе эти _подчерки <em>не считаются</em> окончанием</p>")]
    [TestCase("____", "<p>____</p>")]
    [TestCase("__пересечения _двойных__ и одинарных_", "<p>__пересечения _двойных__ и одинарных_</p>")]
    public void Render_KeepsUnderscoresThatAreNotMarkup(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("\\_Вот это\\_", "<p>_Вот это_</p>")]
    [TestCase("\\\\ _вот это будет выделено_", "<p>\\ <em>вот это будет выделено</em></p>")]
    [TestCase(
        "Здесь сим\\волы экранирования\\ \\должны остаться.\\",
        "<p>Здесь символы экранирования должны остаться.\\</p>")]
    [TestCase("\\# текст", "<p># текст</p>")]
    [TestCase("\\[a](u)", "<p>[a](u)</p>")]
    public void Render_RemovesEscapingCharacters(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("[текст](url)", "<p><a href=\"url\">текст</a></p>")]
    [TestCase("[a _b_ c](u)", "<p><a href=\"u\">a <em>b</em> c</a></p>")]
    [TestCase("[a](u) [b](v)", "<p><a href=\"u\">a</a> <a href=\"v\">b</a></p>")]
    [TestCase("[a [b](u) c](v)", "<p><a href=\"v\">a <a href=\"u\">b</a> c</a></p>")]
    public void Render_RendersLinks(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("[a](b&c)", "<p><a href=\"b&amp;c\">a</a></p>")]
    [TestCase("[a](b<c)", "<p><a href=\"b&lt;c\">a</a></p>")]
    [TestCase("[a](b>c)", "<p><a href=\"b&gt;c\">a</a></p>")]
    [TestCase("[a](b\"c)", "<p><a href=\"b&quot;c\">a</a></p>")]
    public void Render_EscapesLinkUrlWithoutParsingMarkup(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("[a](b)c)", "<p><a href=\"b\">a</a>c)</p>")]
    [TestCase("[a](u\nv)", "<p>[a](u\nv)</p>")]
    [TestCase("[a] (u)", "<p>[a] (u)</p>")]
    [TestCase("[текст без url]", "<p>[текст без url]</p>")]
    [TestCase("текст](url)", "<p>текст](url)</p>")]
    [TestCase("[](u)", "<p>[](u)</p>")]
    public void Render_KeepsIncompleteLinkSyntaxAsText(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("a\n\nb", "<p>a</p>\n<p>b</p>")]
    [TestCase("a\r\n\r\nb", "<p>a</p>\n<p>b</p>")]
    [TestCase("# h\n\nтекст", "<h1>h</h1>\n<p>текст</p>")]
    public void Render_SplitsBlocksByBlankLines(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("a\r\nb", "<p>a\nb</p>")]
    public void Render_NormalizesLineEndings(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("", "")]
    [TestCase("   ", "")]
    public void Render_ReturnsEmptyHtmlForBlankInput(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCase("a < b", "<p>a &lt; b</p>")]
    [TestCase("a & b > c", "<p>a &amp; b &gt; c</p>")]
    public void Render_EscapesHtmlInText(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    private void ShouldRenderTo(string markdown, string expected)
    {
        md.Render(markdown).Should().Be(expected, $"разметка '{ShowNewlines(markdown)}' рендерится в этот HTML");
    }

    private static string ShowNewlines(string markdown)
    {
        return markdown.Replace("\n", "\\n");
    }
}