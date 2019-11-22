using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    public class TreeBuilderTests
    {
        private TreeBuilder treeBuilder = new TreeBuilder();

        private static readonly List<MarkdownTag> Tags =
            new List<MarkdownTag> {new ItalicTag(), new BoldTag()};

        [SetUp]
        public void SetUp()
        {
            treeBuilder = new TreeBuilder();
        }


        private static void CheckTree(Node expected, Node actual)
        {
            actual.Should().BeEquivalentTo(expected,
                op => op.RespectingRuntimeTypes().Excluding(x =>
                    x.SelectedMemberInfo.DeclaringType == typeof(Node) &&
                    x.SelectedMemberInfo.Name == nameof(Node.Parent)));
        }

        [Test]
        public void BuildTree_OnlyPlainText_ShouldReturnTreeFromThisText()
        {
            var markdown = "this is plain text";
            var expected = new RootNode();
            expected.AddNode(new PlainTextNode(markdown));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        private static IEnumerable<TestCaseData> GenerateTextInRightTags()
        {
            foreach (var tag in Tags)
            {
                yield return new TestCaseData(
                        $"{tag.String}this is plain text{tag.String}",
                        tag.CreateNode(),
                        "this is plain text")
                    .SetName($"text in {tag.GetType().Name} tags");
            }
        }

        [TestCaseSource(nameof(GenerateTextInRightTags))]
        public void BuildTree_MarkdownWithRightTags_ShouldReturnTreeWithThisTagsNode(string markdown, Node tagNode,
            string expectedText)
        {
            var expected = new RootNode();
            expected.AddNode(tagNode);
            tagNode.AddNode(new PlainTextNode(expectedText));


            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }


        private static IEnumerable<TestCaseData> GenerateNestedTags()
        {
            foreach (var tag in Tags)
            {
                foreach (var other in Tags.Where(other => tag != other))
                {
                    var expectedResult = new RootNode();
                    var external = tag.CreateNode();
                    expectedResult.AddNode(external);
                    external.AddNode(new PlainTextNode("this is "));
                    var otherNode = other.CreateNode();
                    external.AddNode(otherNode);
                    otherNode.AddNode(new PlainTextNode("plain"));
                    external.AddNode(new PlainTextNode(" text"));

                    yield return new TestCaseData(
                            $"{tag.String}this is {other.String}plain{other.String} text{tag.String}",
                            expectedResult)
                        .SetName($"{other.GetType().Name} tag nested in {tag.GetType().Name} tag");
                }
            }
        }

        [TestCaseSource(nameof(GenerateNestedTags))]
        public void BuildTree_MarkdownWithNestedTags_ShouldReturnTreeWithNestedNodes(
            string markdown, RootNode expected)
        {
            var actual = treeBuilder.ParseMarkdown(markdown);
            CheckTree(expected, actual);
        }


        private static IEnumerable<TestCaseData> GenerateTabAndNewLineInsideValidTags()
        {
            foreach (var tag in Tags)
            {
                var expected = new RootNode();
                var tagNode = tag.CreateNode();
                expected.AddNode(tagNode);
                tagNode.AddNode(new PlainTextNode("this is \nplain \t text"));

                yield return new TestCaseData(
                        $"{tag.String}this is \nplain \t text{tag.String}",
                        expected)
                    .SetName($"new line and tab inside {tag.GetType().Name} tags");
            }
        }

        [TestCaseSource(nameof(GenerateTabAndNewLineInsideValidTags))]
        public void BuildTree_TabAndNewLineBetweenValidTags_ShouldReturnTreeWithThisTagsNode(
            string markdown, RootNode expected)
        {
            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        private static IEnumerable<TestCaseData> GenerateCoherentValidTags()
        {
            foreach (var tag in Tags)
            {
                foreach (var other in Tags.Where(other => tag != other))
                {
                    var markdown = $"{tag.String}first{tag.String} {other.String}second{other.String}";
                    var expected = new RootNode();
                    var first = tag.CreateNode();
                    var second = other.CreateNode();
                    expected.AddNode(first);
                    expected.AddNode(new PlainTextNode(" "));
                    expected.AddNode(second);
                    first.AddNode(new PlainTextNode("first"));
                    second.AddNode(new PlainTextNode("second"));

                    yield return new TestCaseData(markdown, expected)
                        .SetName($"{other.GetType().Name} tags after {tag.GetType().Name} tags");
                }
            }
        }

        [TestCaseSource(nameof(GenerateCoherentValidTags))]
        public void BuildTree_CoherentValidTags_ShouldReturnTreeWithRightNodes(
            string markdown, RootNode expected)
        {
            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        private static IEnumerable<TestCaseData> GenerateTagsBetweenDigits()
        {
            return Tags.Select(tag => new TestCaseData($"{tag.String}4{tag.String}2")
                .SetName($"{tag.GetType().Name} tag between digits"));
        }

        [TestCaseSource(nameof(GenerateTagsBetweenDigits))]
        public void BuildTree_TagsBetweenDigits_ShouldReturnTreeWithOnlyPlainText(string markdown)
        {
            var expected = new RootNode();
            expected.AddNode(new PlainTextNode(markdown));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase(@"\_text_", "_text_")]
        [TestCase(@"_text\_", "_text_")]
        public void BuildTree_MarkdownWithEscapedItalicTag_ShouldReturnTreeWithOnlyPlainText(
            string markdown, string expectedString)
        {
            var expected = new RootNode();
            expected.AddNode(new PlainTextNode(expectedString));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase(@"_\_text_", "_text")]
        [TestCase(@"_text\__", "text_")]
        public void BuildTree_EscapedBoldTagBecomeItalic_ShouldReturnTreeWithItalic(string markdown,
            string expectedText)
        {
            var expected = new RootNode();
            var italic = new ItalicNode();
            expected.AddNode(italic);
            italic.AddNode(new PlainTextNode(expectedText));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase(@"\__text_", "__text_")]
        [TestCase(@"_text_\_", "_text__")]
        public void BuildTree_EscapedBoldTagNotBecomeItalic_ShouldReturnTreeWithPlainText(
            string markdown, string expectedString)
        {
            var expected = new RootNode();
            expected.AddNode(new PlainTextNode(expectedString));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }
    }
}