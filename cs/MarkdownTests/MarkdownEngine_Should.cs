using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;
using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;

namespace MarkdownTests
{
    [TestFixture]
    [UseReporter(typeof(DiffReporter), typeof(FileLauncherReporter))]
    [UseApprovalSubdirectory("ApprovalHtmlPage")]
    public class MarkdownEngine_Should
    {
        [Test]
        public void Render_MarkdownSpecFile_ReturnCorrectHtml() =>
            Approvals.Verify(MarkdownEngine.Render(File.ReadAllText("MarkdownSpec.md")));

        [Test]
        [Description("Performance test")]
        public void Render_PerformanceTest()
        {
            var hundredThousandRepetitionTest = RepeatTestString(50000);
            var fiftyThousandTestTime = (double) GetRenderingTimeFor(hundredThousandRepetitionTest);

            var millionRepetitionTest = RepeatTestString(500000);
            var fiveHundredTestTime = (double) GetRenderingTimeFor(millionRepetitionTest);
            Console.WriteLine($"Time of working for fifty thousand test strings: {fiftyThousandTestTime}");
            Console.WriteLine($"Time of working for five hundred test strings: {fiveHundredTestTime}");


            var expected = 10 * fiftyThousandTestTime;
            var precision = (long) Math.Round(0.15 * expected);
            Console.WriteLine($"10x time of working for fifty thousand test strings: {expected}");
            Console.WriteLine($"Precision: {precision}");
            fiveHundredTestTime.Should().BeApproximately(expected, precision);
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