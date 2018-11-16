using System;
using System.Linq;
using FluentAssertions;
using Markdown.Data;
using Markdown.Data.TagsInfo;
using Markdown.TokenParser;
using Markdown.TreeBuilder;
using Markdown.TreeTranslator;
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
            var tags = new[]
            {
                new Tag(new ItalicTagInfo(), "em"),
                new Tag(new BoldTagInfo(), "strong")
            };

            var allTags = tags.Select(tag => tag.Info.OpeningTag).Concat(tags.Select(tag => tag.Info.ClosingTag));
            var tagsTranslations = tags.Select(tag => tag.ToTranslationInfo);
            var tagsInfo = tags.Select(tag => tag.Info);

            var parser = new MarkdownTokenParser(allTags);
            var tagTranslator = new MarkdownToHtmlTagTranslator(tagsTranslations);
            var treeTranslator = new MarkdownTokenTreeTranslator(tagTranslator);
            var treeBuilder = new MarkdownTokenTreeBuilder(tagsInfo);

            markdown = new Markdown.Markdown(parser, treeTranslator, treeBuilder);
        }

        [TestCase("\n", " ", TestName = "OnlyNewLine")]
        [TestCase("", "", TestName = "EmptyString")]
        [TestCase("a", "a", TestName = "OnlyText")]
        [TestCase(" ", " ", TestName = "OnlySpace")]
        [TestCase("a b", "a b", TestName = "TextWithSpace")]
        [TestCase("a\nb", "a b", TestName = "TextWithSpace")]
        [TestCase("\\\\", "\\", TestName = "EscapedEscapeSymbol")]
        [TestCase("_a_", "<em>a</em>", TestName = "ItalicTag")]
        [TestCase("__a__", "<strong>a</strong>", TestName = "BoldTag")]
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