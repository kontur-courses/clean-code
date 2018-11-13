using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [TestCase("without underscores", ExpectedResult = "without underscores", TestName = "PlainText_ReturnWithoutChanges")]
        [TestCase("____too much____", ExpectedResult = "____too much____", TestName = "UnknownAmountOfUnderscores_ReturnWithMostAppropriateFormatting")]
        [TestCase("_text", ExpectedResult = "_text", TestName = "PairlessSingleUnderscore_ReturnWithoutChanges")]
        [TestCase("__text", ExpectedResult = "__text", TestName = "PairlessDoubleUnderscore_ReturnWithoutChanges")]
        [TestCase("__plain_text", ExpectedResult = "__plain_text", TestName = "PairlessTags_ReturnWithoutChanges")]
        [TestCase("_ text_", ExpectedResult = "_ text_", TestName = "WhiteSpaceAfterOpeningTag_ReturnWithoutChanges")]
        [TestCase("_text _", ExpectedResult = "_text _", TestName = "WhiteSpaceBeforeClosingTag_ReturnWithoutChanges")]

        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "SingleUnderscores_ReplaceWithEmTag")]
        [TestCase("__bold__", ExpectedResult = "<strong>bold</strong>", TestName = "DoubleUnderscores_ReplaceWithStrongTag")]
        [TestCase("__strong _italic_ strong__", ExpectedResult = "<strong>strong <em>italic</em> strong</strong>", TestName = "SingleUnderscoresInDouble_ReturnEmInStrong")]

        [TestCase("\\_simple text\\_", ExpectedResult = "_simple text_", TestName = "EscapeFirstUnderscore_PreventFormatingFirstTag")]
        [TestCase(@"\_save underscores\_", ExpectedResult = "_save underscores_", TestName = "EscapeBothUnderscores_PreventFormatingBothTags")]
        [TestCase("__pairless double_italiс_", ExpectedResult = "__pairless double<em>italiс</em>", TestName = "PairlessTag_FormatByClosingTag")]

        [TestCase("_two__", ExpectedResult = "_two__", TestName = "TwoDifferentTagsIntersectEachOther_ReplaceWithLeadingTag")]
        [TestCase("__bold___italic_", ExpectedResult = "__bold__<em>italic</em>")]
        public String Render_ConvertMarkdownToHtmlCorrectly(String markdown)
        {
            return md.Render(markdown);
        }
    }
}
