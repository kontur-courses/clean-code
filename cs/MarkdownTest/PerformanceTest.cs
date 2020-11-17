using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture, Timeout(1000)]
    public class PerformanceTest_Should
    {
        [Test]
        public void PerformanceTest()
        {
            var testDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent;
            var specDir = testDirectory?.Parent?.Parent;
            if (testDirectory != null && specDir != null)
            {
                var mdText = new StreamReader($"{specDir.FullName}\\MarkdownSpec.md").ReadToEnd();
                var stopwatch = Stopwatch.StartNew();
                Md.Render(mdText);
                var firstStartTime = stopwatch.Elapsed;

                for (var i = 0; i < 1000; i++)
                {
                    stopwatch = Stopwatch.StartNew();
                    Md.Render(mdText);
                    var currentTime = stopwatch.Elapsed;
                    currentTime.Should().BeLessThan(firstStartTime * 2);
                }
            }
        }
    }
}