using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_should
    {
        private Md md;

        [SetUp]
        public void CreateMd() => md = new Md();

        [Test]
        public void Render_ShouldThrowArgumentException_WhenTextIsNull() =>
            Assert.Throws<ArgumentNullException>(
                () => md.Render(null),
                "text"
            );

        [TestCase("", TestName = "text is empty")]
        [TestCase("some text", TestName = "text not contains special symbols")]
        public void Render_ShouldReturnText_When(string text) =>
            md.Render(text).Should().Be(text);

        [Test]
        public void Render_ShouldMarkEmTag_WhenText_UnderlineWrapped() =>
            md.Render("_some text_").Should().Be("<em>some text</em>");

        [Test]
        public void Render_ShouldMarkStrongTag_WhenText_DoubleUnderlineWrapped() =>
            md.Render("__some text__").Should().Be("<strong>some text</strong>");

        [Test]
        public void Render_ShouldMarkEmInsideStrong() =>
            md.Render("__a_some_ text__")
              .Should().Be("<strong>a<em>some</em> text</strong>");
        
        [Test]
        public void Render_ShouldNotMarkStrongInsideEm() =>
            md.Render("_a__some__ text_")
                .Should().Be("<em>a__some__ text</em>");
        
        [TestCase("\\_text\\_", TestName = "contains escaped _")]
        [TestCase("\\_\\_text\\_\\_", TestName = "contains escaped __")]
        public void Render_ShouldNotMarkEscaped_WhenText(string text) =>
            md.Render(text)
                .Should().Be(text);
        
        [Test]
        public void Render_ShouldNotMarkTextWithDigits() =>
            md.Render("12_3_456")
                .Should().Be("12_3_456");
        
        [TestCase("_word__")]
        [TestCase("__word_")]
        public void Render_ShouldNotMarkUnpaired(string text) =>
            md.Render(text)
                .Should().Be(text);
        
        [TestCase("_ some_")]
        [TestCase("__ another__")]
        public void Render_ShouldNotMark_WhenSpaceAfterOpenedControl(string text) =>
            md.Render(text)
                .Should().Be(text);
    }
}
