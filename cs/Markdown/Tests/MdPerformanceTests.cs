using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdPerformanceTests
{
    private readonly Md md = new();

    [TestCase("# Заголовок __с _разными_ символами__ и [ссылкой](url)\n\n")]
    [TestCase("_это ра_ботает так_ __пересечение _двойных__ и одинарных_ [a _b](u) c_ ")]
    [TestCase("текст](без закрытия скобки ](еще ](и еще _непарный __подчерк ")]
    public void Render_ScalesLinearlyWithInputSize(string fragment)
    {
        var smallInput = Repeat(fragment, 1_000);
        var largeInput = Repeat(fragment, 10_000);

        md.Render(smallInput);
        md.Render(largeInput);

        var smallTime = MeasureMinTime(() => md.Render(smallInput));
        var largeTime = MeasureMinTime(() => md.Render(largeInput));

        largeTime.TotalMilliseconds.Should().BeLessThan(
            Math.Max(smallTime.TotalMilliseconds * 25, 50),
            "при увеличении входа в 10 раз время работы линейного алгоритма не должно расти квадратично");
    }

    private static string Repeat(string fragment, int count)
    {
        var builder = new StringBuilder(fragment.Length * count);

        for (var i = 0; i < count; i++)
            builder.Append(fragment);

        return builder.ToString();
    }

    private static TimeSpan MeasureMinTime(Action action, int runs = 5)
    {
        var minElapsed = TimeSpan.MaxValue;

        for (var i = 0; i < runs; i++)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();

            if (stopwatch.Elapsed < minElapsed)
                minElapsed = stopwatch.Elapsed;
        }

        return minElapsed;
    }
}
