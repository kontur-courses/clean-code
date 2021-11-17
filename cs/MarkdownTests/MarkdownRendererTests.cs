using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MarkdownRendererTests
    {
        private static List<string> whiteSpaces = new() {" ", "\t", "\n"};

        [Test]
        public void RenderMatches_SingleMatch()
        {
            var renderer = new MarkdownRenderer("_italic_");
            var matches = new List<TokenMatch> {new() {Start = 0, Length = 8, Token = MarkdownTokensFactory.Italic()}};

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<em>italic</em>");
        }

        [Test]
        public void RenderMatches_TwoMatches()
        {
            var renderer = new MarkdownRenderer("_italic_ __bold__");
            var matches = new List<TokenMatch>
            {
                new() {Start = 0, Length = 8, Token = MarkdownTokensFactory.Italic()},
                new() {Start = 9, Length = 8, Token = MarkdownTokensFactory.Bold()}
            };

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<em>italic</em> <strong>bold</strong>");
        }

        [Test]
        public void RenderMatches_NestedMatches()
        {
            var renderer = new MarkdownRenderer("__bo _italic_ ld__");
            var matches = new List<TokenMatch>
            {
                new() {Start = 0, Length = 18, Token = MarkdownTokensFactory.Bold()},
                new() {Start = 5, Length = 8, Token = MarkdownTokensFactory.Italic()}
            };

            var actual = renderer.RenderMatches(matches);

            actual.Should().Be("<strong>bo <em>italic</em> ld</strong>");
        }

        [Test]
        public void Render_EmptyStringInsideTags() =>
            new MarkdownRenderer("_____")
                .Render()
                .Should().Be("_____");

        [Test]
        public void Render_IgnoreUnderlines_InsideTag() =>
            new MarkdownRenderer($"_a ___ c_")
                .Render()
                .Should().Be($"<em>a ___ c</em>");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_AfterTagStart(string whitespace) =>
            new MarkdownRenderer($"a_{whitespace}b_")
                .Render()
                .Should().Be($"a_{whitespace}b_");

        [TestCaseSource(nameof(whiteSpaces))]
        public void Render_IgnoreTags_BeforeTagEnd(string whitespace) =>
            new MarkdownRenderer($"_a{whitespace}_bc_")
                .Render()
                .Should().Be($"<em>a{whitespace}_bc</em>");

        [TestCaseSource(nameof(SingleTagCases))]
        public string Render_SingleTag(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(InWordPartCases))]
        public string Render_InWordPart(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(IgnoreTagsCases))]
        public string Render_IgnoreTags(string text) => new MarkdownRenderer(text).Render();

        [TestCaseSource(nameof(TagsInteractionBetweenThemselvesCases))]
        public string Render_TagsInteractionBetweenThemselves(string text) => new MarkdownRenderer(text).Render();

        private static IEnumerable<TestCaseData> SingleTagCases()
        {
            yield return new TestCaseData("__bold__") {ExpectedResult = "<strong>bold</strong>", TestName = "Bold"};
            yield return new TestCaseData("_italic_") {ExpectedResult = "<em>italic</em>", TestName = "Italic"};
        }

        private static IEnumerable<TestCaseData> InWordPartCases()
        {
            yield return new TestCaseData("_a_b") {ExpectedResult = "<em>a</em>b", TestName = "In word start"};
            yield return new TestCaseData("a_bc_d") {ExpectedResult = "a<em>bc</em>d", TestName = "In word middle"};
            yield return new TestCaseData("a_bc_") {ExpectedResult = "a<em>bc</em>", TestName = "In word end"};
        }

        private static IEnumerable<TestCaseData> IgnoreTagsCases()
        {
            yield return new TestCaseData("a_bc cb_a")
                {ExpectedResult = "a_bc cb_a", TestName = "In different words"};

            yield return new TestCaseData("__a _b__ c_")
                {ExpectedResult = "__a _b__ c_", TestName = "Intersect tags"};

            yield return new TestCaseData("__a_") {ExpectedResult = "__a_", TestName = "Unpaired"};
            yield return new TestCaseData("a_12_3")
                {ExpectedResult = "a_12_3", TestName = "Tags inside number"};
        }

        private static IEnumerable<TestCaseData> TagsInteractionBetweenThemselvesCases()
        {
            yield return new TestCaseData("__bold _italic_ bold__")
                {ExpectedResult = "<strong>bold <em>italic</em> bold</strong>", TestName = "Italic inside bold"};

            yield return new TestCaseData("_i __b__ i_")
                {ExpectedResult = "<em>i __b__ i</em>", TestName = "Bold inside italic ignored"};

            yield return new TestCaseData("_i __b__ i")
                {ExpectedResult = "_i <strong>b</strong> i", TestName = "Nesting ignore unclosed tags"};
        }
    }
}