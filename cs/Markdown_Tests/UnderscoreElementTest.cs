using Markdown.Analyzers;
using Markdown.Elements;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class UnderscoreElementTest
    {
        [TestCase("abc", 0, ExpectedResult = false, 
            TestName = "false when not indicator char at position")]
        [TestCase("_", 0, ExpectedResult = true, 
            TestName = "true when single indicator char")]
        [TestCase("__", 0, ExpectedResult = false, 
            TestName = "false when not single indicator char")]
        [TestCase(@"\_", 1, ExpectedResult = false,
            TestName = "false when indicator is escaped")]
        [TestCase(@"\__", 2, ExpectedResult = true,
            TestName = "true when underscore before is escaped")]
        [TestCase(@"1_a2", 1, ExpectedResult = false,
            TestName = "false when inside word with digits")]
        [TestCase(@"_a1", 0, ExpectedResult = false,
            TestName = "false when on the left side of word with digits")]
        [TestCase(@"2a_", 2, ExpectedResult = false,
            TestName = "false when on the right side of word with digits")]
        public bool SingleUnderscore_IsIndicatorAt_ShouldReturn(string markdown, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsIndicatorAt(SyntaxAnalyzer.AnalyzeSyntax(markdown), position);
        }

        [TestCase("abc", 0, ExpectedResult = false, 
            TestName = "false when not indicator char at position")]
        [TestCase("_", 0, ExpectedResult = false, 
            TestName = "false when string is shorter than indicator")]
        [TestCase("__", 0, ExpectedResult = true,
            TestName = "true when single indicator char")]
        [TestCase("___", 0, ExpectedResult = false, 
            TestName = "false when more underscores after")]
        [TestCase("___", 1, ExpectedResult = false, 
            TestName = "false when not zero underscores before")]
        [TestCase(@"\__", 1, ExpectedResult = false,
            TestName = "false when first underscore is escaped")]
        [TestCase(@"\___",  2, ExpectedResult = true,
            TestName = "true when underscore before is escaped")]
        public bool DoubleUnderscore_IsIndicatorAt_ShouldReturn(string markdown, int position)
        {
            return DoubleUnderscoreElementType.Create()
                .IsIndicatorAt(SyntaxAnalyzer.AnalyzeSyntax(markdown), position);
        }

        [TestCase("_a", 0, ExpectedResult = true,
            TestName = "true when valid open")]
        [TestCase("a_", 1, ExpectedResult = false,
            TestName = "false when no symbols after")]
        [TestCase("a_ ", 1, ExpectedResult = false,
            TestName = "false when whitespace after")]
        [TestCase(@"\_a", 1, ExpectedResult = false,
            TestName = "false when escaped")]
        public bool SingleUnderscore_IsOpeningOfElement_ShouldReturn(string markdown, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsOpeningOfElement(SyntaxAnalyzer.AnalyzeSyntax(markdown), position);
        }

        [TestCase("a_", 1, ExpectedResult = true,
            TestName = "true when valid close")]
        [TestCase("_a", 0, ExpectedResult = false,
            TestName = "false when no symbols before")]
        [TestCase(" _", 1, ExpectedResult = false,
            TestName = "false when whitespace before")]
        [TestCase(@"a\_", 2, ExpectedResult = false,
            TestName = "false when escaped")]
        public bool SingleUnderscore_IsClosingOfElement_ShouldReturn(string markdown, int position)
        {
            return SingleUnderscoreElementType.Create()
                .IsClosingOfElement(SyntaxAnalyzer.AnalyzeSyntax(markdown), position);
        }
    }
}
