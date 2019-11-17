using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdTagType_Tests
    {
        [TestCase("_asasv dvd", 0, true)]
        [TestCase("__asasv dvd", 0, false)]
        [TestCase("__asasv __dvd", 8, false)]
        [TestCase("__asasv__ _dvd", 10, true)]
        public void StrongTagType_IsOpenedTag_ReturnRightResult(string text, int position, bool expected)
        {
            EmTagType.IsOpenedTag(text, position).Should().Be(expected);
        }

        [TestCase("_asasv_ dvd", 6, true)]
        [TestCase("_asasv dvd", 0, false)]
        [TestCase("__asasv __dvd", 8, false)]
        [TestCase("__asasv_ __dvd", 7, true)]
        [TestCase("__as__ asv_ __dvd", 10, true)]
        public void StrongTagType_IsClosedTag_ReturnRightResult(string text, int position, bool expected)
        {
            EmTagType.IsClosedTag(text, position).Should().Be(expected);
        }
    }
}