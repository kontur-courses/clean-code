using System;
using System.Diagnostics;
using FluentAssertions;
using Markdown.Properties;
using NUnit.Framework;

namespace Markdown.Core.Tests
{
    [TestFixture]
    public class MarkdownProcessorTests
    {
        private MarkdownProcessor processor;

        [SetUp]
        public void SetUp()
        {
            processor = MarkdownProcessorFactory.CreateMarkdownToHtmlProcessor();
        }


        [Test]
        public void Render_ShouldReturnUnchanged_WhenTextDoesNotContainMarkdownSymbols()
        {
            var text = "sample text";

            var renderedToHtmlText = processor.Render(text);

            renderedToHtmlText.ShouldBeEquivalentTo(text);
        }

        [TestCase("_sample text_", TestName = "all text between underscores", ExpectedResult = "<em>sample text</em>")]
        [TestCase("sample _text_ is good", TestName = "underscores in the middle of text", ExpectedResult = "sample <em>text</em> is good")]
        [TestCase("i_love_tea_so_much", TestName = "multiple paired underscores", ExpectedResult = "i<em>love</em>tea<em>so</em>much")]
        public string Render_ShouldReturnCorrect_WhenPairedUnderscoresInText(string text) => processor.Render(text);

        [TestCase("_sample text", TestName = "unpaired single underscore in beginning", ExpectedResult = "_sample text")]
        [TestCase("sample_text", TestName = "unpaired single underscore in middle", ExpectedResult = "sample_text")]
        [TestCase("__sample text", TestName = "unpaired double underscore in beginning", ExpectedResult = "__sample text")]
        public string Render_ShouldReturnUnchanged_WhenUnpairedUnderscoresInText(string text) => processor.Render(text);

        [TestCase(@"\_this is\_", TestName = "two commented single underscores", ExpectedResult = "_this is_")]
        [TestCase(@"\_this is_", TestName = "only one commented single underscore in pair", ExpectedResult = "_this is_")]
        [TestCase(@"\__this is\__", TestName = "two commented double underscores", ExpectedResult = "__this is__")]
        [TestCase(@"\__this is__", TestName = "only one commented double underscore in pair", ExpectedResult = "__this is__")]
        public string Render_ShouldReturnUnchanged_WhenCommentedUnderscores(string text) => processor.Render(text);

        [TestCase(@"\\\a", TestName = "triple backslash before symbol", ExpectedResult = @"\\a")]
        [TestCase(@"\\a", TestName = "double backslash before symbol", ExpectedResult = "\\a")]
        [TestCase("\\a", TestName = "single backslash before symbol", ExpectedResult = "\\a")]
        public string Render_ShouldReturnCorrect_WhenCommentedCommonSymbols(string text) => processor.Render(text);

        [TestCase("_sample_text_", TestName = "three single underscores", ExpectedResult = "<em>sample</em>text_")]
        [TestCase("_sample_text_is_good_", TestName = "five single underscores", ExpectedResult = "<em>sample</em>text<em>is</em>good_")]
        [TestCase("__sample__text__", TestName = "three double underscores", ExpectedResult = "<strong>sample</strong>text__")]
        [TestCase("__sample__text__is__good__", TestName = "five double underscores",
            ExpectedResult = "<strong>sample</strong>text<strong>is</strong>good__")]
        public string Render_ShouldPairClosestUnderscores_WhenOddNumberOfUnderscores(string text) => processor.Render(text);

        [TestCase("_12_3", TestName = "first single underscore in beginning", ExpectedResult = "_12_3")]
        [TestCase("1_2_3a_bc_", TestName = "single underscores around digits and around letters", ExpectedResult = "1_2_3a<em>bc</em>")]
        [TestCase("__12__3", TestName = "first double underscore in beginning", ExpectedResult = "__12__3")]
        [TestCase("1__2__3a__bc__", TestName = "double underscores around digits and around letters",
            ExpectedResult = "1__2__3a<strong>bc</strong>")]
        public string Render_ShouldReturnUnchanged_WhenAllUnderscoreNeighborsAreDigits(string text) => processor.Render(text);

        [TestCase("sample_text_123", TestName = "single underscore before digit", ExpectedResult = "sample<em>text</em>123")]
        [TestCase("123_sample_text", TestName = "single underscore behind digit", ExpectedResult = "123<em>sample</em>text")]
        [TestCase("sample__text__123", TestName = "double underscore before digit", ExpectedResult = "sample<strong>text</strong>123")]
        [TestCase("123__sample__text", TestName = "double underscore behind digit", ExpectedResult = "123<strong>sample</strong>text")]
        public string Render_ShouldConvertUnderscore_WhenUnderscoreHasNonDigitNeighbor(string text) => processor.Render(text);

        [TestCase("sample_ text_", TestName = "one open single underscore before space", ExpectedResult = "sample_ text_")]
        [TestCase("_ sample_ text_", TestName = "two open single underscores before spaces", ExpectedResult = "_ sample_ text_")]
        [TestCase("sample_\ttext_", TestName = "open underscore before tabulation", ExpectedResult = "sample_\ttext_")]
        [TestCase("sample__ text__", TestName = "one open double underscore before space", ExpectedResult = "sample__ text__")]
        [TestCase("__ sample__ text__", TestName = "two open double underscores before spaces",
            ExpectedResult = "__ sample__ text__")]
        public string Render_ShouldIgnoreStartUnderscore_WhenSpaceAfterIt(string text) => processor.Render(text);

