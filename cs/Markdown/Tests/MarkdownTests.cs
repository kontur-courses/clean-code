using System.Diagnostics;
using FluentAssertions;
using Markdown.TagUtils.Implementations;
using Markdown.TokensUtils.Implementations;
using NUnit.Framework;

namespace Markdown.Tests;

public class MarkdownTests
{
    private TagProcessor tagProcessor;
    private Tokenizer tokenizer;
    private IRender render;
    private TagContext tagContext;
    private TagStrategyFactory factory;

    public MarkdownTests()
    {
        tagContext = new TagContext();
        factory = new TagStrategyFactory(tagContext);
        tagProcessor = new TagProcessor(tagContext, factory);
        tokenizer = new Tokenizer();
        render = new MarkdownRender(tokenizer, tagProcessor);
    }

    [TestCaseSource(nameof(MarkdownCases))]
    public void Markdown_RenderText_ShouldMatchExpected(string line, string expectedAndErrorMessage)
    {
        var result = render.RenderText(line);
        result.Should().Be(expectedAndErrorMessage, expectedAndErrorMessage);
    }

    [Test]
    public void Markdown_RenderTime_ShouldScaleLinearly()
    {
        var sizes = new[] { 100, 1000, 3000, 100000 };
        int? previousSize = null;
        double? previousTime = null;
        const int runsPerSize = 5;

        foreach (var size in sizes)
        {
            var part = size / 3;
            var lines = Enumerable.Range(1, size)
                .Select(i =>
                {
                    if (i <= part)
                    {
                        return $"_line {i}_";
                    }
                    return i <= part * 2 ? $"__line {i}__" : $"#line {i}";
                });
            var input = string.Join(Environment.NewLine, lines);
            var times = new List<double>(runsPerSize);

            for (var run = 0; run < runsPerSize; run++)
            {
                var sw = Stopwatch.StartNew();
                render.RenderText(input);
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            var medianTime = times.OrderBy(t => t).ElementAt(runsPerSize / 2);
            
            if (previousTime.HasValue && previousSize.HasValue)
            {
                var timeRatio = medianTime / previousTime.Value;
                var sizeRatio = (double)size / previousSize.Value;

                timeRatio.Should().BeLessThanOrEqualTo(sizeRatio * 1.5);
            }

            previousTime = medianTime;
            previousSize = size;
        }
    }


    public static IEnumerable<TestCaseData> MarkdownCases
    {
        get
        {
            yield return new TestCaseData(
                    "Текст, _окруженный с двух сторон_ одинарными символами подчерка",
                    "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка")
                .SetName("SingleUnderscores_Em");

            yield return new TestCaseData(
                    "__Выделенный двумя символами текст__ должен становиться полужирным",
                    "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным")
                .SetName("DoubleUnderscores_Strong");

            yield return new TestCaseData(
                    "Любой тег можно экранировать \\<em> и даже \\<strong>",
                    "Любой тег можно экранировать \\<em> и даже \\<strong>")
                .SetName("EscapeMarkdownTags");

            yield return new TestCaseData(
                    "Любой символ можно экранировать, чтобы он не считался частью разметки.\n\\_Вот это\\_, " +
                    "не должно выделиться тегом \\<em>. \n Также \\__Это не выделяется\\__ тегом \\<strong>.",
                    "Любой символ можно экранировать, чтобы он не считался частью разметки.\n\\_Вот это\\_, " +
                    "не должно выделиться тегом \\<em>. \n Также \\__Это не выделяется\\__ тегом \\<strong>.")
                .SetName("EscapedUnderscores_NoRender");

            yield return new TestCaseData(
                    "Внутри __двойного выделения _одинарное_ тоже__ работает.",
                    "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")
                .SetName("Nested_StrongAndEm");

            yield return new TestCaseData(
                    "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
                    "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.")
                .SetName("StrongInsideEm_NoRender");

            yield return new TestCaseData(
                    "Подчерки внутри текста c цифрами_12_3 или 1__1__23 не считаются выделением и должны оставаться символами подчерка.",
                    "Подчерки внутри текста c цифрами_12_3 или 1__1__23 не считаются выделением и должны оставаться символами подчерка.")
                .SetName("UnderscoresAroundNumbers_NoRender");

            yield return new TestCaseData(
                    "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон__це.__",
                    "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<strong>це.</strong>")
                .SetName("UnderscoresInsideWord_Render");

            yield return new TestCaseData(
                    "В то же время выделение в ра_зных сл_овах не работает.",
                    "В то же время выделение в ра_зных сл_овах не работает.")
                .SetName("UnderscoresAcrossWords_NoRender");

            yield return new TestCaseData(
                    "__Непарные_ символы в рамках одного абзаца не считаются выделением.",
                    "__Непарные_ символы в рамках одного абзаца не считаются выделением.")
                .SetName("UnmatchedUnderscores_NoRender");

            yield return new TestCaseData(
                    "За подчерками, начинающими выделение, должен следовать непробельный символ. " +
                    "Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка.",
                    "За подчерками, начинающими выделение, должен следовать непробельный символ. " +
                    "Иначе эти_ подчерки_ не считаются выделением \nи остаются просто символами подчерка.")
                .SetName("StartingUnderscoreFollowedByWhitespace_NoRender");

            yield return new TestCaseData(
                    "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                    "Иначе эти _подчерки _не считаются_ окончанием выделения \nи остаются просто символами подчерка.",
                    "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. " +
                    "Иначе эти _подчерки <em>не считаются</em> окончанием выделения \nи остаются просто символами подчерка.")
                .SetName("EndingUnderscorePrecededByWhitespace_Render");

            yield return new TestCaseData(
                    "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.\n",
                    "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.\n")
                .SetName("IntersectingSingleAndDoubleUnderscores_NoRender");

            yield return new TestCaseData(
                    "Если внутри подчерков пустая строка _____, то они остаются символами подчерка.\n",
                    "Если внутри подчерков пустая строка _____, то они остаются символами подчерка.\n")
                .SetName("EmptyDoubleUnderscores_NoRender");

            yield return new TestCaseData(
                    "# Заголовок __с _разными_ символами__",
                    "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")
                .SetName("H1Tag_Render");

            yield return new TestCaseData(
                    "___Слово___",
                    "___Слово___")
                .SetName("TripleUnderscore_NoRender");

            yield return new TestCaseData(
                    "[репозиторий gogy](https://github.com/gogy4?tab=repositories)",
                    "<a href=\"https://github.com/gogy4?tab=repositories\">репозиторий gogy</a>")
                .SetName("CorrectLink_Render");

            yield return new TestCaseData(
                    "[Неполная ссылка(ссылка)",
                    "[Неполная ссылка(ссылка)")
                .SetName("IncorrectLink_NoRender");

            yield return new TestCaseData(
                    "[Google](https://google.com) и [YouTube](https://youtube.com)",
                    "<a href=\"https://google.com\">Google</a> и <a href=\"https://youtube.com\">YouTube</a>")
                .SetName("MultipleLinks_RenderAll");

            yield return new TestCaseData(
                    "[Текст с ошибкой[внутри]()",
                    "[Текст с ошибкой[внутри]()")
                .SetName("IncorrectLink_NestedBrackets");

            yield return new TestCaseData(
                    "[Ссылка без закрытия](https://example.com",
                    "[Ссылка без закрытия](https://example.com")
                .SetName("IncorrectLink_MissingClosingParenthesis");
            
            yield return new TestCaseData(
                    "[Ссылка без открытия]https://example.com",
                    "[Ссылка без открытия]https://example.com")
                .SetName("IncorrectLink_MissingOpeningParenthesis");

            yield return new TestCaseData(
                    "Текст без открывающей скобки](https://example.com)",
                    "Текст без открывающей скобки](https://example.com)")
                .SetName("IncorrectLink_MissingOpeningBracket");

            yield return new TestCaseData(
                    "[Текст без закрывающей скобки(https://example.com)",
                    "[Текст без закрывающей скобки(https://example.com)")
                .SetName("IncorrectLink_MissingClosingBracket");

            yield return new TestCaseData(
                    "(https://example.com)[Неправильный порядок]",
                    "(https://example.com)[Неправильный порядок]")
                .SetName("IncorrectLink_ReversedOrder");

            yield return new TestCaseData(
                    "[](Ссылка без текста https://example.com)",
                    "[](Ссылка без текста https://example.com)")
                .SetName("IncorrectLink_EmptyText");

            yield return new TestCaseData(
                    "[Текст с пустой ссылкой]()",
                    "[Текст с пустой ссылкой]()")
                .SetName("IncorrectLink_EmptyUrl");
        }
    }
}