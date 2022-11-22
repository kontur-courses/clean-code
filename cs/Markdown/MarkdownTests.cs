using FluentAssertions;
using NUnit.Framework;
using System;

namespace Markdown
{
    public class MarkdownTests
    {
        private static readonly MarkdownToHtmlParser parser = new MarkdownToHtmlParser();

        [Test]
        public void Shoud_ShieldUnderscores()
        {
            var markdownText = "word \\_italic\\_";
            var expectedHtmlText = "word _italic_";

            var actualHtmlText = parser.Render(markdownText);

            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [TestCase(null, TestName = "{m}_WhenInputIsNull")]
        [TestCase("", TestName = "{m}_WhenInputIsEmpty")]
        public void Should_ThrowArgumentException(string input)
        {
            var action = new Action(() => parser.Render(input));

            action.Should().Throw<ArgumentException>();
        }

        [TestCase("some really_123_3 awesome text", ExpectedResult = "some really_123_3 awesome text",
                                                    TestName = "{m}_WhenItIsInWordAroundNumber")]
        [TestCase("_test _", ExpectedResult = "_test _", TestName = "{m}_WhenThereIsASpaceBeforeClosingUnderscore")]
        [TestCase("_ test_", ExpectedResult = "_ test_", TestName = "{m}_WhenThereIsASpaceAfterOpeningUnderscore")]
        [TestCase("a_a a_", ExpectedResult = "a_a a_", TestName = "{m}_WhenThereIsATextBeforeOpeningUnderscore")]
        [TestCase("_a a_a", ExpectedResult = "_a a_a", TestName = "{m}_WhenThereIsATextAfterClosingUnderscore")]
        [TestCase("a_a a_a", ExpectedResult = "a_a a_a", TestName = "{m}_WhenThereIsASpaceBetweenUnderscores_AndTextAround")]
        public string Should_DisableHighlightning(string input)
        {
            return parser.Render(input);
        }

        [TestCase("_italic_", ExpectedResult = "<em>italic</em>", TestName = "{m}_ParseUnderscoresToTags")]
        [TestCase("x _x x_ x", ExpectedResult = "x <em>x x</em> x", TestName = "{m}_ParseItalic_WhenSpacesAround")]
        [TestCase("_x __x__ x_", ExpectedResult = "<em>x x x</em>", TestName = "{m}_DisableItalicInStrong_WhenStrongIsInItalic")]
        [TestCase("__x _x_ x__", ExpectedResult = "<strong>x <em>x</em> x</strong>", TestName = "{m}_EnableItalicInStrong_WhenItalicIsInStrong")]
        [TestCase("_exam_ple", ExpectedResult = "<em>exam</em>ple", TestName = "{m}_MakeItalicABeginningPartOfWord")]
        [TestCase("ex_am_ple", ExpectedResult = "ex<em>am</em>ple", TestName = "{m}_MakeItalicAMiddlePartOfWord")]
        [TestCase("exam_ple_", ExpectedResult = "<em>exam</em>ple", TestName = "{m}_MakeItalicATailPartOfWord")]
        public string Should(string input)
        {
            return parser.Render(input);
        }
    }
}