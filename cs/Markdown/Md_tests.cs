using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    class Md_tests
    {
        [TestCase("hello")]
        [TestCase("")]
        [TestCase("aaa aaa")]
        [TestCase("     ")]
        [TestCase("  aaa   ")]
        [TestCase("1234567890")]
        [TestCase("aaa123aaa")]
        [TestCase("aaa 123 aaa")]
        public void TextWithoutSpecialSigns_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }
        [TestCase("_hello")]
        [TestCase("hello_")]
        [TestCase("_  ")]
        [TestCase("hel_lo")]
        public void TextWithOneUnderScoreNotClosed_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }

        [TestCase("_hello_", "<em>hello</em>")]
        [TestCase("aaaa _hello_ aaaa", "aaaa <em>hello</em> aaaa")]
        public void TextWithOneUnderScoreClosed_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_hello _", "_hello _")]
        public void TextWithOneUnderScoreClosedWihtSpaceBeforeClosing_ShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello__", "<strong>hello</strong>")]
        [TestCase("aaaa __hello__ aaaa", "aaaa <strong>hello</strong> aaaa")]
        public void TextWithTwoUnderScoreClosed_ShouldBeInStrong(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello_", "__hello_")]
        [TestCase("_hello__", "_hello__")]
        [TestCase("aaaa _hello__ aaaa", "aaaa _hello__ aaaa")]
        public void TextWithTwoDifferentUnderscores_ShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_123_", "_123_")]
        [TestCase("_12_3", "_12_3")]
        [TestCase("_aaa123aaa_", "_aaa123aaa_")]
        [TestCase("_12_a", "_12_a")]
        public void UnderscoreInTextWithDigits_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa 1_23 aaa_", "<em>aaa 1_23 aaa</em>")]
        [TestCase("_aaa 123_ aaa_", "<em>aaa 123_ aaa</em>")]
        public void UnderscoreInTextWithDigitsAdditionalTests_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("/y", "/y")]
        [TestCase("/", "/")]
        [TestCase("yyy/yyy", "yyy/yyy")]
        public void OneSlashWithoutSpecialSymbols_TextShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("/_", "_")]
        [TestCase("/_aaa", "_aaa")]
        public void OneSlashWithOneUnderscore_SlashShouldNotBeInText(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("/_aaa/_", "_aaa_")]
        public void OneSlashWithTwoUnderscoreClosed_TextShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


    }
}
