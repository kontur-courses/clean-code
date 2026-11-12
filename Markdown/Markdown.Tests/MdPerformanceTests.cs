using System.Diagnostics;
using AwesomeAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

/// <summary>
/// время ограничено не секундами, а ростом - при увеличении входа в 10 раз
/// линейный алгоритм замедляется примерно в 10 раз, квадратичный - в 100
/// берется лучшее из нескольких замеров
/// </summary>
[TestFixture]
[Category("Performance")]
public class MdPerformanceTests
{
    private const int SmallRepetitions = 2_000;
    private const int GrowthFactor = 10;
    private const int MeasurementCount = 5;
    private const double AllowedSlowdown = GrowthFactor * 3;

    private static readonly TestCaseData[] InputShapes =
    {
        Shape(
            "курсив внутри полужирного",
            "__жирный _курсив_ текст__ "),

        Shape(
            "незакрытый курсив",
            "_слово "),

        Shape(
            "незакрытые полужирный текст и курсив внутри",
            "__слово _другое "),

        Shape(
            "пересечение курсива и полужирного текстов",
            "__пересечение _двойных__ и одинарных_ "),

        Shape(
            "подчерк внутни разных слов",
            "ра_зные сл_ова "),

        Shape(
            "Экранированные подчерки",
            "\\_слово\\_ "),

        Shape(
            "и курсив и полужирный текст в одной строке",
            "строка с _курсивом_ и __жирным__\n"),

        Shape(
            "абзац с курсивом",
            "абзац с _курсивом_\n\n"),

        Shape(
            "заголовок с полужирным текстом",
            "# заголовок с __жирным__\n\n"),

        Shape(
            "пункт списка с курсивом",
            "- пункт с _курсивом_\n"),
    };

    [TestCaseSource(nameof(InputShapes))]
    public void Render_InputGrowsTenfold_TimeGrowsRoughlyLinearly(string fragment)
    {
        var small = Repeat(fragment, SmallRepetitions);
        var large = Repeat(fragment, SmallRepetitions * GrowthFactor);

        Md.Render(small); // прогрев JIT

        var smallTime = MeasureBest(small);
        var largeTime = MeasureBest(large);

        var slowdown = largeTime / smallTime;

        slowdown.Should().BeLessThan(AllowedSlowdown);
    }

    [Test]
    public void Render_LargeDocument_CompletesWithinTimeLimit()
    {
        var input = Repeat("__жирный _курсив_ текст__ ", 20_000);

        Md.Render("__прогрев__");

        var stopwatch = Stopwatch.StartNew();

        var actual = Md.Render(input);

        stopwatch.Stop();

        actual.Should().NotBeEmpty();
        stopwatch.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(5));
    }

    private static TestCaseData Shape(string name, string fragment)
    {
        return new TestCaseData(fragment)
            .SetName(name)
            .SetDescription(
                $"Увеличение объёма входных данных в десять раз должно потребовать примерно в десять раз больше времени: {name}.");
    }

    private static string Repeat(string fragment, int count)
    {
        return string.Concat(Enumerable.Repeat(fragment, count));
    }

    // Лучшее время в миллисекундах
    private static double MeasureBest(string input)
    {
        var best = double.MaxValue;

        for (var i = 0; i < MeasurementCount; i++)
        {
            var stopwatch = Stopwatch.StartNew();

            Md.Render(input);

            stopwatch.Stop();
            best = Math.Min(best, stopwatch.Elapsed.TotalMilliseconds);
        }

        return Math.Max(best, 0.001); // избежание деления на ZeroDivisionError
    }
}
