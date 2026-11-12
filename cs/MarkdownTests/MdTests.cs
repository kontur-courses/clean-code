using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests;

[TestFixture]
public class MdTests
{
    private readonly Md md = new();

    [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка",
        "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка")]
    [TestCase("_нач_але", "<em>нач</em>але")]
    [TestCase("сер_еди_не", "сер<em>еди</em>не")]
    [TestCase("кон_це_.", "кон<em>це</em>.")]
    public void Render_Emphasis_WrapsSelectionInEm(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }

    [Test]
    public void Render_Strong_WrapsSelectionInStrong()
    {
        md.Render("__Выделенный двумя символами текст__")
            .Should().Be("<strong>Выделенный двумя символами текст</strong>");
    }

    [TestCase(@"\_Вот это\_", "_Вот это_")]
    [TestCase(@"сим\волы экранирования\ \должны остаться.\", @"сим\волы экранирования\ \должны остаться.\")]
    [TestCase(@"\\_вот это будет выделено тегом_", @"\<em>вот это будет выделено тегом</em>")]
    [TestCase(@"\# не заголовок", "# не заголовок")]
    public void Render_Escape_RemovesBackslashOnlyBeforeMarkup(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }

    [Test]
    public void Render_EmphasisInsideStrong_Works()
    {
        md.Render("__двойного выделения _одинарное_ тоже__")
            .Should().Be("<strong>двойного выделения <em>одинарное</em> тоже</strong>");
    }

    [Test]
    public void Render_StrongInsideEmphasis_StaysLiteral()
    {
        md.Render("_одинарного __двойное__ не_")
            .Should().Be("<em>одинарного __двойное__ не</em>");
    }

    [TestCase("цифрами_12_3", "цифрами_12_3")]
    [TestCase("ра_зных сл_овах", "ра_зных сл_овах")]
    [TestCase("ра_зных\nсл_овах", "ра_зных\nсл_овах")]
    [TestCase("_разных сл_овах", "_разных сл_овах")]
    [TestCase("сл_овах конец_", "сл_овах конец_")]
    [TestCase("__Непарные_", "__Непарные_")]
    [TestCase("эти_ подчерки_", "эти_ подчерки_")]
    [TestCase("эти _подчерки _не считаются_", "эти _подчерки <em>не считаются</em>")]
    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
    [TestCase("____", "____")]
    public void Render_InvalidUnderscores_StayAsText(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }

    [Test]
    public void Render_Heading_WrapsParagraphInH1()
    {
        md.Render("# Заголовок __с _разными_ символами__")
            .Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }

    [TestCase("#без пробела", "#без пробела")]
    [TestCase("## не первый уровень", "## не первый уровень")]
    public void Render_HashWithoutRequiredPrefix_IsNotHeading(string markdown, string html)
    {
        md.Render(markdown).Should().Be(html);
    }

    [Test]
    public void Render_ParagraphsSeparatedByBlankLine_DoNotShareInlineTags()
    {
        md.Render("_первый\n\nвторой_")
            .Should().Be("_первый\nвторой_");
    }

    [Test]
    public void Render_SingleLineBreak_StaysInsideParagraph()
    {
        md.Render("_первая\nстрока_")
            .Should().Be("<em>первая\nстрока</em>");
    }

    [Test]
    public void Render_UnorderedList_WrapsItemsAndInlineMarkup()
    {
        md.Render("- первый\n- второй _пункт_")
            .Should().Be("<ul><li>первый</li><li>второй <em>пункт</em></li></ul>");
    }

    [Test]
    public void Render_MixedLines_AreNotAList()
    {
        md.Render("- пункт\nпросто текст")
            .Should().Be("- пункт\nпросто текст");
    }

    [Test]
    public void Render_EscapedListMarker_IsParagraph()
    {
        md.Render(@"\- не пункт")
            .Should().Be("- не пункт");
    }

    [Test]
    public void Render_HeadingAndList_AreSeparateBlocks()
    {
        md.Render("# Заголовок\n\n- один\n- два")
            .Should().Be("<h1>Заголовок</h1>\n<ul><li>один</li><li>два</li></ul>");
    }

    [Test]
    public void Render_EmptyText_ReturnsEmptyString()
    {
        md.Render(string.Empty).Should().BeEmpty();
    }

    [Test]
    public void Render_LargeDocument_FinishesWithinASecond()
    {
        var text = string.Concat(Enumerable.Repeat("абзац с _курсивом_ и __жирным__ текстом. ", 10_000));
        md.Render("warmup");

        var stopwatch = Stopwatch.StartNew();
        var html = md.Render(text);
        stopwatch.Stop();

        html.Should().Contain("<em>курсивом</em>").And.Contain("<strong>жирным</strong>");
        stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(1));
    }
}