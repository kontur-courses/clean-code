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
        [TestCase("____too much____", ExpectedResult = "<strong>__too much</strong>__", TestName = "UnknownAmountOfUnderscores_ReturnWithMostAppropriateFormatting")]
        [TestCase("_text", ExpectedResult = "_text", TestName = "PairlessSingleUnderscore_ReturnWithoutChanges")]
        [TestCase("__text", ExpectedResult = "__text", TestName = "PairlessDoubleUnderscore_ReturnWithoutChanges")]
        [TestCase("__plain _text", ExpectedResult = "__plain _text", TestName = "PairlessTags_ReturnWithoutChanges")]
        [TestCase("_ text_", ExpectedResult = "_ text_", TestName = "WhiteSpaceAfterOpeningTag_ReturnWithoutChanges")]
        [TestCase("_text _", ExpectedResult = "_text _", TestName = "WhiteSpaceBeforeClosingTag_ReturnWithoutChanges")]

        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "SingleUnderscores_ReplaceWithEmTag")]
        [TestCase("__bold__", ExpectedResult = "<strong>bold</strong>", TestName = "DoubleUnderscores_ReplaceWithStrongTag")]
        [TestCase("__strong_italic_strong__", ExpectedResult = "<strong>strong<em>italic</em>strong</strong>", TestName = "SingleUnderscoresInDouble_ReturnEmInStrong")]
        [TestCase("_italic __without changes__ italic_", ExpectedResult = "<em>italic _</em>without changes__ italic_")]
        [TestCase("_two __", ExpectedResult = "<em>two _</em>", TestName = "TwoDifferentTagsIntersectEachOther_ReplaceWithLeadingTag")]

        [TestCase("\\_simple text\\_", ExpectedResult = "_simple text_", TestName = "EscapeFirstUnderscore_PreventFormatingFirstTag")]
        [TestCase(@"\_save underscores\_", ExpectedResult = "_save underscores_", TestName = "EscapeBothUnderscores_PreventFormatingBothTags")]
        [TestCase("__pairless double_not italic text_", ExpectedResult = "_<em>pairless double</em>not italic text_", TestName = "PairlessTag_FormatByClosingTag")]

        [TestCase("__bold___italic_", ExpectedResult = "<strong>bold</strong><em>italic</em>")]
        [TestCase("__bold _italic_bold_italic_ bold__", ExpectedResult = "<strong>bold <em>italic</em>bold<em>italic</em> bold</strong>")]
        public String Render_ConvertMarkdownToHtmlCorrectly(String markdown)
        {
            return md.Render(markdown);
        }
    }
}
