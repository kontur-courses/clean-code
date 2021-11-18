using System;
using FluentAssertions;
using MarkdownTask;
using NUnit.Framework;

namespace MarkdownTaskTests
{
    public class MdTests
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }

        [Test]
        public void Render_ShouldBeImplemented()
        {
            Action act = () => md.Render("");
            act.Should().NotThrow<NotImplementedException>();
        }

        [Test]
        public void Render_ShouldThrow_WhenTextIsNull()
        {
            string mdText = null;

            Action act = () => md.Render(mdText);

            act.Should().Throw<NullReferenceException>()
                .WithMessage("Text can't has null reference");
        }

        [TestCase("text", "text")]
        [TestCase("", "")]
        public void Render_ShouldReturnStringItself_WhenNoMarkup(string mdText, string expectedHtmlText)
        {
            var actualHtmlText = md.Render(mdText);

            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [TestCase("_text_", @"\<em>text\</em>")]
        [TestCase("__text__", @"\<strong>text\</strong>")]
        [TestCase("#text", @"\<h1>text\</h1>")]
        public void Render_ShouldReplace_SimpleTag(string mdText, string expectedHtmlText)
        {
            var actualHtmlText = md.Render(mdText);

            actualHtmlText.Should().Be(expectedHtmlText);
        }

        [TestCase(@"\_text\_", "_text_")]
        [TestCase(@"te\xt", @"te\xt")]
        [TestCase(@"\_te\xt\_", @"_te\xt_")]
        [TestCase(@"\\_text_", @"\\<em>text\</em>")]
        public void Render_ShouldCorrectlyIdentifyEscapeSymbol(string mdText, string expectedHtmlText)
        {
            var actualHtmlText = md.Render(mdText);

            actualHtmlText.Should().Be(expectedHtmlText);
        }
    }
}