        [TestCase("sample_text _", TestName = "one end single underscore after space", ExpectedResult = "sample_text _")]
        [TestCase("sample_text\t_", TestName = "one underscore after tabulation", ExpectedResult = "sample_text\t_")]
        [TestCase("sample__text __", TestName = "one end double underscore after space", ExpectedResult = "sample__text __")]
        public string Render_ShouldIgnoreEndUnderscore_WhenSpaceBeforeIt(string text) => processor.Render(text);

        [TestCase("__sample text__", TestName = "all text between paired underscores", ExpectedResult = "<strong>sample text</strong>")]
        [TestCase("sample __text is__ good", TestName = "underscores in th middle of the text", ExpectedResult = "sample <strong>text is</strong> good")]
        [TestCase("__sample__ text __is__ good", TestName = "multiple pairs of underscores",
            ExpectedResult = "<strong>sample</strong> text <strong>is</strong> good")]
        public string Render_ShouldReturnCorrect_WhenPairedDoubleUnderscoresInText(string text) => processor.Render(text);

        [TestCase("__sample _text", TestName = "double underscore, then single", ExpectedResult = "__sample _text")]
        [TestCase("_sample __text", TestName = "single underscore, then double", ExpectedResult = "_sample __text")]
        public string Render_ShouldReturnUnchanged_WhenUnpairedUnderscores(string text) => processor.Render(text);

        [TestCase("__im _sample_ text__", TestName = "all text inside double underscores",
            ExpectedResult = "<strong>im <em>sample</em> text</strong>")]
        [TestCase("__im _sam_ple__ __t_ex_t__", TestName = "single underscores inside double underscores two times", 
            ExpectedResult = "<strong>im <em>sam</em>ple</strong> <strong>t<em>ex</em>t</strong>")]
        public string Render_ShouldConvertSingleUnderscores_WhenTheyAreInsideDoubleUnderscores(string text) => processor.Render(text);

        [TestCase("_im__sample__text_", TestName = "all text inside single underscores", ExpectedResult = "<em>im__sample__text</em>")]
        [TestCase("_im__sam__ple_ _t__e__xt_", TestName = "double underscores inside single underscores two times",
            ExpectedResult = "<em>im__sam__ple</em> <em>t__e__xt</em>")]
        public string Render_ShouldIgnoreSingleUnderscores_WhenTheyAreInsideDoubleUnderscores(string text) =>
            processor.Render(text);

        [TestCase("____", TestName = "four underscores", ExpectedResult = "")]
        [TestCase("________", TestName = "eight underscores", ExpectedResult = "")]
        public string Render_ShouldReturnEmptyString_WhenFourUnderscoresInARow(string text) => processor.Render(text);

        [TestCase("# Header\n", TestName = "first header", ExpectedResult = "<h1>Header</h1>")]
        [TestCase("## Header\n", TestName = "second header", ExpectedResult = "<h2>Header</h2>")]
        [TestCase("### Header\n", TestName = "third header", ExpectedResult = "<h3>Header</h3>")]
        [TestCase("#### Header\n", TestName = "fourth header", ExpectedResult = "<h4>Header</h4>")]
        [TestCase("##### Header\n", TestName = "fifth header", ExpectedResult = "<h5>Header</h5>")]
        [TestCase("###### Header\n", TestName = "sixth header", ExpectedResult = "<h6>Header</h6>")]
        [TestCase("sample text\n# Header\n", TestName = "first header after newline",
            ExpectedResult = "sample text\n<h1>Header</h1>")]
        public string Render_ShouldReturnCorrect_OnHeaders(string text) => processor.Render(text);

        [TestCase("#sample text\n", TestName = "no space between # and text", ExpectedResult = "#sample text\n")]
        [TestCase("i# sample text\n", TestName = "# is not first symbol", ExpectedResult = "i# sample text\n")]
        [TestCase("_# sample text\n_", TestName = "single underscore before #", ExpectedResult = "_# sample text\n_")]
        public string Render_ShouldReturnUnchanged_OnIncorrectHeaders(string text) => processor.Render(text);

        [TestCase("# _header_\n", TestName = "single underscores in header", ExpectedResult = "<h1><em>header</em></h1>")]
        [TestCase("# __header__\n", TestName = "double underscores in header",
            ExpectedResult = "<h1><strong>header</strong></h1>")]
        public string Render_ShouldReturnCorrect_WhenUnderscoresInsideHeader(string text) => processor.Render(text);

        [TestCase("\\# header\n", TestName = "escaped first header", ExpectedResult = "# header\n")]
        [TestCase("\\### header\n", TestName = "escaped third header", ExpectedResult = "### header\n")]
        public string Render_ShouldReturnCorrect_WhenCommentedHeader(string text) => processor.Render(text);

        [Test, Explicit]
        public void Render_ShouldRenderFast()
        {
            var averageFileData = Resources.MarkdownTestFileAverage;
            var bigFileData = Resources.MarkdownTestFileBig;

            var averageFileProcessingTime = MeasureRunTimeInMilliseconds(() => processor.Render(averageFileData));
            var bigFileProcessingTime = MeasureRunTimeInMilliseconds(() => processor.Render(bigFileData));

            (bigFileProcessingTime).Should()
                .BeLessThan(2 * (bigFileData.Length / averageFileData.Length) * averageFileProcessingTime);
        }

        private long MeasureRunTimeInMilliseconds(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }
}