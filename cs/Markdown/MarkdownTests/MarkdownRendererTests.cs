using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownRendererTests
    {
        private static List<string> whiteSpaces = new() {" ", "\t", "\n"};

        [Test]
        public void Render_ScalesLinearly()
        {
            const int scale = 300;
            const string text = @"# \___bold _it\al\ic_ bold__";
            void act() => new MarkdownRenderer().Render(text);
            var scaledText = string.Join("", Enumerable.Repeat(text, scale));
            void scaledAct() => new MarkdownRenderer().Render(scaledText);

            var average = GetAverageExecutionTime(act);
            var averageScaled = GetAverageExecutionTime(scaledAct);

            var expected = average * scale * 5;
            TestContext.WriteLine(
                $"Average: {average}\nScale: {scale}\nExpected: {expected}\nActual: {averageScaled}");

            averageScaled.Should().BeLessThan(expected);
        }

        [Test]
        public void Render_ThrowsException_IfTextNull()
        {
            var renderer = new MarkdownRenderer();
            Assert.That(() => renderer.Render(null), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void Render_EmptyStringInsideTags() =>
            new MarkdownRenderer()
                .Render("____")
                .Should().Be("____");

        [Test]
        public void Render_IgnoreUnderlines_InsideTag() =>
            new MarkdownRenderer()
                .Render("_a ___ c_")
                .Should().Be("<em>a ___ c</em>");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_IfAfterTagStartIsWhitespace(string whitespace) =>
            new MarkdownRenderer()
                .Render($"a_{whitespace}b_")
                .Should().Be($"a_{whitespace}b_");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_IfBeforeTagEndIsWhitespace(string whitespace) =>
            new MarkdownRenderer()
                .Render($"_a{whitespace}_bc_")
                .Should().Be($"<em>a{whitespace}_bc</em>");

        [TestCase("__bold__", ExpectedResult = "<strong>bold</strong>", TestName = "Bold")]
        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "Italic")]
        [TestCase("# header", ExpectedResult = "<h1>header</h1>", TestName = "Header")]
        public string Render_SingleTag(string text) => new MarkdownRenderer().Render(text);

        [TestCase("_a_b", ExpectedResult = "<em>a</em>b", TestName = "In word start")]
        [TestCase("a_bc_d", ExpectedResult = "a<em>bc</em>d", TestName = "In word middle")]
        [TestCase("a_bc_", ExpectedResult = "a<em>bc</em>", TestName = "In word end")]
        [TestCase("_W_p _ab ba_", ExpectedResult = "<em>W</em>p <em>ab ba</em>",
            TestName = "Highlights phrase after tag in word")]
        public string Render_InWordPart(string text) => new MarkdownRenderer().Render(text);

        [TestCase("a_bc cb_a", ExpectedResult = "a_bc cb_a", TestName = "In different words")]
        [TestCase("__a _b__ c_", ExpectedResult = "__a _b__ c_", TestName = "Intersect")]
        [TestCase("__a_", ExpectedResult = "__a_", TestName = "Unpaired")]
        [TestCase("a_12_3", ExpectedResult = "a_12_3", TestName = "Inside number")]
        public string Render_IgnoreTags_IfTags(string text) => new MarkdownRenderer().Render(text);

        [TestCase("__bold _italic_ bold__", ExpectedResult = "<strong>bold <em>italic</em> bold</strong>",
            TestName = "Italic inside bold")]
        [TestCase("_i __b__ i_", ExpectedResult = "<em>i __b__ i</em>", TestName = "Bold inside italic ignored")]
        [TestCase("_i __b__ i", ExpectedResult = "_i <strong>b</strong> i", TestName = "Nesting ignore unclosed tags")]
        [TestCase("# h __b _i_ b__", ExpectedResult = "<h1>h <strong>b <em>i</em> b</strong></h1>",
            TestName = "Header can contain italic and bold")]
        public string Render_TagsInteraction(string text) => new MarkdownRenderer().Render(text);

        [TestCase(@"\_a\_", ExpectedResult = "_a_", TestName = "Escaped tags ignored")]
        [TestCase(@"\ab\cd\", ExpectedResult = @"\ab\cd\",
            TestName = "Keeps escape symbols, if they not escape something")]
        [TestCase(@"\\_a_", ExpectedResult = @"\<em>a</em>", TestName = "Escape symbol can be escaped")]
        [TestCase(@"\\\_a_", ExpectedResult = @"\_a_", TestName = "Escaped escape symbol not escape next char")]
        public string Render_SupportEscaping(string text) => new MarkdownRenderer().Render(text);

        private static float GetAverageExecutionTime(Action act)
        {
            const int iterations = 100;
            var timer = new Stopwatch();

            GC.Collect();
            timer.Start();
            Enumerable.Range(1, iterations).ToList().ForEach(_ => act());
            timer.Stop();

            return (float)timer.ElapsedMilliseconds / iterations;
        }
    }
}