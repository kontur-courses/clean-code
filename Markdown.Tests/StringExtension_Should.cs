using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class StringExtension_Should
    {
        [TestCase("defg", "abcdefgh", 3, ExpectedResult = true)]
        [TestCase("deft", "abcdef", 3, ExpectedResult = false)]
        [TestCase("abc", "qweab", 3, ExpectedResult = false)]
        public bool FindSubstringStartingWithPosition(string substring, string text, int position)
        {
            return substring.IsSubstring(text, position);
        }

        [TestCase("ab\\cd", 3, ExpectedResult = true)]
        [TestCase("a\\bcd", 1, ExpectedResult = false)]
        [TestCase("abc", 0, ExpectedResult = false)]
        public bool CheckSymbolForShielding(string text, int position)
        {
            return text.IsEscapedCharacter(position);
        }

        [Test]
        public void CheckSymbolForIncorrectEndingShell()
        {
            "_a_ ".IsIncorrectEndingShell(3).Should().BeTrue();
            "_a_b".IsIncorrectEndingShell(3).Should().BeFalse();
        }

        [TestCase("abc1__2", 4, 5, ExpectedResult = true)]
        [TestCase("abc2_r", 4, 4, ExpectedResult = false)]
        [TestCase("9_-1", 1, 1, ExpectedResult = false)]
        public bool CheckSubstringSurroundedByNumbers(string text, int start, int end)
        {
            return text.IsSurroundedByNumbers(start, end);
        }

        [Test]
        public void RemoveEscapeСharacters()
        {
            "\\abc\\qwe\\_".RemoveEscapeСharacters().Should().Be("abcqwe_");
        }

        [Test]
        public void GetEndSubstringStartingFromPosition()
        {
            "123".GetPositionEndSubstring(10).Should().Be(12);
        }
    }
}
