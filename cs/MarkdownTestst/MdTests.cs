using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Renderer;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    internal class MdTests
    {
        private readonly IReadOnlyDictionary<string, HtmlTag> htmlTagsBySeparator = new Dictionary<string, HtmlTag>
        {
            { BoldToken.Separator, new HtmlTag("<strong>", "</strong>", true) },
            { ItalicToken.Separator, new HtmlTag("<em>", "</em>", true) },
            { HeaderToken.Separator, new HtmlTag("<h1>", "</h1>", true) },
            { ScreeningToken.Separator, new HtmlTag(string.Empty, string.Empty, false) },
            { ImageToken.Separator, new HtmlTag("<img >", string.Empty, false) }
        };

        private readonly IReadOnlyDictionary<string, Func<int, Token>> tokensBySeparator = new Dictionary<string, Func<int, Token>>
        {
            { ItalicToken.Separator, index => new ItalicToken(index) },
            { BoldToken.Separator, index => new BoldToken(index) },
            { HeaderToken.Separator, index => new HeaderToken(index) },
            { ScreeningToken.Separator, index => new ScreeningToken(index) },
            { ImageToken.Separator, index => new ImageToken(index) }
        };

        private Md sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var renderer = new HtmlRenderer(htmlTagsBySeparator);
            var parser = new MdParser(tokensBySeparator);
            sut = new Md(renderer, parser);
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