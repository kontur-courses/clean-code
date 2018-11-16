using Markdown.Elements;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class UnderscoreElementTest
    {
        [TestCase("abc", new []{ false, false, false }, 0, 
            ExpectedResult = false, 
            TestName = "false when not indicator char at position")]
        [TestCase("_", new[] { false }, 0, 
            ExpectedResult = true, 
            TestName = "true when single indicator char")]
        [TestCase("__", new[] { false, false }, 0, 
            ExpectedResult = false, 
            TestName = "false when not single indicator char")]
        [TestCase(@"\_", new []{ false, true }, 1, 
            ExpectedResult = false,
            TestName = "false when indicator is escaped")]
        [TestCase(@"\__", new[] { false, true, false }, 2,
            ExpectedResult = true,
            TestName = "true when underscore before is escaped")]
        public bool SingleUnderscore_IsIndicatorAt_ShouldReturn(string markdown, bool[] escapeBitMask, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsIndicatorAt(markdown, escapeBitMask, position);
        }

        [TestCase("abc", new []{ false, false, false }, 0, 
            ExpectedResult = false, 
            TestName = "false when not indicator char at position")]
        [TestCase("_", new[] { false }, 0, 
            ExpectedResult = false, 
            TestName = "false when string is shorter than indicator")]
        [TestCase("__", new[] { false, false }, 0, 
            ExpectedResult = true,
            TestName = "true when single indicator char")]
        [TestCase("___", new[] { false, false, false }, 0, 
            ExpectedResult = false, 
            TestName = "false when more underscores after")]
        [TestCase("___", new[] { false, false, false }, 1, 
            ExpectedResult = false, 
            TestName = "false when not zero underscores before")]
        [TestCase(@"\__", new[] { false, true, false }, 1,
            ExpectedResult = false,
            TestName = "false when first underscore is escaped")]
        [TestCase(@"\___", new[] { false, true, false, false }, 2,
            ExpectedResult = true,
            TestName = "true when underscore before is escaped")]
        public bool IsIndicatorAt_ShouldReturn(string markdown, bool[] escapeBitMask, int position)
        {
            return DoubleUnderscoreElementType.Create()
                .IsIndicatorAt(markdown, escapeBitMask, position);
        }

    }
}
