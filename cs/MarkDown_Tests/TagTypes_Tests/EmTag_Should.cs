using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class EmTag_Should
    {
        private EmTag tag;

        [SetUp]
        public void SetUp()
        {
            tag = new EmTag();
        }

        [Test]
        public void HaveCorrectSpecialSymbol_AfterCreation()
        {
            tag.SpecialSymbol.Should().Be("_");
        }
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("em");
        }

        [Test]
        public void IsInAvailableNestedTagTypes_BeFalse_OnStrongTag() =>
            tag.IsInAvailableNestedTagTypes(new StrongTag()).Should().BeFalse();

        [Test]
        public void IsInAvailableNestedTagTypes_BeFalse_OnEmTag() =>
            tag.IsInAvailableNestedTagTypes(new EmTag()).Should().BeFalse();


        [Test]
        public void ToHtml_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<em>a</em>");
    }
}
