using System;
using NUnit.Framework;
using Markdown;

namespace Markdown_Tests
{
    [TestFixture]
    class StringExtensionTest
    {
        [TestCase("ab", 0, ExpectedResult = true, TestName = "true when not whitespace")]
        [TestCase("a b", 1, ExpectedResult = false, TestName = "false when space char")]
        [TestCase("a\nb", 1, ExpectedResult = false, TestName = "false when newline char")]
        [TestCase("a", 1, ExpectedResult = false, TestName = "false when position more than string length")]
        [TestCase("a", -1, ExpectedResult = false, TestName = "false when position is negative")]
        public bool IsWhitespaceAt_ShouldReturn(string str, int position)
        {
            return str.IsNonWhitespaceAt(position);
        }

        [TestCase("ab", 0, ExpectedResult = false, TestName = "false when another char")]
        [TestCase("a_b", 1, ExpectedResult = true, TestName = "true when given char")]
        [TestCase("a", 1, ExpectedResult = false, TestName = "false when position more than string length")]
        [TestCase("a", -1, ExpectedResult = false, TestName = "false when position is negative")]
        public bool IsCharAt_ShouldReturn(string str, int position)
        {
            return str.IsCharAt(position, '_');
        }

        [TestCase("abcd", 0, ExpectedResult = 4, TestName = "length when all chars in suffix matches")]
        [TestCase("ab  ", 0, ExpectedResult = 2, TestName = "index of first not matching")]
        public int GetIndexOfFirstNotMatching_ShouldReturn(string str, int position)
        {
            return str.GetIndexOfFirstMatching(position, ch => Char.IsWhiteSpace(ch));
        }

        [TestCase("a b", 0, 2, ExpectedResult = true, TestName = "true when contains one matching symbol")]
        [TestCase("a  b", 0, 3, ExpectedResult = true, TestName = "true when contains several matching symbols")]
        [TestCase("ab", 0, 1, ExpectedResult = false, TestName = "false when contains no matching symbol")]
        public bool ContainsMatchingSymbolsBetween_ShouldReturn(string str, int leftPosition, int rightPosition)
        {
            return str.ContainsMatchingSymbolsBetween(leftPosition, rightPosition,
                ch => Char.IsWhiteSpace(ch));
        }
    }
}
