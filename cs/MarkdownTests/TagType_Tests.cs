using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TagType_Tests
    {
        [TestCase("_first_ _ second__ __third __fourth _fifth_", 2)]
        [TestCase("_fi_ _rst_  sec_ond__ __thi _rd __fourth _fift_h_", 4)]
        public void EmTagType_IsOpenedTag_RightDefinesSingleOpenedTags(string text, int expectedCount)
        {
            var underlineCount = text.Where((t, i) => EmTagType.IsOpenedTag(text, i)).Count();
            underlineCount.Should().Be(expectedCount);
        }
        
        [TestCase("_first_ _ second__ __third __fourth _fifth_", 2)]
        [TestCase("_fi_ _rst_  sec_ond__ __thi _rd __fourth _fift_h_", 3)]
        public void EmTagType_IsClosedTag_RightDefinesSingleClosedTags(string text, int expectedCount)
        {
            var underlineCount = text.Where((t, i) => EmTagType.IsClosedTag(text, i)).Count();
            underlineCount.Should().Be(expectedCount);
        }

        [TestCase("_first_ _ second__ __third __fourth _fifth_", 2)]
        [TestCase("_fi_ _rst_  sec_ond__ __thi _rd __fourth __fift_h_", 3)]
        public void StrongTagType_IsOpenedTag_RightDefinesDoubleOpenedTags(string text, int expectedCount)
        {
            var underlineCount = text.Where((t, i) => StrongTagType.IsOpenedTag(text, i)).Count();
            underlineCount.Should().Be(expectedCount);
        }
        
        [TestCase("_first_ _ second__ __third __fourth _fifth_", 1)]
        [TestCase("_fi__ _rst_  sec_ond__ __thi _rd __fourth __fift_h__", 3)]
        public void StrongTagType_IsClosedTag_RightDefinesDoubleClosedTags(string text, int expectedCount)
        {
            var underlineCount = text.Where((t, i) => StrongTagType.IsClosedTag(text, i)).Count();
            underlineCount.Should().Be(expectedCount);
        }
    }
}