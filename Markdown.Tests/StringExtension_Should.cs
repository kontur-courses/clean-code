using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class StringExtension_Should
    {
        [Test]
        public void FindSubstringStartingWithPosition()
        {
            "defg".IsSubstring("abcdefgh", 3).Should().BeTrue();
            "deft".IsSubstring("abcdef", 3).Should().BeFalse();
        }

        [Test]
        public void CheckSymbolForShielding()
        {
            "ab\\cd".IsEscapedCharacter(3).Should().BeTrue();
            "a\\bcd".IsEscapedCharacter(1).Should().BeFalse();
        }

        [Test]
        public void CheckSymbolForIncorrectEndingShell()
        {
            "_a_ ".IsIncorrectEndingShell(3).Should().BeTrue();
            "_a_b".IsIncorrectEndingShell(3).Should().BeFalse();
        }

        [Test]
        public void CheckSubstringSurroundedByNumbers()
        {
            "abc1__2".IsSurroundedByNumbers(4, 5).Should().BeTrue();
            "abc2_r".IsSurroundedByNumbers(4, 4).Should().BeFalse();
        }
    }
}
