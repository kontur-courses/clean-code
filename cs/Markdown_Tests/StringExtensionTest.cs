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
    }
}
