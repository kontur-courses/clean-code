using System;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Md_Tests
    {
        [TestCase("", ExpectedResult = "", TestName = "WhenEmptyInput")]
        [TestCase("  ", ExpectedResult = "  ", TestName = "WhenEmptySpacesInput")]
        [TestCase("_", ExpectedResult = "_", TestName = "WhenNotPairedItalic")]
        [TestCase("_ _", ExpectedResult = "_ _", TestName = "WhenSpaceBetweenItalic")]
        [TestCase("_abc_", ExpectedResult = "<em>abc</em>", TestName = "WhenWordBetweenItalic")]
        [TestCase("_abc", ExpectedResult = "_abc", TestName = "WhenNotPairedItalicBeforeWord")]
        [TestCase("abc_", ExpectedResult = "abc_", TestName = "WhenNotPairedItalicAfterWord")]
        [TestCase("_ abc_", ExpectedResult = "_ abc_", TestName = "WhenSpaceAfterOpenedItalic")]
        [TestCase("_abc _", ExpectedResult = "_abc _", TestName = "WhenSpaceBeforeClosedItalic")]
        [TestCase("_abc_cdf_", ExpectedResult = "<em>abc</em>cdf_", TestName = "WhenOddCountOfItalic")]
        [TestCase("_ab_a_cd_", ExpectedResult = "<em>ab</em>a<em>cd</em>", TestName = "WhenEvenCountOfItalic")]
        [TestCase("_a12a_", ExpectedResult = "<em>a12a</em>", TestName = "WhenDigitsAndLettersBetweenItalic")]
        [TestCase("_111_", ExpectedResult = "<em>111</em>", TestName = "WhenOnlyDigitsBetweenItalic")]
        [TestCase("1_1a1_2", ExpectedResult = "1_1a1_2", TestName = "WhenDigitsAlongItalic")]
        [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах", TestName = "WhenItalicInDifferentWords")]
        [TestCase("_разных сл_овах", ExpectedResult = "_разных сл_овах", TestName = "WhenClosedItalicInOtherWord")]
        [TestCase("ра_зных словах_", ExpectedResult = "ра_зных словах_", TestName = "WhenOpenedItalicInOtherWord")]
        [TestCase("_окруженный с двух сторон_",
            ExpectedResult = "<em>окруженный с двух сторон</em>",
            TestName = "WhenMoreThanOneWordBetweenItalic")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._",
            ExpectedResult = "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>",
            TestName = "WhenItalicInDifferentPartsOfWords")]
        [TestCase("_words1_2 1_2word _ word_",
            ExpectedResult = "<em>words1_2 1_2word _ word</em>",
            TestName = "WhenNotValidItalicBetweenValidItalic")]
        [TestCase("__", ExpectedResult = "__", TestName = "WhenNotPairedBold")]
        [TestCase("____", ExpectedResult = "____", TestName = "WhenBoldWithoutString")]
        [TestCase("__ __", ExpectedResult = "__ __", TestName = "WhenSpaceBetweenBold")]
        [TestCase("__abc__", ExpectedResult = "<strong>abc</strong>", TestName = "WhenWordBetweenBold")]
        [TestCase("__abc", ExpectedResult = "__abc", TestName = "WhenNotPairedBoldBeforeWord")]
        [TestCase("abc__", ExpectedResult = "abc__", TestName = "WhenNotPairedBoldAfterWord")]
        [TestCase("__ abc__", ExpectedResult = "__ abc__", TestName = "WhenSpaceAfterOpenedBold")]
        [TestCase("__abc __", ExpectedResult = "__abc __", TestName = "WhenSpaceBeforeClosedBold")]
        [TestCase("1__1a1__2", ExpectedResult = "1__1a1__2", TestName = "WhenDigitsAlongBold")]
        [TestCase("__abc__cdf__",
            ExpectedResult = "<strong>abc</strong>cdf__",
            TestName = "WhenOddCountOfBold")]
        [TestCase("__a12a__",
            ExpectedResult = "<strong>a12a</strong>",
            TestName = "WhenDigitsAndLettersBetweenBold")]
        [TestCase("__111__",
            ExpectedResult = "<strong>111</strong>",
            TestName = "WhenOnlyDigitsBetweenBold")]
        [TestCase("ра__зных сл__овах",
            ExpectedResult = "ра__зных сл__овах",
            TestName = "WhenBoldInDifferentWords")]
        [TestCase("__разных сл__овах",
            ExpectedResult = "__разных сл__овах",
            TestName = "WhenClosedBoldInOtherWord")]
        [TestCase("ра__зных словах__",
            ExpectedResult = "ра__зных словах__",
            TestName = "WhenOpenedBoldInOtherWord")]
        [TestCase("__ab__aa__cd__",
            ExpectedResult = "<strong>ab</strong>aa<strong>cd</strong>",
            TestName = "WhenEvenCountOfBold")]
        [TestCase("__окруженный с двух сторон__",
            ExpectedResult = "<strong>окруженный с двух сторон</strong>",
            TestName = "WhenMoreThanOneWordBetweenBold")]
        [TestCase("__нач__але, и в сер__еди__не, и в кон__це.__",
            ExpectedResult = "<strong>нач</strong>але, и в сер<strong>еди</strong>не, и в кон<strong>це.</strong>",
            TestName = "WhenBoldInDifferentPartsOfWords")]
        [TestCase("__words1__2 1__2word __ word__",
            ExpectedResult = "<strong>words1__2 1__2word __ word</strong>",
            TestName = "WhenNotValidBoldBetweenValidBold")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает",
            ExpectedResult = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает",
            TestName = "WhenItalicBetweenBold")]
        [TestCase("__пересечения _двойных__ и одинарных_",
            ExpectedResult = "__пересечения _двойных__ и одинарных_",
            TestName = "WhenItalicAndBoldIntersected")]
        [TestCase("Внутри _одинарного выделения __двойное__ не_ работает",
            ExpectedResult = "Внутри _одинарного выделения __двойное__ не_ работает",
            TestName = "WhenBoldBetweenItalic")]
        [TestCase("__Непарные_ символы", 
            ExpectedResult = "__Непарные_ символы",
            TestName = "WhenNotPairedBoldAndItalic")]
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