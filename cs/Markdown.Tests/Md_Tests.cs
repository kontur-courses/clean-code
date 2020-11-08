using System;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Md_Tests
    {
        [TestCase("_abc_", ExpectedResult = "<em>abc</em>", TestName = "WhenWordBetweenUnderlines")]
        [TestCase("_abc", ExpectedResult = "_abc", TestName = "WhenUnderlineOnlyBeforeWord")]
        [TestCase("abc_", ExpectedResult = "abc_", TestName = "WhenUnderlineOnlyAfterWord")]
        [TestCase("_ abc_", ExpectedResult = "_ abc_", TestName = "WhenSpaceAfterOpenedUnderline")]
        [TestCase("_abc _", ExpectedResult = "_abc _", TestName = "WhenSpaceBeforeClosedUnderline")]
        [TestCase("_abc_cdf_", ExpectedResult = "<em>abc</em>cdf_", TestName = "WhenOddCountOfUnderlines")]
        [TestCase("_abc_a_cdf_", ExpectedResult = "<em>abc</em>a<em>cdf</em>", TestName = "WhenEvenCountOfUnderlines")]
        [TestCase("_ _", ExpectedResult = "_ _", TestName = "WhenSpaceBetweenUnderlines")]
        [TestCase("_", ExpectedResult = "_", TestName = "WhenOnlyOneUnderline")]
        [TestCase("_a12a_", ExpectedResult = "_a12a_", TestName = "WhenDigitsBetweenUnderlines")]
        [TestCase("di_ff wo_rds", ExpectedResult = "di_ff wo_rds", TestName = "WhenUnderlinesInDifferentWords")]
        [TestCase("", ExpectedResult = "", TestName = "WhenEmptyInput")]
        [TestCase("  ", ExpectedResult = "  ", TestName = "WhenEmptySpacesInput")]
        public string Render_CorrectResult(string input)
        {
            return Md.Render(input);
        }

        [Test]
        public void Render_ThrowException_WhenNullInput()
        {
            Assert.Throws<NullReferenceException>(() => Md.Render(null));
        }
    }
}