using System;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Md_Tests
    {
        [TestCase("", ExpectedResult = "", TestName = "WhenEmptyInput")]
        [TestCase("  ", ExpectedResult = "  ", TestName = "WhenEmptySpacesInput")]
        [TestCase("_", ExpectedResult = "_", TestName = "WhenOnlyOneUnderline")]
        [TestCase("_ _", ExpectedResult = "_ _", TestName = "WhenSpaceBetweenUnderlines")]
        [TestCase("_abc_", ExpectedResult = "<em>abc</em>", TestName = "WhenWordBetweenUnderlines")]
        [TestCase("_abc", ExpectedResult = "_abc", TestName = "WhenUnderlineOnlyBeforeWord")]
        [TestCase("abc_", ExpectedResult = "abc_", TestName = "WhenUnderlineOnlyAfterWord")]
        [TestCase("_ abc_", ExpectedResult = "_ abc_", TestName = "WhenSpaceAfterOpenedUnderline")]
        [TestCase("_abc _", ExpectedResult = "_abc _", TestName = "WhenSpaceBeforeClosedUnderline")]
        [TestCase("_abc_cdf_", ExpectedResult = "<em>abc</em>cdf_", TestName = "WhenOddCountOfUnderlines")]
        [TestCase("_ab_a_cd_", ExpectedResult = "<em>ab</em>a<em>cd</em>", TestName = "WhenEvenCountOfUnderlines")]
        [TestCase("_a12a_", ExpectedResult = "<em>a12a</em>", TestName = "WhenDigitsAndLettersBetweenUnderlines")]
        [TestCase("_111_", ExpectedResult = "<em>111</em>", TestName = "WhenOnlyDigitsBetweenUnderlines")]
        [TestCase("1_1a1_2", ExpectedResult = "1_1a1_2", TestName = "WhenDigitsAlongUnderlines")]
        [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах", TestName = "WhenUnderlinesInDifferentWords")]
        [TestCase("_разных сл_овах", ExpectedResult = "_разных сл_овах", TestName = "WhenClosedUnderlineInOtherWord")]
        [TestCase("ра_зных словах_", ExpectedResult = "ра_зных словах_", TestName = "WhenOpenedUnderlineInOtherWord")]
        [TestCase("_окруженный с двух сторон_",
            ExpectedResult = "<em>окруженный с двух сторон</em>",
            TestName = "WhenMoreThanOneWordBetweenUnderlines")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._",
            ExpectedResult = "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>",
            TestName = "WhenUnderlinesInDifferentPartsOfWords")]
        [TestCase("_words1_2 word_", 
            ExpectedResult = "<em>words1_2 word</em>", 
            TestName = "WhenNotValidUnderlineBetweenValidUnderlines")]
        [TestCase("_words1_2 1_2word _ word_",
            ExpectedResult = "<em>words1_2 1_2word _ word</em>", 
            TestName = "WhenNotValidUnderlinesBetweenValidUnderlines")]
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