using FluentAssertions;
using NUnit.Framework;

namespace Markdown;

[TestFixture]
public class MarkdownTest
{
    private Md md;

    [SetUp]
    public void Setup()
    {
        md = new Md();
    }

    [TestCase("Просто текст", "Просто текст", TestName = "Plain text")]
    [TestCase("Текст _курсивом_ тут", "Текст <em>курсивом</em> тут", TestName = "Italic")]
    [TestCase("Текст __жирным__ тут", "Текст <strong>жирным</strong> тут", TestName = "Bold")]
    [TestCase("_одинарное __двойное__ не_", "<em>одинарное __двойное__ не</em>", TestName = "Bold inside Italic does not work")]
    [TestCase("__двойного _одинарное_ тоже__", "<strong>двойного <em>одинарное</em> тоже</strong>", TestName = "Italic inside Bold works")]
    public void Render_InlineMarkup_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("\\_Вот это\\_", "_Вот это_", TestName = "Escaped Italic")]
    [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\", "Здесь сим\\волы экранирования\\ \\должны остаться.\\", TestName = "Escaped non-special characters")]
    [TestCase("\\\\_вот это будет выделено тегом_", "\\<em>вот это будет выделено тегом</em>", TestName = "Escaped backslash followed by italic")]
    public void Render_Escaping_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("_ текст_", "_ текст_", TestName = "Space after opening marker")]
    [TestCase("_текст _", "_текст _", TestName = "Space before closing marker")]
    [TestCase("____", "____", TestName = "Empty markers")]
    [TestCase("в _нач_але", "в <em>нач</em>але", TestName = "Italic at start of word")]
    [TestCase("в сер_еди_не", "в сер<em>еди</em>не", TestName = "Italic in middle of word")]
    [TestCase("в кон_це._", "в кон<em>це.</em>", TestName = "Italic at end of word")]
    [TestCase("в ра_зных сл_овах", "в ра_зных сл_овах", TestName = "Italic across different words does not work")]
    public void Render_SpacesAndBoundaries_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("цифрами_12_3", "цифрами_12_3", TestName = "Underscores between digits")]
    [TestCase("__Непарные_ символы", "__Непарные_ символы", TestName = "Unpaired markers")]
    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_", TestName = "Intersecting markers")]
    public void Render_TrickyCases_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("# Заголовок", "<h1>Заголовок</h1>", TestName = "Header")]
    [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "Header with inline markup")]
    [TestCase("#Заголовок", "#Заголовок", TestName = "Hash without space is not a header")]
    public void Render_Headers_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [Test]
    public void Render_LargeInput_CompletesQuickly()
    {
        var input = string.Concat(Enumerable.Repeat("Текст _курсив_ и __жирный__ ", 30000));

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = md.Render(input);
        stopwatch.Stop();

        result.Should().NotBeNullOrEmpty();
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1500,
            "алгоритм должен быть линейным или почти линейным (O(N))");
    }
}