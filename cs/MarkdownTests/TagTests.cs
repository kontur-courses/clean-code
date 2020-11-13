using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TagTests
    {
        [Test]
        public void IntersectWith_EndOfCallingTagStandEarlierThanEndOfSecondTag_ReturnTrue()
        {
            var line = "_ab __c_ d__";
            var tagThatCallsMethod = new ItalicTag(0, 7);
            var tagToMethod = new BoldTag(4, line.Length - 2);

            tagThatCallsMethod.IntersectWith(tagToMethod).Should().BeTrue();
        }

        [Test]
        public void IntersectWith_EndOfCallingTagStandFurtherThanEndOfSecondTag_ReturnTrue()
        {
            var line = "_ab __c_ d__";
            var tagThatCallsMethod = new BoldTag(4, line.Length - 2);
            var tagToMethod = new ItalicTag(0, 7);

            tagThatCallsMethod.IntersectWith(tagToMethod).Should().BeTrue();
        }

        [Test]
        public void Contains_WhenTagInside_ReturnTrue()
        {
            var line = "__ab _cd_ ef__";
            var externalTag = new BoldTag(0, line.Length - 2);
            var internalTag = new ItalicTag(5, 8);

            externalTag.Contains(internalTag).Should().BeTrue();
        }

        [Test]
        public void Contains_WhenTagOutside_ReturnFalse()
        {
            var line = "__ab__ _cd_";
            var firstTag = new BoldTag(0, 4);
            var secondTag = new ItalicTag(7, 10);

            firstTag.Contains(secondTag).Should().BeFalse();
        }
    }
}