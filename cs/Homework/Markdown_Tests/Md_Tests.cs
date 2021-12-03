using System;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Parser;
using Markdown.Renderer;
using NUnit.Framework;

namespace Markdown_Tests
{
    public class Md_Tests
    {
        private Md sut;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var parser = new MdTokenParser();
            var renderer = new MdToHtmlRenderer();
            sut = new Md(parser, renderer);
        }

        [TestCase("abc", ExpectedResult = "abc", TestName = "when plain text only")]
        [TestCase("_abc_", ExpectedResult = "<i>abc</i>", TestName = "when italic text only")]
        [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>", TestName = "when strong text only")]
        [TestCase("# abc", ExpectedResult = "<h1>abc</h1>", TestName = "when header only")]
        [TestCase("abc _cde_", ExpectedResult = "abc <i>cde</i>", TestName = "when plain text and italic")]
        public string Render_RenderCorrect(string text)
        {
            return sut.Render(text);
        }

        [Test]
        public void Render_RenderDifferentHeaders()
        {
            var tagTemplate = "######";
            for (var i = 1; i <= 6; i++)
            {
                var tag = tagTemplate[..i];
                var text = $"{tag} abc";
                sut.Render(text).Should().Be($"<h{i}>abc</h{i}>");
            }
        }

        [TestCase("__ab_cd_e__", ExpectedResult = "<strong>ab<i>cd</i>e</strong>")]
        [TestCase("# _abc_", ExpectedResult = "<h1><i>abc</i></h1>")]
        [TestCase("# __ab _cd_ fg__", ExpectedResult = "<h1><strong>ab <i>cd</i> fg</strong></h1>")]
        public string Render_RenderNestedTokens(string text)
        {
            return sut.Render(text);
        }

        [TestCase(@"\_ab_", ExpectedResult = "_ab_", TestName = "when screen italic selector")]
        [TestCase(@"\__ab__", ExpectedResult = "__ab__", TestName = "when screen strong selector")]
        [TestCase(@"\\", ExpectedResult = @"\", TestName = "when screen strong screening symbol")]
        [TestCase(@"\\_abc_", ExpectedResult = @"\<i>abc</i>", TestName = "screening symbol does not screen selector when under screen itself")]
        public string Render_RenderCorrectly_WhenScreening(string text)
        {
            return sut.Render(text);
        }

        [Test]
        public void Render_ShouldHaveLinearExecutionTime()
        {
            var testingString = "# abc __ab_abc_de__\n";
            var repeationCount = 100;

            var shortString = string.Concat(Enumerable.Repeat(testingString, repeationCount));
            var longString = string.Concat(Enumerable.Repeat(testingString, repeationCount * 100));

            var shortStringTime = MeasureActionTimeInMilliseconds(() => sut.Render(shortString));
            var longStringTime = MeasureActionTimeInMilliseconds(() => sut.Render(longString));

            longStringTime.Should().BeLessOrEqualTo((long)(shortStringTime * 100 * 1.25));

            Console.WriteLine(@$"string '{testingString}' repeated {repeationCount}: {shortStringTime}");
            Console.WriteLine(@$"string '{testingString}' repeated {repeationCount * 100}: {longStringTime}");
        }

        private static long MeasureActionTimeInMilliseconds(Action action)
        {
            var watch = new Stopwatch();

            action.Invoke();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            watch.Start();
            action.Invoke();
            watch.Stop();

            return watch.ElapsedMilliseconds;
        }
    }
}