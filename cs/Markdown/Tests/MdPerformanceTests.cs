using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdPerformanceTests
    {
        private const string ParagraphPart =
            @"__ртф _урфу_ попв__ \_ртф\_ ур_фу по_пв ртф_12_3 __урфу _попв__ ртф_ ____ _урфу попв ";

        private const string SeveralParagraphsPart =
            "# __урфу__ ртф _попв_\n- попв _ртф_ урфу\nртф \\_урфу\\_ по_пв_ в ртф\n";

        private const int SmallRepeatCount = 1_000;
        private const int SizeMultiplier = 8;
        private const int NoiseFactor = 3;
        private const int MaxTimeRatio = SizeMultiplier * NoiseFactor;
        private const int RunCount = 5;

        [Test]
        public void Render_ShouldWorkInLinearTime_WhenParagraphGrows_Test()
        {
            var timeRatio = GetTimeRatio(ParagraphPart);

            timeRatio
                .Should()
                .BeLessThan(MaxTimeRatio);
        }

        [Test]
        public void Render_ShouldWorkInLinearTime_WhenParagraphCountGrows_Test()
        {
            var timeRatio = GetTimeRatio(SeveralParagraphsPart);

            timeRatio
                .Should()
                .BeLessThan(MaxTimeRatio);
        }

        private static double GetTimeRatio(string markdownPart)
        {
            var smallMarkdown = Repeat(markdownPart, SmallRepeatCount);
            var largeMarkdown = Repeat(markdownPart, SmallRepeatCount * SizeMultiplier);

            var smallTime = GetBestRenderTime(smallMarkdown);
            var largeTime = GetBestRenderTime(largeMarkdown);

            return largeTime / smallTime;
        }

        private static double GetBestRenderTime(string markdown)
        {
            Md.Render(markdown);
            var bestTime = double.MaxValue;
            for (var i = 0; i < RunCount; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                Md.Render(markdown);
                stopwatch.Stop();
                bestTime = Math.Min(bestTime, stopwatch.Elapsed.TotalMilliseconds);
            }

            return bestTime;
        }

        private static string Repeat(string part, int count)
        {
            var text = new StringBuilder(part.Length * count);
            for (var i = 0; i < count; i++)
            {
                text.Append(part);
            }

            return text.ToString();
        }
    }
}
