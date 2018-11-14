using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class ParagraphTag_Should
    {
        private ParagraphTag tag;

        [SetUp]
        public void SetUp()
        {
            tag = new ParagraphTag();
        }

        [Test]
        public void HaveCorrectSpecialSymbol_AfterCreation()
        {
            tag.SpecialSymbol.Should().Be("\n");
        }
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("p");
        }

        [Test]
        public void IsInAvailableNestedTagTypes_BeTrue_OnStrongTag() =>
            tag.IsInAvailableNestedTagTypes(new StrongTag()).Should().BeTrue();

        [Test]
        public void IsInAvailableNestedTagTypes_BeTrue_OnEmTag() =>
            tag.IsInAvailableNestedTagTypes(new EmTag()).Should().BeTrue();


        [Test]
        public void ToHtml_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<p>a</p>");
    }
}
