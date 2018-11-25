using System;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Tests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Render_ReturnsEmptyString_OnEmptyInput()
        {
            md.Render("").Should().BeEmpty();
        }

        [Test]
        public void Render_Converts_SingleToken()
        {
            md.Render("_text_").Should().Be("<em>text</em>");
        }


        [Test]
        public void Render_Converts_SingleTokenBold()
        {
            md.Render("__text__").Should().Be("<strong>text</strong>");
        }

        [Test]
        public void Render_Converts_WhenTwoTokens()
        {
            md.Render("_tex_ _t_").Should().Be("<em>tex</em> <em>t</em>");
        }

        [Test]
        public void Render_Converts_TwoTokensBold()
        {
            md.Render("__tex__ __t__").Should().Be("<strong>tex</strong> <strong>t</strong>");
        }


        [Test]
        public void Render_Converts_SingleNonPairDelimiter()
        {
            md.Render("_tex _t_ _abc_").Should().Be("_tex <em>t</em> <em>abc</em>");
        }

        [Test]
        public void Render_Converts_SingleUnderscoreBetweenDouble()
        {
            md.Render("__a _text_ b__").Should().Be("<strong>a <em>text</em> b</strong>");
        }

        [Test]
        public void Render_Converts_DoubleUnderscoreBetweenSingle()
        {
            md.Render("_a __text__ b_").Should().Be("<em>a __text__ b</em>");
        }

        [Test]
        public void Render_Converts_When_SingleNonPairDelimiterBold()
        {
            md.Render("__tex __t__ __abc__").Should().Be("__tex <strong>t</strong> <strong>abc</strong>");
        }

        [Test]
        public void Render_Converts_SingleNonPairDelimiter2()
        {
            md.Render("tex _t_ _abc_ _t").Should().Be("tex <em>t</em> <em>abc</em> _t");
        }


        [Test]
        public void Render_Ignores_EscapedSymbols()
        {
            md.Render("\\_abc\\_").Should().Be("_abc_");
        }

        [Test]
        public void Render_Ignores_EscapedDoubleUnderscore()
        {
            md.Render("\\__abc\\__").Should().Be("__abc__");
        }


        [Test]
        public void Render_Ignores_DoubleEscape()
        {
            md.Render("\\\\_abc_").Should().Be("\\<em>abc</em>");
        }

        [Test]
        public void Render_DoesNotIgnore_LetterAfterEscape()
        {
            md.Render("\\abc").Should().Be("\\abc");
        }

        [Test]
        public void Render_ShouldIgnore_UnderscoreAfterDigit()
        {
            md.Render("a0_abc_").Should().Be("a0_abc_");
        }

        [Test]
        public void Render_Ignores_UnderscoreBeforeDigit()
        {
            md.Render("_abc_8a").Should().Be("_abc_8a");
        }

        [Test]
        public void Render_Ignores_DoubleUnderscoreAfterDigit()
        {
            md.Render("a0__abc__").Should().Be("a0__abc__");
        }

        [Test]
        public void Render_Ignores_DoubleUnderscoreBeforeDigit()
        {
            md.Render("__abc__8a").Should().Be("__abc__8a");
        }

        [Test]
        public void RenderConverts_NestingOfSameType()
        {
            md.Render("_a _abc_ a_").Should().Be("<em>a <em>abc</em> a</em>");
        }

        [Test]
        public void RenderConverts_With_SimpleHeaders1()
        {
            md.Render("#text\r\n").Should().Be("<h1>text</h1>\r\n");
        }

        [Test]
        public void RenderConverts_SimpleHeaders2()
        {
            md.Render("##text\r\n").Should().Be("<h2>text</h2>\r\n");
        }

        [Test]
        public void RenderConverts_SimpleHeaders3()
        {
            md.Render("###text\r\n").Should().Be("<h3>text</h3>\r\n");
        }

        [Test]
        public void RenderConverts_SimpleHeaders4()
        {
            md.Render("####text\r\n").Should().Be("<h4>text</h4>\r\n");
        }

        [Test]
        public void RenderConverts_SimpleHeaders5()
        {
            md.Render("#####text\r\n").Should().Be("<h5>text</h5>\r\n");
        }

        [Test]
        public void RenderConverts_SimpleHeaders6()
        {
            md.Render("######text\r\n").Should().Be("<h6>text</h6>\r\n");
        }

        [Test]
        public void RenderIgnoreHeaderAfterText()
        {
            md.Render("text#text\r\n").Should().Be("text#text\r\n");
        }

        [Test]
        public void RenderIgnoreHeaderWhenSingleLine()
        {
            md.Render("text#text").Should().Be("text#text");
        }

        [Test]
        public void RenderParseHeaderWhenTwoLines()
        {
            md.Render("#text\r\ntext").Should().Be("<h1>text</h1>\r\ntext");
        }
        [Test]
        public void RenderParseTwoHeaders()
        {
            md.Render("#text\r\n#text\r\n").Should().Be("<h1>text</h1>\r\n<h1>text</h1>\r\n");
        }
    }
}