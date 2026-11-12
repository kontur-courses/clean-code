using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdRenderTests
{
    private readonly Md md = new();

    [TestCaseSource(nameof(EmphasisInTags))]
    public void Render_WrapsEmphasisInTags(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(Escaping))]
    public void Render_RemovesEscapingCharacters(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(EmphasisNesting))]
    public void Render_RespectsEmphasisNesting(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(EmphasisInsideWord))]
    public void Render_EmphasizesPartOfWord(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(NonMarkupUnderscores))]
    public void Render_KeepsUnderscoresThatAreNotMarkup(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(CrossingLinksAndUnderscores))]
    public void Render_KeepsOnlyPairThatOpenedFirstOnCrossing(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(Headers))]
    public void Render_RendersHeader(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(HeaderMarkerInsideParagraph))]
    public void Render_KeepsHeaderMarkerInsideParagraphAsText(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(Links))]
    public void Render_RendersLinks(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(EscapedLinkUrl))]
    public void Render_EscapesLinkUrlWithoutParsingMarkup(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(IncompleteLinks))]
    public void Render_KeepsIncompleteLinkSyntaxAsText(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(NestedLinks))]
    public void Render_ForbidsLinksInsideLinks(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(HtmlEscaping))]
    public void Render_EscapesHtmlInText(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(BlockSplitting))]
    public void Render_SplitsBlocksByBlankLines(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(LineEndings))]
    public void Render_NormalizesLineEndings(string markdown, string expected)
    {
        ShouldRenderTo(markdown, expected);
    }

    [TestCaseSource(nameof(BlankInput))]
    public void Render_ReturnsEmptyHtmlForBlankInput(string markdown, string expected)
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

    #region Данные кейсов: пары «разметка → ожидаемый HTML»

    public static IEnumerable<TestCaseData> EmphasisInTags => Cases(
        ("_текст_", "<p><em>текст</em></p>"),
        ("__текст__", "<p><strong>текст</strong></p>"),
        ("_a_ _b_", "<p><em>a</em> <em>b</em></p>"),
        ("_a\nb_", "<p><em>a\nb</em></p>"));

    public static IEnumerable<TestCaseData> Escaping => Cases(
        ("\\_Вот это\\_", "<p>_Вот это_</p>"),
        ("a\\_b\\_c", "<p>a_b_c</p>"),
        ("\\\\ _вот это_", "<p>\\ <em>вот это</em></p>"),
        ("Сим\\волы экранирования\\ \\должны остаться.\\",
            "<p>Сим\\волы экранирования\\ \\должны остаться.\\</p>"),
        ("\\# текст", "<p># текст</p>"),
        ("\\\\# текст", "<p>\\# текст</p>"),
        ("\\\\\\# текст", "<p>\\\\# текст</p>"),
        ("номер \\#1", "<p>номер \\#1</p>"),
        ("\\[a](u)", "<p>[a](u)</p>"),
        ("[a\\](u)", "<p>[a](u)</p>"),
        ("[a\\\\](u)", "<p><a href=\"u\">a\\</a></p>"),
        ("\\a", "<p>\\a</p>"),
        ("\\\\\\", "<p>\\\\</p>"),
        ("\\\\_текст_", "<p>\\<em>текст</em></p>"),
        ("\\__текст__", "<p>__текст__</p>"));

    public static IEnumerable<TestCaseData> EmphasisNesting => Cases(
        ("__двойное _одинарное_ тоже__", "<p><strong>двойное <em>одинарное</em> тоже</strong></p>"),
        ("_одинарного __двойное__ не_", "<p><em>одинарного __двойное__ не</em></p>"));

    public static IEnumerable<TestCaseData> EmphasisInsideWord => Cases(
        ("и в _нач_але", "<p>и в <em>нач</em>але</p>"),
        ("сер_еди_не", "<p>сер<em>еди</em>не</p>"),
        ("кон_це._", "<p>кон<em>це.</em></p>"));

    public static IEnumerable<TestCaseData> NonMarkupUnderscores => Cases(
        ("цифрами_12_3", "<p>цифрами_12_3</p>"),
        ("1___а___", "<p>1___а___</p>"),
        ("ра_зных сл_овах", "<p>ра_зных сл_овах</p>"),
        ("_это ра_ботает так_", "<p><em>это ра_ботает так</em></p>"),
        ("_начало ра_зных сл_овах конец_", "<p><em>начало ра_зных сл_овах конец</em></p>"),
        ("__Непарные_ символы", "<p>__Непарные_ символы</p>"),
        ("эти_ подчерки_", "<p>эти_ подчерки_</p>"),
        ("Иначе эти _подчерки _не считаются_ окончанием",
            "<p>Иначе эти _подчерки <em>не считаются</em> окончанием</p>"),
        ("____", "<p>____</p>"),
        ("а____б", "<p>а____б</p>"),
        ("__а ____ б__", "<p><strong>а ____ б</strong></p>"),
        ("_а ___ б_", "<p><em>а ___ б</em></p>"),
        ("__пересечения _двойных__ и одинарных_", "<p>__пересечения _двойных__ и одинарных_</p>"));

    public static IEnumerable<TestCaseData> CrossingLinksAndUnderscores => Cases(
        ("_a [b_ c](u)", "<p><em>a [b</em> c](u)</p>"),
        ("[a _b](u) c_", "<p><a href=\"u\">a _b</a> c_</p>"),
        ("_x [a _b](u) c_", "<p><em>x <a href=\"u\">a _b</a> c</em></p>"),
        ("[a _b](u)сер_еди_не", "<p><a href=\"u\">a _b</a>сер<em>еди</em>не</p>"),
        ("[a _b [c_ d](u) e](v)", "<p><a href=\"v\">a <em>b [c</em> d](u) e</a></p>"),
        ("_a __b [c_ d__ e](u)", "<p>_a __b <a href=\"u\">c_ d__ e</a></p>"),
        ("[x _y](u) a [b_ c](v)", "<p><a href=\"u\">x _y</a> a <a href=\"v\">b_ c</a></p>"),
        ("_a __b [c__ d](u) e_", "<p><em>a __b <a href=\"u\">c__ d</a> e</em></p>"));

    public static IEnumerable<TestCaseData> Headers => Cases(
        ("# Заголовок", "<h1>Заголовок</h1>"),
        ("# ", "<h1></h1>"),
        ("# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"));

    public static IEnumerable<TestCaseData> HeaderMarkerInsideParagraph => Cases(
        ("a\n# b", "<p>a\n# b</p>"));

    public static IEnumerable<TestCaseData> Links => Cases(
        ("[текст](url)", "<p><a href=\"url\">текст</a></p>"),
        ("[a _b_ c](u)", "<p><a href=\"u\">a <em>b</em> c</a></p>"),
        ("[a](u) [b](v)", "<p><a href=\"u\">a</a> <a href=\"v\">b</a></p>"));

    public static IEnumerable<TestCaseData> EscapedLinkUrl => Cases(
        ("[a](b&c)", "<p><a href=\"b&amp;c\">a</a></p>"),
        ("[a](b<c)", "<p><a href=\"b&lt;c\">a</a></p>"),
        ("[a](b>c)", "<p><a href=\"b&gt;c\">a</a></p>"),
        ("[a](b\"c)", "<p><a href=\"b&quot;c\">a</a></p>"));

    public static IEnumerable<TestCaseData> IncompleteLinks => Cases(
        ("[a](b)c)", "<p><a href=\"b\">a</a>c)</p>"),
        ("[a](u\nv)", "<p>[a](u\nv)</p>"),
        ("[a] (u)", "<p>[a] (u)</p>"),
        ("[текст без url]", "<p>[текст без url]</p>"),
        ("текст](url)", "<p>текст](url)</p>"),
        ("текст](_курсив_)", "<p>текст](_курсив_)</p>"),
        ("[](_курсив_)", "<p>[](_курсив_)</p>"),
        ("[](u)", "<p>[](u)</p>"));

    public static IEnumerable<TestCaseData> NestedLinks => Cases(
        ("[a [b](u) c](v)", "<p>[a <a href=\"u\">b</a> c](v)</p>"),
        ("[a [b [c](u) d](v) e](w)", "<p>[a [b <a href=\"u\">c</a> d](v) e](w)</p>"),
        ("[a [b] c](v)", "<p><a href=\"v\">a [b] c</a></p>"));

    public static IEnumerable<TestCaseData> HtmlEscaping => Cases(
        ("a < b", "<p>a &lt; b</p>"),
        ("a & b > c", "<p>a &amp; b &gt; c</p>"));

    public static IEnumerable<TestCaseData> BlockSplitting => Cases(
        ("a\n\nb", "<p>a</p>\n<p>b</p>"),
        ("# h\n\nтекст", "<h1>h</h1>\n<p>текст</p>"));

    public static IEnumerable<TestCaseData> LineEndings => Cases(
        ("a\r\nb", "<p>a\nb</p>"));

    public static IEnumerable<TestCaseData> BlankInput => Cases(
        ("", ""),
        ("   ", ""));

    private static IEnumerable<TestCaseData> Cases(params (string Markdown, string Html)[] cases)
    {
        foreach (var (markdown, html) in cases)
            yield return new TestCaseData(markdown, html).SetName(Describe(markdown));
    }

    private static string Describe(string markdown)
    {
        return $"\"{markdown.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "'")}\"";
    }

    #endregion
}
