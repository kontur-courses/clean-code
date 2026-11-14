using System.Diagnostics;
using FluentAssertions;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    private Md _md;

    [SetUp]
    public void SetUp()
    {
        _md = new Md();
    }

    [Test]
    public void Render_ShouldReturnEmptyString_WhenInputIsNullOrEmpty()
    {
        _md.Render(string.Empty).Should().BeEmpty();
        _md.Render(null!).Should().BeEmpty();
    }

    [Test]
    public void Render_ShouldReturnSameText_WhenNoMarkup()
    {
        var input = "Обычный текст без всякого выделения.";

        var result = _md.Render(input);

        result.Should().Be(input);
    }

    [Test]
    public void Render_ShouldWrapInEm_WhenTextSurroundedBySingleUnderscore()
    {
        var input = "_текст в курсиве_";

        var result = _md.Render(input);

        result.Should().Be("<em>текст в курсиве</em>");
    }

    [TestCase("в _нач_але слова", "в <em>нач</em>але слова")]
    [TestCase("в сер_еди_не слова", "в сер<em>еди</em>не слова")]
    [TestCase("в кон_це._", "в кон<em>це.</em>")]
    public void Render_ShouldSupportItalic_InsidePartOfWord(string input, string expected)
    {
        _md.Render(input).Should().Be(expected);
    }

    [Test]
    public void Render_ShouldNotHighlight_WhenUnderscoresInDifferentWords()
    {
        var input = "ра_зных сл_овах";

        var result = _md.Render(input);

        result.Should().Be("ра_зных сл_овах");
    }

    [Test]
    public void Render_ShouldWrapInStrong_WhenTextSurroundedByDoubleUnderscore()
    {
        var input = "__полужирный текст__";

        var result = _md.Render(input);

        result.Should().Be("<strong>полужирный текст</strong>");
    }

    [Test]
    public void Render_ShouldSupportMultipleBoldTags_InSingleParagraph()
    {
        var input = "__первый__ обычный __второй__";

        var result = _md.Render(input);

        result.Should().Be("<strong>первый</strong> обычный <strong>второй</strong>");
    }

    [Test]
    public void Render_ShouldSupportItalic_InsideBold()
    {
        var input = "__двойное выделение _одинарное_ тоже__";

        var result = _md.Render(input);

        result.Should().Be("<strong>двойное выделение <em>одинарное</em> тоже</strong>");
    }

    [Test]
    public void Render_ShouldNotSupportBold_InsideItalic()
    {
        var input = "_одинарное __двойное__ не_";

        var result = _md.Render(input);

        result.Should().Be("<em>одинарное __двойное__ не</em>");
    }

    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
    [TestCase("_пересечения __двойных_ и одинарных__", "_пересечения __двойных_ и одинарных__")]
    public void Render_ShouldNotHighlight_WhenTagsIntersect(string input, string expected)
    {
        _md.Render(input).Should().Be(expected);
    }

    [Test]
    public void Render_ShouldKeepFourUnderscores_WhenEmptyStringInside()
    {
        var input = "Текст ____ текст";

        var result = _md.Render(input);

        result.Should().Be("Текст ____ текст");
    }

    [Test]
    public void Render_ShouldNotHighlight_UnpairedUnderscores()
    {
        var input = "__Непарные_ символы в рамках одного абзаца";

        var result = _md.Render(input);

        result.Should().Be("__Непарные_ символы в рамках одного абзаца");
    }

    [TestCase("эти_ подчерки_ не выделение", "эти_ подчерки_ не выделение")]
    [TestCase("эти _подчерки _не считаются_ окончанием", "эти _подчерки <em>не считаются</em> окончанием")]
    public void Render_ShouldValidate_WhitespaceBoundariesForUnderscores(string input, string expected)
    {
        _md.Render(input).Should().Be(expected);
    }

    [Test]
    public void Render_ShouldNotHighlight_UnderscoresInsideTextWithDigits()
    {
        var input = "цифрами_12_3";

        var result = _md.Render(input);

        result.Should().Be("цифрами_12_3");
    }

    [TestCase("_100 рублей_", "<em>100 рублей</em>")]
    [TestCase("_глава 1_", "<em>глава 1</em>")]
    [TestCase("_123_", "<em>123</em>")]
    [TestCase("__2026 год__", "<strong>2026 год</strong>")]
    public void Render_ShouldHighlight_WhenNumberIsAtBoundaryOfSelection(string input, string expected)
    {
        _md.Render(input).Should().Be(expected);
    }

    [Test]
    public void Render_ShouldNotHighlight_WhenUnderscoresAreEscaped()
    {
        var input = @"\_Вот это\_, не должно выделиться тегом.";

        var result = _md.Render(input);

        result.Should().Be("_Вот это_, не должно выделиться тегом.");
    }

    [Test]
    public void Render_ShouldKeepBackslash_WhenItDoesNotEscapeMarkup()
    {
        var input = @"Здесь сим\волы экранирования\ \должны остаться.\";

        var result = _md.Render(input);

        result.Should().Be(@"Здесь сим\волы экранирования\ \должны остаться.\");
    }

    [Test]
    public void Render_ShouldSupportEscapedBackslash_BeforeMarkup()
    {
        var input = @"\\_вот это будет выделено тегом_";

        var result = _md.Render(input);

        result.Should().Be(@"\<em>вот это будет выделено тегом</em>");
    }

    [Test]
    public void Render_ShouldNotCreateHeader_WhenHashIsEscaped()
    {
        var input = @"\# Не заголовок";

        var result = _md.Render(input);

        result.Should().Be("# Не заголовок");
    }

    [Test]
    public void Render_ShouldWrapInH1_WhenLineStartsWithHashAndSpace()
    {
        var input = "# Простой заголовок";

        var result = _md.Render(input);

        result.Should().Be("<h1>Простой заголовок</h1>");
    }

    [Test]
    public void Render_ShouldSupportFormatting_InsideHeader()
    {
        var input = "# Заголовок __с _разными_ символами__";

        var result = _md.Render(input);

        result.Should().Be("<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>");
    }

    [Test]
    public void Render_ShouldNotCreateHeader_WhenNoSpaceAfterHash()
    {
        var input = "#БезПробела";

        var result = _md.Render(input);

        result.Should().Be("#БезПробела");
    }

    [Test]
    public void Render_ShouldHandleMultipleBlocks_WithNewlines()
    {
        var input = "# Заголовок\nПервый абзац с _курсивом_\nВторой абзац с __жирным__";
        var expected = "<h1>Заголовок</h1>\nПервый абзац с <em>курсивом</em>\nВторой абзац с <strong>жирным</strong>";

        var result = _md.Render(input);

        result.Should().Be(expected);
    }

    [Test]
    public void Render_ShouldRenderSimpleLink()
    {
        var input = "[Google](https://google.com)";

        var result = _md.Render(input);

        result.Should().Be("<a href=\"https://google.com\">Google</a>");
    }

    [Test]
    public void Render_ShouldSupportFormatting_InsideLinkText()
    {
        var input = "[текст _с курсивом_ и __жирным__](https://example.com)";

        var result = _md.Render(input);

        result.Should().Be("<a href=\"https://example.com\">текст <em>с курсивом</em> и <strong>жирным</strong></a>");
    }

    [Test]
    public void Render_ShouldSupportFormattedNumbers_InsideLinkText()
    {
        var input = "[_123_ и __456__](https://example.com)";

        var result = _md.Render(input);

        result.Should().Be("<a href=\"https://example.com\"><em>123</em> и <strong>456</strong></a>");
    }

    [Test]
    public void Render_ShouldSupportLink_InsideItalic()
    {
        var input = "_нажми [сюда](https://example.com) сейчас_";

        var result = _md.Render(input);

        result.Should().Be("<em>нажми <a href=\"https://example.com\">сюда</a> сейчас</em>");
    }

    [Test]
    public void Render_ShouldNotParseMarkup_InsideLinkUrl()
    {
        var input = "[ссылка](https://example.com/a_b_c__d)";

        var result = _md.Render(input);

        result.Should().Be("<a href=\"https://example.com/a_b_c__d\">ссылка</a>");
    }

    [Test]
    public void Render_ShouldNotCreateLink_WhenUrlContainsWhitespace()
    {
        var input = "[не ссылка](https://example.com with space)";

        var result = _md.Render(input);

        result.Should().Be("[не ссылка](https://example.com with space)");
    }

    [Test]
    public void Render_ShouldNotCreateLink_WhenBracketsAreEscaped()
    {
        var input = @"\[не ссылка\](https://example.com)";

        var result = _md.Render(input);

        result.Should().Be("[не ссылка](https://example.com)");
    }

    [Test]
    public void Render_ShouldLeaveUnclosedBrackets_AsText()
    {
        var input = "Это [просто скобки] без ссылки";

        var result = _md.Render(input);

        result.Should().Be("Это [просто скобки] без ссылки");
    }

    [Test]
    public void Render_ShouldHaveLinearExecutionTime_OnLargeInput()
    {
        var sampleParagraph = "# Заголовок __с _разными_ стилями__\n" +
                              "Текст с [ссылкой](https://site.com/foo_bar) и _курсивом_ и __полужирным__.\n" +
                              "Обычные слова, цифрами_12_3 и _100_ рублей.\n\n";

        _md.Render(sampleParagraph);

        const int smallCount = 300;
        const int largeCount = 3000;

        var smallText = string.Concat(Enumerable.Repeat(sampleParagraph, smallCount));
        var largeText = string.Concat(Enumerable.Repeat(sampleParagraph, largeCount));
        var sw = Stopwatch.StartNew();
        _md.Render(smallText);
        sw.Stop();
        var smallElapsed = sw.ElapsedMilliseconds;

        sw.Restart();
        _md.Render(largeText);
        sw.Stop();
        var largeElapsed = sw.ElapsedMilliseconds;

        if (smallElapsed > 5)
        {
            var ratio = (double)largeElapsed / smallElapsed;
            ratio.Should().BeLessThan(25, "время работы должно расти почти линейно от размера входа");
        }
    }

    [Test]
    public void Render_ShouldHaveLinearExecutionTime_OnNestedUnclosedLinkPatterns()
    {
        const string unclosedLinkPattern = "[x](";
        _md.Render(unclosedLinkPattern);

        const int smallCount = 1000;
        const int largeCount = 10000;

        var smallText = string.Concat(Enumerable.Repeat(unclosedLinkPattern, smallCount));
        var largeText = string.Concat(Enumerable.Repeat(unclosedLinkPattern, largeCount));

        for (int i = 0; i < 10; i++)
        {
            _md.Render(smallText);
            _md.Render(largeText);
        }

        double smallMedianMs = MeasureMedianTimeMs(smallText, 5);
        double largeMedianMs = MeasureMedianTimeMs(largeText, 5);

        double baselineMs = Math.Max(smallMedianMs, 0.05);
        double ratio = largeMedianMs / baselineMs;

        ratio.Should().BeLessThan(25.0);

        double MeasureMedianTimeMs(string input, int iterations)
        {
            if (iterations <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterations), "Iteration count must be positive.");

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var samples = new double[iterations];
            var sw = new Stopwatch();

            for (int i = 0; i < iterations; i++)
            {
                sw.Restart();
                var result = _md.Render(input);
                sw.Stop();

                GC.KeepAlive(result);

                samples[i] = sw.Elapsed.TotalMilliseconds;
            }

            Array.Sort(samples);

            int middle = iterations / 2;

            return iterations % 2 != 0
                ? samples[middle]
                : (samples[middle - 1] + samples[middle]) / 2.0;
        }
    }
}
