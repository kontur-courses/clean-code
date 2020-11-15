using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        private Md mdRender;

        [SetUp]
        public void SetUp()
        {
            mdRender = new Md();
        }

        private string CreateMdText(string formattingLine, int iteration = 1)
        {
            var text = new StringBuilder();
            for (int i = 0; i < iteration; i++)
                text.Append(formattingLine);
            return text.ToString();
        }

        private double GetAverageTime(string mdText, int iteration = 150)
        {
            var times = new double[iteration];
            var watch = new Stopwatch();
            for (int i = 0; i < iteration; i++)
            {
                watch.Start();
                mdRender.Render(mdText);
                watch.Stop();
                times[i] = watch.ElapsedMilliseconds;
                watch.Reset();
            }
            return times.Average();
        }

        [TestCase("_abc_f__ty__")]
        public void PerfomanceTest(string text)
        {
            WarmUpMethodStart(text);

            var averageTime = GetAverageTime(text);
            var oneThousandText = CreateMdText(text, 1000);
            var oneThousandTextAverageTime = GetAverageTime(oneThousandText);

            averageTime.Should().BeLessOrEqualTo(oneThousandTextAverageTime * 1000);
        }

        private void WarmUpMethodStart(string text)
        {
            for (int i = 0; i < 25; i++)
                mdRender.Render(text);
        }
    }
}
