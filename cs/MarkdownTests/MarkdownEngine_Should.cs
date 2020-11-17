using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownEngine_Should
    {
        [Test]
        [Timeout(30000)]
        [Description("Performance test")]
        public void Render_PerformanceTest()
        {
            var hundredThousandRepetitionTest = RepeatTestString(100000);
            var hundredThousandTestTime = GetRenderingTimeFor(hundredThousandRepetitionTest);

            var millionRepetitionTest = RepeatTestString(1000000);
            var millionTestTime = GetRenderingTimeFor(millionRepetitionTest);

            millionTestTime.Should().BeLessOrEqualTo(hundredThousandTestTime * 10);
        }

        private static string RepeatTestString(int repetitionCount)
        {
            const string test = "# __[Url](http://usr.com)__ _test_ _with _random_ tags_ and __string [Text]( ";
            return string.Concat(Enumerable.Repeat(test, repetitionCount));
        }

        private static long GetRenderingTimeFor(string input)
        {
            var timer = Stopwatch.StartNew();
            MarkdownEngine.Render(input);
            timer.Stop();

            return timer.ElapsedMilliseconds;
        }
    }
}