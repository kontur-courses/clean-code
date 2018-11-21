using System.Collections.Generic;
using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace MarkDown_Tests.TagTypes_Tests
{
    [TestFixture]
    public class EmTag_Should
    {
        private EmTag tag;
        private List<TagType> availableTagTypes;

        [SetUp]
        public void SetUp()
        {
            tag = new EmTag();
            availableTagTypes = new List<TagType>(){new ATag(), new EmTag(), new StrongTag()};
        }

        [Test]
        public void HaveCorrectOpeningSymbol_AfterCreation()
        {
            tag.OpeningSymbol.Should().Be("_");
        }        
        
        [Test]
        public void HaveCorrectClosingSymbol_AfterCreation()
        {
            tag.OpeningSymbol.Should().Be("_");
        }
        
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("em");
        }

        [Test]
        public void HaveCorrectParameter_AfterCreation()
        {
            tag.Parameter.Should().BeNull();
        }

        [Test]
        public void GetNestedTagTypes_Correctly() =>
            tag.GetNestedTagTypes(availableTagTypes).Should().BeEmpty();
        

        [Test]
        public void ToHtml_ReturnCorrectHtmlString() => tag.ToHtml("a").Should().Be("<em>a</em>");
    }
}
