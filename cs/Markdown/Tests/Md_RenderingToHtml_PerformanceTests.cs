using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using FluentAssertions;
namespace Markdown.Tests
{
    [TestFixture, Explicit]
    public class Md_RenderingToHtml_PerformanceTests
    {
        [Test]
        public void RenderNonNestedTagsByLinearTime()
        {
            const double accuracyCoefficient = 1.2;

            var s = BenchmarkRunner.Run<RenderingNonNestedTags>();
            var results = RenderingNonNestedTags.RepeatsCounts()
                .Select((count, i) => s.Reports[i].ResultStatistics.Median / count)
                .ToArray();

            var maxValue = results[0] * accuracyCoefficient;
            results.All(res => res < maxValue).Should().BeTrue();
        }

        [Test]
        public void RenderNestedTagsByLinearTime()
        {
            const double accuracyCoefficient = 1.2;

            var s = BenchmarkRunner.Run<RenderingNestedTags>();
            var results = RenderingNestedTags.InnerCharsCounts()
                .Select((count, i) => s.Reports[i].ResultStatistics.Median / count)
                .ToArray();

            var maxValue = results[0] * accuracyCoefficient;
            results.All(res => res < maxValue).Should().BeTrue();
        }
    }

    public class RenderingNonNestedTags
    {
        public static int[] RepeatsCounts() => new[] { 1, 10, 30 };

        [Benchmark]
        [ArgumentsSource(nameof(RepeatsCounts))]
        public string Run(int repeatsCount)
        {
            const string template = @"a __bb__ ccc _d_ \_e\_";
            var currentText = string.Concat(Enumerable.Repeat(template, repeatsCount));
            return new Md().RenderToHtml(currentText);
        }
    }

    public class RenderingNestedTags
    {
        public static int[] InnerCharsCounts() => new[] { 1, 10, 30 };

        [Benchmark]
        [ArgumentsSource(nameof(InnerCharsCounts))]
        public string Run(int innerCharsCount)
        {
            var currentText = "__b _" + new string('a', innerCharsCount) + "_ b__";
            return new Md().RenderToHtml(currentText);
        }
    }
}