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
            Assert.Throws<ArgumentException>(
                () => md.Render(null),
                "text should not be null"
            );

        [Test]
        public void Render_ShouldReturnEmptyString_WhenTextIsEmpty() =>
            md.Render("").Should().Be("");
    }
}
