using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Renderer;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MdTests
    {
        private Md sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            sut = new Md(new HtmlRenderer());
        }

        [TestCaseSource(typeof(MdTestCases), nameof(MdTestCases.RenderTestCases))]
        public void RenderTest(string input, string expected)
        {
            var actual = sut.Render(input);

            actual.Should().Be(expected);
        }

        [Test]
        public void Render_ShouldHaveLinearComplexity()
        {
            const int repetitionsCount = 100;
            const string inputString = "# ![abc](abc) __a__ _a_ __a_a_a__ \\_a\\_ _a__a__a_ __a_a__a_ _a__a_a__ \n";
            var shortString = string.Concat(Enumerable.Repeat(inputString, repetitionsCount));
            var longString = string.Concat(Enumerable.Repeat(inputString, repetitionsCount * 10));

            var shortStringTime = MeasureInMilliseconds(() => sut.Render(shortString));
            var longStringTime = MeasureInMilliseconds(() => sut.Render(longString));

            longStringTime.Should().BeLessOrEqualTo((long)(shortStringTime * 10 * 1.1));
        }

        private static long MeasureInMilliseconds(Action action)
        {
            var stopwatch = new Stopwatch();
            action.Invoke();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();

            return stopwatch.ElapsedMilliseconds;
        }
    }
}