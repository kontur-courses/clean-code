using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser.Tags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MdTests
    {
        private Md md;

        private static readonly List<MarkdownTag> Tags =
            new List<MarkdownTag> {new ItalicTag(), new BoldTag()};

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        private static IEnumerable<TestCaseData> GenerateMarkdownAndHtmlResultForAllTags()
        {
            foreach (var tag in Tags)
            {
                var node = tag.CreateNode();
                var tagString = tag.String;
                var tagName = tag.GetType().Name;
                var startWrapper = node.StartWrapper;
                var endWrapper = node.EndWrapper;

                yield return new TestCaseData(
                        $"{tagString}this is plain text{tagString}",
                        $"{startWrapper}this is plain text{endWrapper}")
                    .SetName($"should wrap {tagName} text right");

                yield return new TestCaseData(
                        $"{tagString}42{tagString}56",
                        $"{tagString}42{tagString}56")
                    .SetName($"{tagName} between digits shouldn't wrapped");

                yield return new TestCaseData(
                        $"{tagString}this is \n text{tagString}",
                        $"{startWrapper}this is \n text{endWrapper}")
                    .SetName($"newline must be included in {tagName}-wrapped text");

                yield return new TestCaseData(
                        $"{tagString}this is \t text{tagString}",
                        $"{startWrapper}this is \t text{endWrapper}")
                    .SetName("tab must be included in {tagName}-wrapped text");

                foreach (var other in Tags.Where(other => other != tag))
                {
                    yield return new TestCaseData(
                            $"{tagString}this is text{other.String}",
                            $"{tagString}this is text{other.String}")
                        .SetName($"with {tagName} start and {other.GetType().Name} end shouldn't wrapped");
                }

                yield return new TestCaseData(
                        $"this{tagString}is text{tagString}",
                        $"this{tagString}is text{tagString}")
                    .SetName($"{tagName} start after no space not valid");

                yield return new TestCaseData(
                        $"{tagString}this is {tagString}text",
                        $"{tagString}this is {tagString}text")
                    .SetName($"{tagName} end before no space not valid");
            }
        }

        private static IEnumerable<TestCaseData> GetMarkdownAndHtmlResult()
        {
            yield return new TestCaseData("this is plain text", "this is plain text")
                .SetName("shouldn't wrap plain text");

            yield return new TestCaseData("__this is _plain_ text__", "<strong>this is <em>plain</em> text</strong>")
                .SetName("italic are wrapped inside bold");

            yield return new TestCaseData("_this is __plain__ text_", "<em>this is plain text</em>")
                .SetName("bold are ignored at italic");

            yield return new TestCaseData("\\_this is text_", "_this is text_")
                .SetName("escape start tag");

            yield return new TestCaseData("_this is text\\_", "_this is text_")
                .SetName("escape end tag");
        }

        [TestCaseSource(nameof(GenerateMarkdownAndHtmlResultForAllTags))]
        [TestCaseSource(nameof(GetMarkdownAndHtmlResult))]
        public void Render_SimpleCases_ShouldReturnRightHtml(string markdown, string html)
        {
            var actual = md.Render(markdown);

            actual.Should().BeEquivalentTo(html);
        }
    }
}