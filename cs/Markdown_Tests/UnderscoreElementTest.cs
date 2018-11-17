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
        public bool DoubleUnderscore_IsIndicatorAt_ShouldReturn(string markdown, bool[] escapeBitMask, int position)
        {
            return DoubleUnderscoreElementType.Create()
                .IsIndicatorAt(markdown, escapeBitMask, position);
        }


        [TestCase("_a", new []{false, false}, 0, ExpectedResult = true,
            TestName = "true when valid open")]
        [TestCase("a_", new[] { false, false }, 1, ExpectedResult = false,
            TestName = "false when no symbols after")]
        [TestCase("a_ ", new[] { false, false, false }, 1, ExpectedResult = false,
            TestName = "false when whitespace after")]
        [TestCase(@"\_a", new[] { false, true, false }, 1, ExpectedResult = false,
            TestName = "false when escaped")]
        public bool SingleUnderscore_IsOpeningOfElement_ShouldReturn(string markdown, bool[] escapeBitMask, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsOpeningOfElement(markdown, escapeBitMask, position);
        }

        [TestCase("a_", new[] { false, false }, 1, ExpectedResult = true,
            TestName = "true when valid close")]
        [TestCase("_a", new[] { false, false }, 0, ExpectedResult = false,
            TestName = "false when no symbols before")]
        [TestCase(" _", new[] { false, false }, 1, ExpectedResult = false,
            TestName = "false when whitespace before")]
        [TestCase(@"a\_", new[] { false, false, true }, 2, ExpectedResult = false,
            TestName = "false when escaped")]
        public bool SingleUnderscore_IsClosingOfElement_ShouldReturn(string markdown, bool[] escapeBitMask, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsClosingOfElement(markdown, escapeBitMask, position);
        }

    }
}
