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
        public void Render_ShouldReturnEmptyString_OnEmptyInput()
        {
            md.Render("").Should().BeEmpty();
        }

        [Test]
        public void Render_ShouldConvert_WhenSingleToken()
        {
            md.Render("_text_").Should().Be("<em>text</em>");
        }


        [Test]
        public void Render_ShouldConvert_WhenSingleTokenBold()
        {
            md.Render("__text__").Should().Be("<strong>text</strong>");
        }

        [Test]
        public void Render_ShouldConvert_WhenTwoTokens()
        {
            md.Render("_tex_ _t_").Should().Be("<em>tex</em> <em>t</em>");
        }

        [Test]
        public void Render_ShouldConvert_WhenTwoTokensBold()
        {
            md.Render("__tex__ __t__").Should().Be("<strong>tex</strong> <strong>t</strong>");
        }


        [Test]
        public void Render_ShouldConvert_When_SingleNonPairDelimiter()
        {
            md.Render("_tex _t_ _abc_").Should().Be("_tex <em>t</em> <em>abc</em>");
        }

        [Test]
        public void Render_ShouldConvert_When_SingleUnderscoreBetweenDouble()
        {
            md.Render("__a _text_ b__").Should().Be("<strong>a <em>text</em> b</strong>");
        }

        [Test]
        public void Render_ShouldConvert_When_DoubleUnderscoreBetweenSingle()
        {
            md.Render("_a __text__ b_").Should().Be("<em>a __text__ b</em>");
        }

        [Test]
        public void Render_ShouldConvert_When_SingleNonPairDelimiterBold()
        {
            md.Render("__tex __t__ __abc__").Should().Be("__tex <strong>t</strong> <strong>abc</strong>");
        }

        [Test]
        public void Render_ShouldConvert_When_SingleNonPairDelimiter2()
        {
            md.Render("tex _t_ _abc_ _t").Should().Be("tex <em>t</em> <em>abc</em> _t");
        }


        [Test]
        public void Render_ShouldIgnore_EscapeSymbols()
        {
            md.Render("\\_abc\\_").Should().Be("_abc_");
        }

        [Test]
        public void Render_ShouldIgnore_EscapedDoubleUnderscore()
        {
            md.Render("\\__abc\\__").Should().Be("__abc__");
        }


        [Test]
        public void Render_ShouldIgnore_DoubleEscape()
        {
            md.Render("\\\\_abc_").Should().Be("\\<em>abc</em>");
        }

        [Test]
        public void Render_ShouldNotIgnore_LetterAfterEscape()
        {
            md.Render("\\abc").Should().Be("\\abc");
        }

        [Test]
        public void Render_ShouldIgnore_UnderscoreAfterDigit()
        {
            md.Render("a0_abc_").Should().Be("a0_abc_");
        }

        [Test]
        public void Render_ShouldIgnore_UnderscoreBeforeDigit()
        {
            md.Render("_abc_8a").Should().Be("_abc_8a");
        }

        [Test]
        public void Render_ShouldIgnore_DoubleUnderscoreAfterDigit()
        {
            md.Render("a0__abc__").Should().Be("a0__abc__");
        }

        [Test]
        public void Render_ShouldIgnore_DoubleUnderscoreBeforeDigit()
        {
            md.Render("__abc__8a").Should().Be("__abc__8a");
        }

        [Test]
        public void RenderConvert_When_NestingOfSameType()
        {
            md.Render("_a _abc_ a_").Should().Be("<em>a <em>abc</em> a</em>");
        }
    }
}