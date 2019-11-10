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

        [TestCase("_hello_", "<em>hello<em>")]
        [TestCase("aaaa _hello_ aaaa", "aaaa <em>hello<em> aaaa")]
        public void TextWithOneUnderScoreClosed_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }
    }
}
