using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    internal class HeadlineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BasicHeadline()
        {
            Markdown.Markdown.Render("#Text")
                .Should().Be(@"<h1>Text<\h1>");
        }

        [Test]
        public void Headline_With_Cursive_And_Bold()
        {
            Markdown.Markdown.Render("#Text _with_ __text__")
                .Should().Be(@"<h1>Text <em>with<\em> <strong>text<\strong><\h1>");
        }
    }
}