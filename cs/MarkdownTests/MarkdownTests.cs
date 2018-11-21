using System;
using FluentAssertions;
using Markdown.Data;
using Markdown.TokenParser;
using Markdown.TreeBuilder;
using Markdown.TreeTranslator;
using Markdown.TreeTranslator.NodeTranslator;
using Markdown.TreeTranslator.TagTranslator;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTests
    {
        private Markdown.Markdown markdown;

        [SetUp]
        public void SetUp()
        {
            var language = new MarkdownLanguage();

            var parser = new MarkdownTokenParser(language.GetAllTags);
            var tagTranslator = new MarkdownToHtmlTagTranslator(language.GetTranslations);
            var nodeTranslator = new MarkdownNodeTranslator(tagTranslator);
            var treeTranslator = new MarkdownTokenTreeTranslator(nodeTranslator);
            var treeBuilder = new MarkdownTokenTreeBuilder(language.GetTagsInfo);

            markdown = new Markdown.Markdown(parser, treeTranslator, treeBuilder);
        }

        [TestCase("\n", "\n", TestName = "Only New Line")]
        [TestCase("", "", TestName = "Empty String")]
        [TestCase("a", "a", TestName = "Only Text")]
        [TestCase(" ", " ", TestName = "Only Space")]
        [TestCase("a b", "a b", TestName = "Text With Space")]
        [TestCase("a  b", "a b", TestName = "Text With More Than One Space")]
        [TestCase("a\tb", "a b", TestName = "Tab In Text")]
        [TestCase("a\nb", "a\nb", TestName = "Text With New Line")]
        [TestCase("\\\\", "\\", TestName = "Escaped Escape Symbol")]
        [TestCase("_\\\\_", "<em>\\</em>", TestName = "Escaped Escape Symbol In Tag")]
        [TestCase("_a_", "<em>a</em>", TestName = "Italic Tag")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "Bold Tag")]
        [TestCase("# a\n", "<h1>a</h1>", TestName = "Heading1")]
        [TestCase("## a\n", "<h2>a</h2>", TestName = "Heading2")]
        [TestCase("### a\n", "<h3>a</h3>", TestName = "Heading3")]
        [TestCase("#### a\n", "<h4>a</h4>", TestName = "Heading4")]
        [TestCase("##### a\n", "<h5>a</h5>", TestName = "Heading5")]
        [TestCase("###### a\n", "<h6>a</h6>", TestName = "Heading6")]
        [TestCase("a _b_ __c__", "a <em>b</em> <strong>c</strong>", TestName = "Different Tags")]
        [TestCase("\\_a_", "_a_", TestName = "Italic Tag With Escaped Opening")]
        [TestCase("\\__a__", "__a__", TestName = "Bold Tag With Escaped Opening")]
        [TestCase("_a\\_", "_a_", TestName = "Italic Tag With Escaped Closing")]
        [TestCase("__a\\__", "__a__", TestName = "Bold Tag With Escaped Closing")]
        [TestCase("_\\__", "<em>_</em>", TestName = "Escaped Italic Tag In Italic Tag Opening")]
        [TestCase("a _b_", "a <em>b</em>", TestName = "Text And Italic Tag")]
        [TestCase("a __b__", "a <strong>b</strong>", TestName = "Text And Bold Tag")]
        [TestCase("a _b_ c", "a <em>b</em> c", TestName = "Italic Tag With Text Around")]
        [TestCase("a __b__ c", "a <strong>b</strong> c", TestName = "Bold Tag With Text Around")]
        [TestCase("_a_ b", "<em>a</em> b", TestName = "Italic Tag And Text")]
        [TestCase("__a__ b", "<strong>a</strong> b", TestName = "Bold Tag And Text")]
        [TestCase("a_b_ c", "a_b_ c", TestName = "Italic Tag Opening In Text")]
        [TestCase("a__b__ c", "a__b__ c", TestName = "Bold Opening Tag In Text")]
        [TestCase("a _b_c", "a _b_c", TestName = "Italic Tag Closing In Text")]
        [TestCase("a __b__c", "a __b__c", TestName = "Bold Tag Closing In Text")]
        [TestCase("a_b_c", "a_b_c", TestName = "Italic Tag In Text")]
        [TestCase("a__b__c", "a__b__c", TestName = "Bold Tag In Text")]
        [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>", TestName = "Italic Tag In Bold Tag")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>", TestName = "Bold Tag In Italic Tag")]
        [TestCase("_a _b_ c_", "<em>a _b</em> c_", TestName = "Italic Tag In Italic Tag")]
        [TestCase("__a __b__ c__", "<strong>a __b</strong> c__", TestName = "Bold Tag In Bold Tag")]
        public void TestRender(string inputString, string expectedResult)
        {
            var translation = markdown.Render(inputString);

            translation.Should().Be(expectedResult);
        }

        [Test]
        public void Render_ShouldThrowArgumentNullException_OnNullInput()
        {
            Action action = () => markdown.Render(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Render_ShouldThrowArgumentException_OnNotParagraphInput()
        {
            Action action = () => markdown.Render("a\n\nb");

            action.Should().Throw<ArgumentException>();
        }
    }
}