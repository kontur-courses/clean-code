using Markdown.Elements;
using NUnit.Framework;

namespace Markdown_Tests
{
    [TestFixture]
    class DoubleUnderscoreElementTest
    {
        [TestCase("abc", 0, ExpectedResult = false, TestName = "false when not indicator char at position")]
        [TestCase("_", 0, ExpectedResult = false, TestName = "false when strign is shorter than indicator")]
        [TestCase("__", 0, ExpectedResult = true, TestName = "true when single indicator char")]
        [TestCase("___", 0, ExpectedResult = false, TestName = "false when not single indicator char")]
        public bool IsIndicatorAt_ShouldReturn(string markdown, int position)
        {
            return DoubleUnderscoreElementType.Create().IsIndicatorAt(markdown, position);
        }

    }
}
