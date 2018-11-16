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

        [Test]
        public void Render_ShouldReturnEmptyString_WhenTextIsEmpty() =>
            md.Render("").Should().Be("");
    }
}
