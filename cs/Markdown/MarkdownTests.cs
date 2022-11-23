using FluentAssertions;
using NUnit.Framework;
using System;

namespace Markdown
{
    public class MarkdownTests
    {
        private static readonly MarkdownToHtmlParser parser = new MarkdownToHtmlParser();

        [TestCase(null, TestName = "{m}_WhenInputIsNull")]
        [TestCase("", TestName = "{m}_WhenInputIsEmpty")]
        public void Should_ThrowArgumentException(string input)
        {
            var action = new Action(() => parser.Render(input));

            action.Should().Throw<ArgumentException>();
        }


        
        [TestCase("a_a a_", ExpectedResult = "a_a a_", TestName = "{m}_WhenThereIsATextBeforeOpeningUnderscore")]
        [TestCase("_a a_a", ExpectedResult = "_a a_a", TestName = "{m}_WhenThereIsATextAfterClosingUnderscore")]
        [TestCase("a_a a_a", ExpectedResult = "a_a a_a", TestName = "{m}_WhenThereIsASpaceBetweenUnderscores_AndTextAround")]
        public string Should_DisableHighlightning(string input)
        {
            return parser.Render(input);
        }

        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "{m}_WhenThereIsAWordInsideItalic")]
        [TestCase("__text__", ExpectedResult = "<strong>text</strong>", TestName = "{m}_WhenThereIsAWordInsideStrong")]
        //[TestCase("x _x x_ x", ExpectedResult = "x <em>x x</em> x", TestName = "{m}_ParseItalic_WhenSpacesAround")]
        //[TestCase("_x __x__ x_", ExpectedResult = "<em>x x x</em>", TestName = "{m}_DisableItalicInStrong_WhenStrongIsInItalic")]
        //[TestCase("_exam_ple", ExpectedResult = "<em>exam</em>ple", TestName = "{m}_MakeItalicABeginningPartOfWord")]
        //[TestCase("ex_am_ple", ExpectedResult = "ex<em>am</em>ple", TestName = "{m}_MakeItalicAMiddlePartOfWord")]
        //[TestCase("exam_ple_", ExpectedResult = "exam<em>ple</em>", TestName = "{m}_MakeItalicATailPartOfWord")]
        public string Should_EnableHighlight(string input)
        {
            return parser.Render(input);
        }

        [TestCase("\\_some text\\_", ExpectedResult = "_some text_", TestName = "{m}_DisableItalicWithShielding")]
        [TestCase("te\\xt with\\ random \\slashes.\\", ExpectedResult = "te\\xt with\\ random \\slashes.\\",
                                                       TestName = "{m_NotShieldNonShieldableSymbols}")]
        [TestCase("\\\\_text_", ExpectedResult = "\\<em>text</em>", TestName = "{m}_Shield_ShieldingSymbol")]
        public string ShieldingShould(string input)
        {
            return parser.Render(input);
        }

        [TestCase("__x _x_ x__", ExpectedResult = "<strong>x <em>x</em> x</strong>", TestName = "{m}_EnableItalicInStrong_WhenItalicIsInStrong")]
        [TestCase("_x __x__ x_", ExpectedResult = "<em>x x x</em>", TestName = "{m}_DisableItalicInStrong_WhenStrongIsInItalic")]
        [TestCase("really_123_3 awesome text", ExpectedResult = "really_123_3 awesome text",
                                               TestName = "{m}_WhenItalicIsAroundNumber")]
        [TestCase("_exam_ple", ExpectedResult = "<em>exam</em>ple", TestName = "{m}_MakeItalicABeginningPartOfWord")]
        [TestCase("ex_am_ple", ExpectedResult = "ex<em>am</em>ple", TestName = "{m}_MakeItalicAMiddlePartOfWord")]
        [TestCase("exam_ple_", ExpectedResult = "exam<em>ple</em>", TestName = "{m}_MakeItalicATailPartOfWord")]
        [TestCase("te_xt te_xt", ExpectedResult = "te_xt te_xt", TestName = "{m}_DisableItalicWhenItStartsAndEndsInDifferentWords")]
        [TestCase("__NonPair_ tags", ExpectedResult = "__NonPair_ tags", TestName = "{m}_NoHighlightingInNonPairTags")]
        [TestCase("_test _", ExpectedResult = "_test _", TestName = "{m}_DisableItalic_WhenThereIsASpaceBeforeClosingUnderscore")]
        [TestCase("_ test_", ExpectedResult = "_ test_", TestName = "{m}_DisableItalic_WhenThereIsASpaceAfterOpeningUnderscore")]
        [TestCase("__text _text__ text_", ExpectedResult = "text text text", TestName = "{m}_DisableHighlighting_WhenItIntersects")]
        [TestCase("____", ExpectedResult = "____", TestName = "{m}_DisableHighlighting_WhenThereIsAEmptyStringInStrong")]
        public string Should(string input)
        {
            return parser.Render(input);
        }
    }
}