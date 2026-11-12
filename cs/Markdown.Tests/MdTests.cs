namespace Markdown.Tests;
using FluentAssertions;
using Markdown;

// решил начать с написания тестов, показалось, что в данной задаче будет удобно придерживаться TDD
// т.к. перед написанием самой логики нужно полностью разобраться с принципом работы данного процессора md разметки

[TestFixture]
public class MdTests
{
    private static void MakeRenderTest(string markdown, string expectedHtml)
    {
        var actualHtml = Md.Render(markdown);
        actualHtml.Should().Be(expectedHtml);
    }
    
    [Test]
    public void Render_ReplacesUnderscoreWithEmTag()
    {
        MakeRenderTest("a_bc_d", "a<em>bc</em>d");
    }

    [Test]
    public void Render_ReplacesDoubleUnderscoreWithStrongTag()
    {
        MakeRenderTest("a__bc__d", "a<strong>bc</strong>d");
    }

    [Test]
    public void Render_DoesNotReplaceEscapedUnderscores()
    {
        MakeRenderTest(@"a\_b\_c", "a_b_c");
    }
    
    [Test]
    public void Render_ReplacesUnderscoreWithEmTag_IfBackSlashEscaped()
    {
        MakeRenderTest(@"\\_ab_", @"\<em>ab</em>");
    }

    [Test]
    public void Render_ProcessItalicInsideBold()
    {
        // Внутри __двойного выделения _одинарное_ тоже__ работает.
        MakeRenderTest(@"__ab_cd_ef__", "<strong>ab<em>cd</em>ef</strong>");
    }

    [Test]
    public void Render_DoesNotProcessBoldInsideItalic()
    {
        // внутри _одинарного __двойное__ не_ работает
        MakeRenderTest(@"_ab__cd__ef_", "<em>ab__cd__ef</em>");
    }

    [Test]
    public void Render_DoesNotProcessUnderscoresInsideTextWithDigits()
    {
        // Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка
        MakeRenderTest("abc_1_2_3", "abc_1_2_3");
    }
    
    [Test]
    public void Render_ProcessesUnderscoreInsideSingleWordWithNoDigits()
    {
        MakeRenderTest("_нач_але", "<em>нач</em>але");
        MakeRenderTest("сер_еди_не", "сер<em>еди</em>не");
        MakeRenderTest("кон_це._", "кон<em>це.</em>");
    }

    [Test]
    public void Render_DoesNotProcessUnderscoreInIndividualWords()
    {
        MakeRenderTest("ра_зных сл_овах", "ра_зных сл_овах");
    }
    
    [Test]
    public void Render_DoesNotReplaceUnpairedUnderscores()
    {
        MakeRenderTest("__Непарные_ символы", "__Непарные_ символы");
    }
    
    [Test]
    public void Render_DoesNotProcessUnderscore_IfWhitespaceAfterUnderscore()
    {
        // За подчерками, начинающими выделение, должен следовать непробельный символ
        MakeRenderTest("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением");
    }
    
    [Test]
    public void Render_DoesNotCloseItalicWithUnderscoreAfterWhitespace()
    {
        // Подчерки, заканчивающие выделение, должны следовать за непробельным символом
        MakeRenderTest("_подчерки _не считаются_", "<em>подчерки _не считаются</em>");
    }

    [Test]
    public void Render_DoesNotProcessUnderscores_IfTagsIntersected()
    {
        // В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением
        MakeRenderTest("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_");
    }

    [Test]
    public void Render_DoesNotProcessUnderscore_IfEmptyStringInside()
    {
        MakeRenderTest("внутри подчерков пустая строка ____", "внутри подчерков пустая строка ____");
    }

    [Test]
    public void Render_ConvertsHeadingToH1()
    {
        MakeRenderTest("# Заголовок", "<h1>Заголовок</h1>");
    }

    [Test]
    public void Render_ProcessesHeadingWithOtherTags()
    {
        MakeRenderTest("# Заголовок __с _разными_ символами__", 
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }
    
    [Test]
    public void Render_DoesNotReuseClosingMarkerOfIntersectedItalic()
    {
        MakeRenderTest("__a _b__ c_ d_e", "__a _b__ c_ d_e");
    }
}