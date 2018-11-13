using System;
using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class StringExtensionTest
    {
        [TestCase(@"\a", 1, ExpectedResult = true, TestName = "true when escaped")]
        [TestCase(@"\\a", 2, ExpectedResult = false, TestName = "false when escape char escaped")]
        [TestCase(@"\\\a", 3, ExpectedResult = true, TestName = "true when odd escape chars")]
        [TestCase(@"a", 0, ExpectedResult = false, TestName = "false when first char")]
        [TestCase(@"ab", 1, ExpectedResult = false, TestName = "false when not escaped")]
        public bool IsEscapedCharAt_ShouldReturn(string str, int position)
        {
            return str.IsEscapedCharAt(position);
        }
    }
}
