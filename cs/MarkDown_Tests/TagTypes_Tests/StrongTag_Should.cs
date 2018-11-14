using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class StrongTag_Should
    {
        private StrongTag tag;

        [SetUp]
        public void SetUp()
        {
            tag = new StrongTag();
        }

        [Test]
        public void HaveCorrectSpecialSymbol_AfterCreation()
        {
            tag.SpecialSymbol.Should().Be("__");
        }
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("strong");
        }

        [Test]
        public void IsInAvailableNestedTagTypes_BeFalse_OnStrongTag() =>
            tag.IsInAvailableNestedTagTypes(new StrongTag()).Should().BeFalse();

        [Test]
        public void IsInAvailableNestedTagTypes_BeTrue_OnEmTag() =>
            tag.IsInAvailableNestedTagTypes(new EmTag()).Should().BeTrue();


        [Test]
        public void ToHtml_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<strong>a</strong>");
    }
}
