using System;
using System.Linq;
using FluentAssertions;
using Markdown.Data;
using Markdown.Data.TagsInfo.Headings;
using Markdown.Data.TagsInfo.StandardTags;
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

        [TestCase("\n", "\n", TestName = "OnlyNewLine")]
        [TestCase("", "", TestName = "EmptyString")]
        [TestCase("a", "a", TestName = "OnlyText")]
        [TestCase(" ", " ", TestName = "OnlySpace")]
        [TestCase("a b", "a b", TestName = "TextWithSpace")]
        [TestCase("a\nb", "a\nb", TestName = "TextWithNewLine")]
        [TestCase("\\\\", "\\", TestName = "EscapedEscapeSymbol")]
        [TestCase("_a_", "<em>a</em>", TestName = "ItalicTag")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "BoldTag")]
        [TestCase("# a\n", "<h1>a</h1>", TestName = "Heading1WithText")]
        [TestCase("## a\n", "<h2>a</h2>", TestName = "Heading2WithText")]
        [TestCase("### a\n", "<h3>a</h3>", TestName = "Heading3WithText")]
        [TestCase("#### a\n", "<h4>a</h4>", TestName = "Heading4WithText")]
        [TestCase("##### a\n", "<h5>a</h5>", TestName = "Heading5WithText")]
        [TestCase("###### a\n", "<h6>a</h6>", TestName = "Heading6WithText")]
        [TestCase("\\_a_", "_a_", TestName = "ItalicTagWithEscapedOpening")]
        [TestCase("\\__a__", "__a__", TestName = "BoldTagWithEscapedOpening")]
        [TestCase("_a\\_", "_a_", TestName = "ItalicTagWithEscapedClosing")]
        [TestCase("__a\\__", "__a__", TestName = "BoldTagWithEscapedClosing")]
        [TestCase("a _b_", "a <em>b</em>", TestName = "TextAndItalicTag")]
        [TestCase("a __b__", "a <strong>b</strong>", TestName = "TextAndBoldTag")]
        [TestCase("a _b_ c", "a <em>b</em> c", TestName = "ItalicTagWithTextAround")]
        [TestCase("a __b__ c", "a <strong>b</strong> c", TestName = "BoldTagWithTextAround")]
        [TestCase("_a_ b", "<em>a</em> b", TestName = "ItalicTagAndText")]
        [TestCase("__a__ b", "<strong>a</strong> b", TestName = "BoldTagAndText")]
        [TestCase("a_b_ c", "a_b_ c", TestName = "ItalicTagOpeningInText")]
        [TestCase("a__b__ c", "a__b__ c", TestName = "BoldOpeningTagInText")]
        [TestCase("a _b_c", "a _b_c", TestName = "ItalicTagClosingInText")]
        [TestCase("a __b__c", "a __b__c", TestName = "BoldTagClosingInText")]
        [TestCase("a_b_c", "a_b_c", TestName = "ItalicTagInText")]
        [TestCase("a__b__c", "a__b__c", TestName = "BoldTagInText")]
        [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>", TestName = "ItalicTagInBoldTag")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>", TestName = "BoldTagInItalicTag")]
        [TestCase("_a _b_ c_", "<em>a _b</em> c_", TestName = "ItalicTagInItalicTag")]
        [TestCase("__a __b__ c__", "<strong>a __b</strong> c__", TestName = "BoldTagInBoldTag")]
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