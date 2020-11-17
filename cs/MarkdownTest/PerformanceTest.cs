using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    [TestFixture, Timeout(10000)]
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

                var miltiplier = 1;
                var text = "";
                for (var i = 0; i < 200; i++)
                {
                    text += mdText;
                    stopwatch = Stopwatch.StartNew();
                    Md.Render(text);
                    var currentTime = stopwatch.Elapsed;
                    currentTime.Should().BeLessThan(miltiplier * firstStartTime * 10);
                    miltiplier++;
                }
            }
        }
    }
}