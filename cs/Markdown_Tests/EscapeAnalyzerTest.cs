using Markdown;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class EscapesAnalyzerTest
    {
        [TestCase(@"\a", 1, ExpectedResult = true, TestName = "true when escaped")]
        [TestCase(@"\\a", 2, ExpectedResult = false, TestName = "false when escape char escaped")]
        [TestCase(@"\\\a", 3, ExpectedResult = true, TestName = "true when odd escape chars")]
        [TestCase(@"a", 0, ExpectedResult = false, TestName = "false when first char")]
        [TestCase(@"ab", 1, ExpectedResult = false, TestName = "false when not escaped")]
        public bool GetBitMaskOfEscapedChars_ShouldSetBit(string str, int position)
        {
            return EscapesAnalyzer.GetBitMaskOfEscapedChars(str)[position];
        }

        [TestCase(@"ab", ExpectedResult = "ab", TestName = "leave string without slashes the same")]
        [TestCase(@"\a", ExpectedResult = @"\a", TestName = "not remove slash which escapes not given char")]
        [TestCase(@"\\", ExpectedResult = @"\", TestName = "remove slash which escapes slash")]
        [TestCase(@"\_", ExpectedResult = @"_", TestName = "remove slash which escapes underscore")]
        public string RemoveEscapeSlashes_Should(string str)
        {
            return EscapesAnalyzer.RemoveEscapeSlashes(str, new[] {'\\', '_'});
        }
    }
}
