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
        public void RenderHtml_OnGrowingInputData_ComplexityGrowsLinearly()
        {
            const int growCoefficient = 10;

            var firstText = GenerateMarkdown(10 * growCoefficient);
            var secondText = GenerateMarkdown(10 * growCoefficient * growCoefficient);
            var thirdText = GenerateMarkdown(10 * growCoefficient * growCoefficient * growCoefficient);

            MeasureRenderingTime(GenerateMarkdown(5000));

            var firstTextRenderingTime = MeasureRenderingTime(firstText);
            var secondTextRenderingTime = MeasureRenderingTime(secondText);
            var thirdTextRenderingTime = MeasureRenderingTime(thirdText);

            var secondGrow = (double)thirdTextRenderingTime / secondTextRenderingTime;
            var firstGrow = (double)secondTextRenderingTime / firstTextRenderingTime;

            (secondGrow / firstGrow).Should().BeLessThan(5);
        }

        private static string GenerateMarkdown(int length) =>
            string.Join(" ", Enumerable.Repeat("_w_", length));

        private static long MeasureRenderingTime(string markdownText)
        {
            var stopwatch = Stopwatch.StartNew();

            Markdown.RenderHtml(markdownText);

            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
    }
}