using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownProcessorTests
    {
        private MarkdownProcessor processor;

        [SetUp]
        public void SetUp()
        {
            processor = MarkdownProcessorFactory.Create();
        }


        [Test]
        public void RenderToHtml_ShouldReturnUnchanged_WhenTextDoesNotContainMarkdownSymbols()
        {
            var text = "sample text";

            var renderedToHtmlText = processor.RenderToHtml(text);

            renderedToHtmlText.ShouldBeEquivalentTo(text);
        }

        [TestCase("_sample text_", TestName = "all text between underscores", ExpectedResult = "<em>sample text</em>")]
        [TestCase("sample _text_ is good", TestName = "underscores in the middle of text", ExpectedResult = "sample <em>text</em> is good")]
        [TestCase("i_love_tea_so_much", TestName = "multiple paired underscores", ExpectedResult = "i<em>love</em>tea<em>so</em>much")]
        public string RenderToHtml_ShouldReturnCorrect_WhenPairedUnderscoresInText(string text) => processor.RenderToHtml(text);

        [TestCase("_sample text", TestName = "unpaired underscore in beginning", ExpectedResult = "_sample text")]
        [TestCase("sample_text", TestName = "unpaired underscore in middle", ExpectedResult = "sample_text")]
        public string RenderToHtml_ShouldReturnUnchanged_WhenUnpairedUnderscoresInText(string text) => processor.RenderToHtml(text);

        [TestCase(@"\_this is\_", TestName = "two commented underscores", ExpectedResult = "_this is_")]
        [TestCase(@"\_this is_", TestName = "only one commented underscore in pair", ExpectedResult = "_this is_")]
        public string RenderToHtml_ShouldReturnUnchanged_WhenCommentedUnderscores(string text) => processor.RenderToHtml(text);

        [TestCase("_sample_text_", TestName = "three underscores", ExpectedResult = "<em>sample</em>text_")]
        [TestCase("_sample_text_is_good_", TestName = "five underscores", ExpectedResult = "<em>sample</em>text<em>is</em>good_")]
        public string RenderToHtml_ShouldPairClosestUnderscores_WhenOddNumberOfUnderscores(string text) => processor.RenderToHtml(text);

        [TestCase("_12_3", TestName = "first underscore in beginning", ExpectedResult = "_12_3")]
        [TestCase("1_2_3a_bc_", TestName = "underscores around digits and around letters", ExpectedResult = "1_2_3a<em>bc</em>")]
        public string RenderToHtml_ShouldReturnUnchanged_WhenAllUnderscoreNeighborsAreDigits(string text) => processor.RenderToHtml(text);

        [TestCase("sample_text_123", TestName = "underscore before digit", ExpectedResult = "sample<em>text</em>123")]
        [TestCase("123_sample_text", TestName = "underscore behind digit", ExpectedResult = "123<em>sample</em>text")]
        public string RenderToHtml_ShouldConvertUnderscore_WhenUnderscoreHasNonDigitNeighbor(string text) => processor.RenderToHtml(text);

        [TestCase("sample_ text_", TestName = "one open underscores before space", ExpectedResult = "sample_ text_")]
        [TestCase("_ sample_ text_", TestName = "two open underscores before spaces", ExpectedResult = "_ sample_ text_")]
        [TestCase("sample_\ttext_", TestName = "open underscore before tabulation", ExpectedResult = "sample_\ttext_")]
        public string RenderToHtml_ShouldIgnoreStartUnderscore_WhenSpaceAfterIt(string text) => processor.RenderToHtml(text);

        [TestCase("sample_text _", TestName = "one end underscore after space", ExpectedResult = "sample_text _")]
        [TestCase("sample_text\t_", TestName = "one underscore after tabulation", ExpectedResult = "sample_text\t_")]
        public string RenderToHtml_ShouldIgnoreEndUnderscore_WhenSpaceBeforeIt(string text) => processor.RenderToHtml(text);
    }
}