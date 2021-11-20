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
        public void Constructor_ThrowsException_TextNull()
        {
            Assert.Throws<ArgumentException>(() =>
                new MarkdownRenderer(null));
        }

        [Test]
        public void Render_ScalesLinearly()
        {
            const int scale = 300;
            const string text = @"# \___bold _it\al\ic_ bold__";
            void act() => new MarkdownRenderer(text).Render();
            var scaledText = string.Join("", Enumerable.Repeat(text, scale));
            void scaledAct() => new MarkdownRenderer(scaledText).Render();

            var average = GetAverageExecutionTime(act);
            var averageScaled = GetAverageExecutionTime(scaledAct);

            var expected = average * scale * 5;
            TestContext.WriteLine(
                $"Average: {average}\nScale: {scale}\nExpected: {expected}\nActual: {averageScaled}");

            averageScaled.Should().BeLessThan(expected);
        }

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

        [Test]
        public void Render_EmptyStringInsideTags() =>
            new MarkdownRenderer("____")
                .Render()
                .Should().Be("____");

        [Test]
        public void Render_IgnoreUnderlines_InsideTag() =>
            new MarkdownRenderer("_a ___ c_")
                .Render()
                .Should().Be("<em>a ___ c</em>");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_AfterTagStartWhitespace(string whitespace) =>
            new MarkdownRenderer($"a_{whitespace}b_")
                .Render()
                .Should().Be($"a_{whitespace}b_");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_BeforeTagEndWhitespace(string whitespace) =>
            new MarkdownRenderer($"_a{whitespace}_bc_")
                .Render()
                .Should().Be($"<em>a{whitespace}_bc</em>");

        [TestCaseSource(nameof(SingleTagCases))]
        public string Render_SingleTag(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(InWordPartCases))]
        public string Render_InWordPart(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(IgnoreTagsCases))]
        public string Render_IgnoreTags_IfTags(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(TagsInteractionCases))]
        public string Render_TagsInteraction(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(EscapingCases))]
        public string Render_SupportEscaping(string text) => new MarkdownRenderer(text).Render();

        private static IEnumerable<TestCaseData> SingleTagCases()
        {
            yield return new TestCaseData("__bold__") {ExpectedResult = "<strong>bold</strong>", TestName = "Bold"};
            yield return new TestCaseData("_italic_") {ExpectedResult = "<em>italic</em>", TestName = "Italic"};
            yield return new TestCaseData("# header") {ExpectedResult = "<h1>header</h1>", TestName = "Header"};
        }

        private static IEnumerable<TestCaseData> InWordPartCases()
        {
            yield return new TestCaseData("_a_b") {ExpectedResult = "<em>a</em>b", TestName = "In word start"};
            yield return new TestCaseData("a_bc_d") {ExpectedResult = "a<em>bc</em>d", TestName = "In word middle"};
            yield return new TestCaseData("a_bc_") {ExpectedResult = "a<em>bc</em>", TestName = "In word end"};

            yield return new TestCaseData("q_W_p _ab ba_")
                {ExpectedResult = "q<em>W</em>p <em>ab ba</em>", TestName = "Highlights phrase after tag in word"};
        }

        private static IEnumerable<TestCaseData> IgnoreTagsCases()
        {
            yield return new TestCaseData("a_bc cb_a")
                {ExpectedResult = "a_bc cb_a", TestName = "In different words"};

            yield return new TestCaseData("__a _b__ c_")
                {ExpectedResult = "__a _b__ c_", TestName = "Intersect"};

            yield return new TestCaseData("__a_") {ExpectedResult = "__a_", TestName = "Unpaired"};
            yield return new TestCaseData("a_12_3")
                {ExpectedResult = "a_12_3", TestName = "Inside number"};
        }

        private static IEnumerable<TestCaseData> TagsInteractionCases()
        {
            yield return new TestCaseData("__bold _italic_ bold__")
                {ExpectedResult = "<strong>bold <em>italic</em> bold</strong>", TestName = "Italic inside bold"};

            yield return new TestCaseData("_i __b__ i_")
                {ExpectedResult = "<em>i __b__ i</em>", TestName = "Bold inside italic ignored"};

            yield return new TestCaseData("_i __b__ i")
                {ExpectedResult = "_i <strong>b</strong> i", TestName = "Nesting ignore unclosed tags"};

            yield return new TestCaseData("# h __b _i_ b__")
            {
                ExpectedResult = "<h1>h <strong>b <em>i</em> b</strong></h1>",
                TestName = "Header can contain italic and bold"
            };
        }

        private static IEnumerable<TestCaseData> EscapingCases()
        {
            yield return new TestCaseData(@"\_a\_") {ExpectedResult = "_a_", TestName = "Escaped tags ignored"};
            yield return new TestCaseData(@"\ab\cd\")
                {ExpectedResult = @"\ab\cd\", TestName = "Keeps escape symbols, if they not escape something"};

            yield return new TestCaseData(@"\\_a_")
                {ExpectedResult = @"\<em>a</em>", TestName = "Escape symbol can be escaped"};

            yield return new TestCaseData(@"\\\_a_")
                {ExpectedResult = @"\_a_", TestName = "Escaped escape symbol not escape next char"};
        }
    }
}