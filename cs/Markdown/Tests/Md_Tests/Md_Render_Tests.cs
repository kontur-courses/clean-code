using System;
using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics;

namespace Markdown.Tests.Md_Tests
{
    class Md_Render_Tests
    {
        private Md sut;

        [SetUp]
        public void SetUp()
        {
            sut = new Md();
        }

        [Test]
        public void ShouldThrow_WhenArgumentIsNull()
        {
            Action act = () => sut.Render(null);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestCase("")]
        [TestCase("123")]
        [TestCase(" asd")]
        public void ShouldReturnSameString_IfArgumentNotContainsMdTags(string text)
        {
            var result = sut.Render(text);

            result.Should().Be(text);
        }

        [TestCase("_asd_", ExpectedResult = "<em>asd</em>")]
        [TestCase("asd_qwe_zxc", ExpectedResult = "asd<em>qwe</em>zxc")]
        [TestCase("asd_qwe_zxc_a_sd_qwe_", ExpectedResult = "asd<em>qwe</em>zxc<em>a</em>sd<em>qwe</em>")]
        public string ShouldSupport_SingleUnderlineTag(string text) => sut.Render(text);

        [TestCase("__asd__", ExpectedResult = "<strong>asd</strong>")]
        [TestCase("__q____w__", ExpectedResult = "<strong>q</strong><strong>w</strong>")]
        public string ShouldSupport_DoubleUnderlineTag(string text) => sut.Render(text);

        [TestCase("__asd_zxc_qwe__", ExpectedResult = "<strong>asd<em>zxc</em>qwe</strong>")]
        public string ShouldSupport_SingleUnderlineInsideDoubleUnderline(string text) => sut.Render(text);

        [TestCase("_asd__zxc__qwe_", ExpectedResult = "<em>asd</em><em>zxc</em><em>qwe</em>")]
        public string ShouldNotSupport_DoubleUnderlineInsideSingleUnderline(string text) => sut.Render(text);

        [TestCase("_q2_5e_", ExpectedResult = "<em>q2_5e</em>")]
        [TestCase("__q2_5asd3_4a__", ExpectedResult = "<strong>q2_5asd3_4a</strong>")]
        [TestCase("3_asd_2", ExpectedResult = "3_asd_2")]
        [TestCase("_3asd4_", ExpectedResult = "_3asd4_")]
        [TestCase("_3asd_5", ExpectedResult = "_3asd_5")]
        public string ShouldIgnoreTag_IfItIsNearWithDigit(string text) => sut.Render(text);

        [TestCase("__qwe_zxc", ExpectedResult = "__qwe_zxc")]
        [TestCase("_qwe__zxc", ExpectedResult = "<em>qwe</em>_zxc")]
        public string ShouldNotReplace_IfNotFoundPairForPairTag(string text) => sut.Render(text);

        [TestCase("_ asd_zxc_", ExpectedResult = "_ asd<em>zxc</em>")]
        [TestCase("__ asd__zxc__", ExpectedResult = "__ asd<strong>zxc</strong>")]
        public string ShouldIgnoreOpenTag_IfItNextSymbolIsSpace(string text) => sut.Render(text);

        [TestCase("_asd _qwe_", ExpectedResult = "<em>asd _qwe</em>")]
        [TestCase("__asd __qwe__", ExpectedResult = "<strong>asd __qwe</strong>")]
        public string ShouldIgnoreCloseTag_IfItPreviousSymbolIsSpace(string text) => sut.Render(text);

        [TestCase("\\>_zxc_", ExpectedResult = "><em>zxc</em>")]
        [TestCase("\\__zxc_", ExpectedResult = "_<em>zxc</em>")]
        [TestCase("_asd\\_zxc_", ExpectedResult = "<em>asd_zxc</em>")]
        [TestCase("__asd\\_\\_zxc__", ExpectedResult = "<strong>asd__zxc</strong>")]
        public string ShouldIgnoreTagWithEscapeSymbol(string text) => sut.Render(text);
        
