using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Parser.Tags;
using Markdown.Tools;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    public class TreeBuilderTests
    {
        private TreeBuilder treeBuilder;

        [SetUp]
        public void SetUp()
        {
            var tags = new List<MarkdownTag> {new BoldTag(), new ItalicTag()};
            var classifier = new CharClassifier(tags.SelectMany(t => t.String));

            treeBuilder = new TreeBuilder(tags, classifier);
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

        [Test]
        public void BuildTree_MarkdownWithRightUnderling_ShouldReturnTreeWithItalicNode()
        {
            var markdown = "_this is plain text_";
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            expected.AddNode(italic);
            italic.AddNode(new PlainTextNode(text));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_MarkdownWithRightDoubleUnderling_ShouldReturnTreeWithBoldNode()
        {
            var markdown = "__this is plain text__";
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var bold = new BoldNode();
            expected.AddNode(bold);
            bold.AddNode(new PlainTextNode(text));


            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_MarkdownWithDoubleUnderlingBetweenSingleUnderline_ShouldReturnTreeWithItalicAndBoldNodes()
        {
            var markdown = "_this is __plain__ text_";
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            var bold = new BoldNode();
            var textPart = text.Split('_');

            expected.AddNode(italic);
            italic.AddNode(new PlainTextNode(textPart[0]));
            italic.AddNode(bold);
            bold.AddNode(new PlainTextNode(textPart[2]));
            italic.AddNode(new PlainTextNode(textPart[4]));


            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_MarkdownWithSingleUnderlingBetweenDoubleUnderline_ShouldReturnTreeWithItalicAndBoldNodes()
        {
            var markdown = "__this is _plain_ text__";
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            var bold = new BoldNode();
            var textPart = text.Split('_');

            expected.AddNode(bold);
            bold.AddNode(new PlainTextNode(textPart[0]));
            bold.AddNode(italic);
            italic.AddNode(new PlainTextNode(textPart[1]));
            bold.AddNode(new PlainTextNode(textPart[2]));


            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase("_this is \nplain text_")]
        [TestCase("_this is \tplain text_")]
        public void BuildTree_MarkdownWithTabAndNewLineBetweenSingleUnderling_ShouldReturnTreeWithItalicNode(
            string markdown)
        {
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            expected.AddNode(italic);
            italic.AddNode(new PlainTextNode(text));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase("__this is \nplain text__")]
        [TestCase("__this is \tplain text__")]
        public void BuildTree_MarkdownWithTabAndNewLineBetweenDoubleUnderling_ShouldReturnTreeWithBoldNode(
            string markdown)
        {
            var text = markdown.Trim('_');
            var expected = new RootNode();
            var bold = new BoldNode();
            expected.AddNode(bold);
            bold.AddNode(new PlainTextNode(text));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_SingleAndDoubleUnderlingAlternate_ShouldReturnTreeWithItalicAndBoldNodes()
        {
            var markdown = "_italic_ __bold__";
            var text = markdown.Split('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            var bold = new BoldNode();
            expected.AddNode(italic);
            expected.AddNode(bold);
            expected.AddNode(new PlainTextNode(text[2]));
            italic.AddNode(new PlainTextNode(text[1]));
            bold.AddNode(new PlainTextNode(text[4]));

            var actual = treeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [TestCase("_4_2")]
        [TestCase("__4__2")]
        public void BuildTree_UnderlingBetweenDigits_ShouldReturnTreeWithOnlyPlainText(string markdown)
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