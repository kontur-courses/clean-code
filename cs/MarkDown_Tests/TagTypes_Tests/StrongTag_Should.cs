using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class StrongTag_Should
    {
        private StrongTag tag;
        private List<TagType> availableTagTypes;

        [SetUp]
        public void SetUp()
        {
            tag = new StrongTag();
            availableTagTypes = new List<TagType>(){new ATag(), new EmTag(), new StrongTag()};
        }
        [Test]
        public void HaveCorrectOpeningSymbol_AfterCreation()
        {
            tag.OpeningSymbol.Should().Be("__");
        }
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("strong");
        }

        [Test]
        public void GetNestedTagTypes_Correctly() =>
            tag.GetNestedTagTypes(availableTagTypes).Select(s => s.GetType())
                .Should().Contain(new Type[]{typeof(EmTag)});


        [Test]
        public void RenderToHtml_And_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<strong>a</strong>");
    }
}
