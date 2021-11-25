using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;

namespace MarkdownTests
{
    internal class PerformanceTests
    {
        [TestCase(100, 100)]
        [TestCase(1000, 100)]
        [TestCase(100, 1000)]
        [Timeout(1000)]
        public void Should_WorkWithLinearComplexity(int actionCount, int scale)
        {
            var data = "# _abc_\n__def__";
            Action render = () => Md.Render(data);
            var scaledData = string.Join('\n', Enumerable.Repeat(data, scale));
            Action scaledRender = () => Md.Render(scaledData);

            Md.Render(data);

            var averageTime = MeasureAverage(render, actionCount);

            var averageTimeScaled = MeasureAverage(scaledRender, actionCount);

            averageTimeScaled.Should().BeLessThan(averageTime * scale * 2);
        }

        private TimeSpan MeasureAverage(Action action, int count)
        {
            GC.Collect();
            var timer = Stopwatch.StartNew();
            for (var i = 0; i < count; i++)
                action();
            timer.Stop();
            return timer.Elapsed / count;
        }
    }
}