        [TestCase("aa\\sd", ExpectedResult = "aasd")]
        [TestCase("aa\\\\sd", ExpectedResult = "aa\\sd")]
        public string ShouldEscapeAnySymbol(string text) => sut.Render(text);

        [TestCase("\\\\_zxc_", ExpectedResult = "\\<em>zxc</em>")]
        [TestCase("\\\\__zxc__", ExpectedResult = "\\<strong>zxc</strong>")]
        public string ShouldNotIgnoreTag_IfEscapeCharBeforeTagIsEscaped(string text) => sut.Render(text);

        [TestCase("__asd_q__w__e_zxc__", ExpectedResult = "<strong>asd<em>q</em><em>w</em><em>e</em>zxc</strong>")]
        [TestCase("__asd_qwe__q_", ExpectedResult = "__asd<em>qwe</em><em>q</em>")]
        public string ShouldReturnExpectedResult(string text) => sut.Render(text);

        [TestCase(">qwe\n", ExpectedResult = "<blockquote>qwe</blockquote>")]
        [TestCase(">qwe", ExpectedResult = "<blockquote>qwe</blockquote>")]
        [TestCase("qw>e\n>asd", ExpectedResult = "qw<blockquote>e</blockquote><blockquote>asd</blockquote>")]
        [TestCase(">q_w_e", ExpectedResult = "<blockquote>q<em>w</em>e</blockquote>")]
        [TestCase(">q__w__e", ExpectedResult = "<blockquote>q<strong>w</strong>e</blockquote>")]
        [TestCase(">asd>qwe>zxc", ExpectedResult = "<blockquote>asd<blockquote>qwe<blockquote>zxc</blockquote></blockquote></blockquote>")]
        [TestCase(">asd>zxc\n\nqwe", ExpectedResult = "<blockquote>asd<blockquote>zxc</blockquote></blockquote>qwe")]
        public string ShouldSupportBlockquoteTag(string text) => sut.Render(text);

        [Test]
        public void ShouldCorrectWorkWithRealText()
        {
            var text = "_edem_\\__edem_ v sosednee selooo na ___diskoteeekuu!!!___ _(200_2)_KmPH";
            var expectedResult = "<em>edem</em>_<em>edem</em> v sosednee selooo na <strong><em>diskoteeekuu!!!</em></strong> <em>(200_2)</em>KmPH";

            var result = sut.Render(text);

            result.Should().Be(expectedResult);
        }

        [TestCase(10, 20)]
        [TestCase(100, 20)]
        [TestCase(1000, 5)]
        public void ShouldBe_LinearPerformanceDependenceByInputText(int inputTextMultiplyCount, int testStartsCount)
        {
            var input = ">_asd_q__zxc__q\\zxcq\\__zxc_f\n";
            for (var i = 0; i < 4; i++)
                input += input;
            var inputWithMultiplyCount = "";
            for (var i = 0; i < inputTextMultiplyCount; i++)
                inputWithMultiplyCount += input;

            var inputPerformance = GetAverageMeasurePerformance(() => sut.Render(input), testStartsCount);
            var inputCountXPerformance = GetAverageMeasurePerformance(() => sut.Render(inputWithMultiplyCount), testStartsCount);

            inputCountXPerformance.Should().BeLessThan(2 * inputTextMultiplyCount * inputPerformance);
        }

        private double GetAverageMeasurePerformance(Action act, int testsCount)
        {
            var totalTime = 0.0;
            for (var i = 0; i < testsCount; i++)
                totalTime += MeasurePerformance(act).TotalMilliseconds;
            return totalTime / testsCount;
        }

        private TimeSpan MeasurePerformance(Action action)
        {
            var watch = new Stopwatch();
            GC.Collect();
            watch.Start();
            action();
            watch.Stop();
            return watch.Elapsed;
        }
    }
}