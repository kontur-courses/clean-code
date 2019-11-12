using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;

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
        public void ShouldReturnSameString_IfArgumentNotContainsMdTags(string mdParagraph)
        {
            var result = sut.Render(mdParagraph);

            result.Should().Be(mdParagraph);
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
        [TestCase("__q2_5asd3_4__", ExpectedResult = "<strong>q2_5asd3_4</strong>")]
        public string ShouldIgnoreTag_IfItInsideDigits(string text) => sut.Render(text);

        [TestCase("__qwe_zxc", ExpectedResult = "__qwe_zxc")]
        [TestCase("_qwe__zxc", ExpectedResult = "<em>qwe</em>_zxc")]
        public string ShouldNotReplace_IfNotFoundPairForPairTag(string text) => sut.Render(text);

        [TestCase("_ asd_zxc_", ExpectedResult = "_ asd<em>zxc</em>")]
        [TestCase("__ asd__zxc__", ExpectedResult = "__ asd<strong>zxc</strong>")]
        public string ShouldIgnoreOpenTag_IfItNextSymbolIsSpace(string text) => sut.Render(text);

        [TestCase("_asd _qwe_", ExpectedResult = "<em>asd _qwe</em>")]
        [TestCase("__asd __qwe__", ExpectedResult = "<strong>asd __qwe</strong>")]
        public string ShouldIgnoreCloseTag_IfItPreviousSymbolIsSpace(string text) => sut.Render(text);

        [TestCase("_,_zxc_", ExpectedResult = "_<em>zxc</em>")]
        [TestCase("_asd_,zxc_", ExpectedResult = "<em>asd_zxc</em>")]
        [TestCase("__asd_,_,zxc__", ExpectedResult = "<strong>asd__zxc</strong>")]
        public string ShouldIgnoreTagWithEscapeSymbol(string text) => sut.Render(text);
        
        [TestCase("aa,sd", ExpectedResult = "aasd")]
        [TestCase("aa,,sd", ExpectedResult = "aa,sd")]
        public string ShouldEscapeAnySymbol(string text) => sut.Render(text);

        [TestCase("_,,zxc_", ExpectedResult = "<em>,zxc</em>")]
        [TestCase("__,,zxc__", ExpectedResult = "<strong>,zxc</strong>")]
        public string ShouldNotIgnoreTag_IfEscapeCharAfterTagIsEscaped(string text) => sut.Render(text);

        [TestCase("__asd_q__w__e_zxc__", ExpectedResult = "<strong>asd<em>q</em><em>w</em><em>e</em>zxc</strong>")]
        [TestCase("__asd_qwe__q_", ExpectedResult = "__asd<em>qwe</em><em>q</em>")]
        public string ShouldReturnExpectedResult(string text) => sut.Render(text);

        [Test]
        public void ShouldCorrectWorkWithRealText()
        {
            var text = "_edem_,, _edem_ v sosednee selooo na ___diskoteeekuu!!!___ _200_2_KmPH";
            var expectedResult = "<em>edem</em>, <em>edem</em> v sosednee selooo na <strong><em>diskoteeekuu!!!</em></strong> <em>200_2</em>KmPH";

            var result = sut.Render(text);

            result.Should().Be(expectedResult);
        }
    }
}