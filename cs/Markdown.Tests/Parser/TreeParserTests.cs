using FluentAssertions;
using Markdown.Parser;
using Markdown.Tree;
using NUnit.Framework;

namespace Markdown.Tests.Parser
{
    [TestFixture]
    public class TreeParserTests
    {
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

            var actual = TreeBuilder.ParseMarkdown(markdown);

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

            var actual = TreeBuilder.ParseMarkdown(markdown);

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


            var actual = TreeBuilder.ParseMarkdown(markdown);

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


            var actual = TreeBuilder.ParseMarkdown(markdown);

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


            var actual = TreeBuilder.ParseMarkdown(markdown);

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

            var actual = TreeBuilder.ParseMarkdown(markdown);

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

            var actual = TreeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_SingleAndDoubleUnderlingAlternate_ShouldReturnTreeWithItalicAndBoldNodes()
        {
            var markdown = "_italic___bold__";
            var text = markdown.Split('_');
            var expected = new RootNode();
            var italic = new ItalicNode();
            var bold = new BoldNode();
            expected.AddNode(italic);
            expected.AddNode(bold);
            italic.AddNode(new PlainTextNode(text[1]));
            bold.AddNode(new PlainTextNode(text[4]));

            var actual = TreeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }

        [Test]
        public void BuildTree_UnderlingContainsDigits_ShouldReturnTreeWithOnlyPlainText()
        {
            var markdown = "_12_ __5__";
            var underling = "_";
            var plain1 = "12";
            var space = " ";
            var plain2 = "5";
            var expected = new RootNode();
            expected.AddNode(new PlainTextNode(underling));
            expected.AddNode(new PlainTextNode(plain1));
            expected.AddNode(new PlainTextNode(underling));
            expected.AddNode(new PlainTextNode(space));
            expected.AddNode(new PlainTextNode(underling));
            expected.AddNode(new PlainTextNode(underling));
            expected.AddNode(new PlainTextNode(plain2));
            expected.AddNode(new PlainTextNode(underling));
            expected.AddNode(new PlainTextNode(underling));

            var actual = TreeBuilder.ParseMarkdown(markdown);

            CheckTree(expected, actual);
        }
    }
}