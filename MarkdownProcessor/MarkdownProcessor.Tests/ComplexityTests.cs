using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor.Tests
{
    [TestFixture]
    public class ComplexityTests
    {
        [Test]
        [Repeat(5)]
        public void RenderHtml_OnGrowingInputData_ComplexityGrowsLinearly()
        {
            var firstText = GenerateMarkdown(100);
            var secondText = GenerateMarkdown(1000);
            var thirdText = GenerateMarkdown(10000);

            MeasureRenderingTime(GenerateMarkdown(1000));

            var firstTextRenderingTime = MeasureRenderingTime(firstText);
            var secondTextRenderingTime = MeasureRenderingTime(secondText);
            var thirdTextRenderingTime = MeasureRenderingTime(thirdText);

            var secondGrow = (double)thirdTextRenderingTime / secondTextRenderingTime;
            var firstGrow = (double)secondTextRenderingTime / firstTextRenderingTime;

            (secondGrow / firstGrow).Should().BeLessThan(10 + 5);
        }

        private static string GenerateMarkdown(int length) =>
            string.Concat(Enumerable.Repeat("__q__\\_w_", length));

        private static long MeasureRenderingTime(string markdownText)
        {
            var stopwatch = Stopwatch.StartNew();

            Markdown.RenderHtml(markdownText);

            stopwatch.Stop();

            return stopwatch.ElapsedTicks;
        }
    }
}