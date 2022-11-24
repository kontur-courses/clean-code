using FluentAssertions;
using Markdown.Markdown;
using NUnit.Framework;
using System.Diagnostics;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdShould
    {
        [TestCase(10000, TestName = "10000 times repeat")]
        [TestCase(100000, TestName = "100000 times repeat")]
        [TestCase(100, TestName = "100 times repeat")]
        [TestCase(1000, TestName = "1000 times repeat")]
        public void CheckTime_LinearComplexityAlgorithm(int count)
        {
            var time = new Stopwatch();
            time.Start();
            for (var i = 0; i < count; i++)
                RenderMarkdownString_WhenHaveAllTagsSimpleTest();
            var timeRepeat = time.Elapsed.Milliseconds;
            var times = (int)Math.Sqrt(count) * 10;
            var longString = string.Concat(Enumerable.Repeat("# markdown _test_ sentence__\n", times));
            time.Restart();
            Md.Render(longString);
            time.Elapsed.Milliseconds.Should().BeLessOrEqualTo(timeRepeat);
        }

        [Test]
        public void CheckTime_WhenManyTimesRender_ForLinearAlgorithm()
        {
            var time = new Stopwatch();
            var timeList = new List<double>();
            for (var i = 200; i <= 6400; i *= 2)
                GetTimeOfIteration(i, time, timeList);
            for (var iteration = 0; iteration < timeList.Count - 1; iteration++)
                (timeList[iteration + 1] / timeList[iteration]).Should().BeLessThan(Math.Pow(2, 2));
        }

        private static void GetTimeOfIteration(int i, Stopwatch time, List<double> timeList)
        {
            var markdownString = string.Concat(Enumerable.Repeat("# markdown _test_ sentence__\n", i));
            time.Restart();
            Md.Render(markdownString);
            timeList.Add(time.Elapsed.TotalMilliseconds);
            time.Stop();
        }
        [Test]
        public void RenderMarkdownString_WhenHaveAllTagsSimpleTest()
        {
            var mdString = "# markdown _test_ sentence__\n";
            Md.Render(mdString).Should().Be("\\<h1>markdown \\<em>test\\</em> sentence__\\</h1>");
        }
    }
}
