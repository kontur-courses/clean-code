using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class ParagraphTag_Should
    {
        private ParagraphTag tag;
        private List<TagType> availableTagTypes;

        [SetUp]
        public void SetUp()
        {
            tag = new ParagraphTag();
            availableTagTypes = new List<TagType>(){new ATag(), new EmTag(), new StrongTag()};
        }

        [Test]
        public void HaveCorrectOpeningSymbol_AfterCreation()
        {
            tag.OpeningSymbol.Should().Be("\n");
        }
        
        [Test]
        public void HaveCorrectClosingSymbol_AfterCreation()
        {
            tag.ClosingSymbol.Should().Be("\n");
        }
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("p");
        }

        [Test]
        public void GetNestedTagTypes_Correctly() =>
            tag.GetNestedTagTypes(availableTagTypes).Select(s => s.GetType())
                .Should().Contain(new Type[]{typeof(EmTag), typeof(StrongTag), typeof(ATag)});


        [Test]
        public void RenderToHtml_And_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<p>a</p>");
    }
}
