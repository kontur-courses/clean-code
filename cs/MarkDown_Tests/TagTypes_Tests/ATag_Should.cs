using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests.TagTypes_Tests
{
    public class ATag_Should
    {
        private ATag tag;
        private List<TagType> availableTagTypes;

        [SetUp]
        public void SetUp()
        {
            tag = new ATag();
            availableTagTypes = new List<TagType>(){new ATag(), new EmTag(), new StrongTag()};
        }

        [Test]
        public void HaveCorrectOpeningSymbol_AfterCreation()
        {
            tag.OpeningSymbol.Should().Be("(");
        }        
        
        [Test]
        public void HaveCorrectClosingSymbol_AfterCreation()
        {
            tag.ClosingSymbol.Should().Be(")");
        }
        
        [Test]
        public void HaveCorrectHtmlTag_AfterCreation()
        {
            tag.HtmlTag.Should().Be("a");
        }

        [Test]
        public void HaveCorrectParameter_AfterCreation()
        {
            tag.Parameter.Should().BeEquivalentTo(new Parameter("[", "]", "href"));
        }

        [Test]
        public void GetNestedTagTypes_Correctly() =>
            tag.GetNestedTagTypes(availableTagTypes).Select(s => s.GetType())
                .Should().Contain(new Type[]{typeof(EmTag), typeof(StrongTag)});
        

        [Test]
        public void ToHtml_ReturnCorrectHtmlString() => tag.ToHtml("a", "a").Should().Be(@"<a href=""a"">a</a>");
    }
}