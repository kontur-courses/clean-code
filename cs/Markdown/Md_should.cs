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
    }
}
