using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MarkdownTest
{
    private Md md;

    [SetUp]
    public void Setup()
    {
        md = new Md();
    }

    [TestCase("Просто текст", "Просто текст")]
    [TestCase("Текст _курсивом_ тут", "Текст <em>курсивом</em> тут")]
    [TestCase("Текст __жирным__ тут", "Текст <strong>жирным</strong> тут")]
    [TestCase("_одинарное __двойное__ не_", "<em>одинарное __двойное__ не</em>")]
    [TestCase("__двойного _одинарное_ тоже__", "<strong>двойного <em>одинарное</em> тоже</strong>")]
    [TestCase("___текст___", "<strong><em>текст</em></strong>")]
    public void Render_InlineMarkup_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("\\_Вот это\\_", "_Вот это_")]
    [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\", "Здесь сим\\волы экранирования\\ \\должны остаться.\\")]
    [TestCase("\\\\_вот это будет выделено тегом_", "\\<em>вот это будет выделено тегом</em>")]
    public void Render_Escaping_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("_ текст_", "_ текст_")]
    [TestCase("_текст _", "_текст _")]
    [TestCase("____", "____")]
    [TestCase("в _нач_але", "в <em>нач</em>але")]
    [TestCase("в сер_еди_не", "в сер<em>еди</em>не")]
    [TestCase("в кон_це._", "в кон<em>це.</em>")]
    [TestCase("в ра_зных сл_овах", "в ра_зных сл_овах")]
    public void Render_SpacesAndBoundaries_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("цифрами_12_3", "цифрами_12_3")]
    [TestCase("цифрами_123_", "цифрами_123_")]
    [TestCase("__Непарные_ символы", "__Непарные_ символы")]
    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
    public void Render_TrickyCases_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
    [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    [TestCase("#Заголовок", "#Заголовок")]
    public void Render_Headers_ReturnsExpectedHtml(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }

    [TestCase("* Элемент 1\n* Элемент 2", "<ul><li>Элемент 1</li><li>Элемент 2</li></ul>")]
    [TestCase("* Элемент _с курсивом_", "<ul><li>Элемент <em>с курсивом</em></li></ul>")]
    [TestCase("* Элемент\nОбычный текст", "<ul><li>Элемент</li></ul>Обычный текст")]
    [TestCase("Обычный текст\n* Элемент", "Обычный текст<ul><li>Элемент</li></ul>")]
    public void Render_UnorderedLists_ReturnsExpectedHtml(string input, string expected)
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

    [Test]
    public void Render_AllTagsTogether_ReturnsExpectedHtml()
    {
        var input =
            "# Заголовок с __жирным _и курсивом___\n\n" +
            "Обычный абзац с \\_экранированием\\_ и __жирным__.\n\n" +
            "* Первый элемент _списка_\n" +
            "* Второй элемент с __жирным__";

        var expected =
            "<h1>Заголовок с <strong>жирным <em>и курсивом</em></strong></h1>" +
            "Обычный абзац с _экранированием_ и <strong>жирным</strong>." +
            "<ul><li>Первый элемент <em>списка</em></li><li>Второй элемент с <strong>жирным</strong></li></ul>";

        md.Render(input).Should().Be(expected);
    }
}