using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            ExpectedResult = "Внутри <em>одинарного выделения __двойное__ не</em> работает",
            TestName = "WhenBoldBetweenItalic")]
        [TestCase("__Непарные_ символы",
            ExpectedResult = "__Непарные_ символы",
            TestName = "WhenNotPairedBoldAndItalic")]

        [TestCase(@"\abc\", ExpectedResult = @"\abc\", TestName = "WhenNotShieldedSlash")]
        [TestCase(@"\\\\abc", ExpectedResult = @"\\abc", TestName = "WhenSlashShielded")]
        [TestCase(@"\_abc_", ExpectedResult = @"_abc_", TestName = "WhenItalicShielded")]
        [TestCase(@"\__abc__", ExpectedResult = @"__abc__", TestName = "WhenBoldShielded")]
        [TestCase(@"\\_abc_", ExpectedResult = @"\<em>abc</em>", TestName = "WhenSlashShieldedBeforeItalic")]
        [TestCase(@"\\__abc__", ExpectedResult = @"\<strong>abc</strong>", TestName = "WhenSlashShieldedBeforeBold")]
        [TestCase(@"\\\__abc__", ExpectedResult = @"\__abc__", TestName = "WhenOddCountOfSlashesBeforeBold")]
        [TestCase(@"\\\\_\\abc\\_", ExpectedResult = @"\\<em>\abc\</em>", TestName = "WhenManyShieldedSlashes")]
        [TestCase(@"\\_\abc\_", ExpectedResult = @"\_\abc_", TestName = "WhenClosedItalicShielded")]
        [TestCase(@"\\__\abc\__", ExpectedResult = @"\__\abc__", TestName = "WhenClosedBoldShielded")]

        [TestCase(@"# abc", ExpectedResult = @"<h1>abc</h1>", TestName = "WhenHeadingWithoutAnyTags")]
        [TestCase(@"#abc", ExpectedResult = @"#abc", TestName = "WhenNoSpaceAfterHeading")]
        [TestCase(@"\# abc", ExpectedResult = @"# abc", TestName = "WhenShieldedHeading")]

        [TestCase("# Заголовок __с _разными_ символами__",
            ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "WhenHeadingAndAllAnyTags")]
        [TestCase(@"# \\\\__\\ab_abc_c\\__",
            ExpectedResult = @"<h1>\\<strong>\ab<em>abc</em>c\</strong></h1>",
            TestName = "WhenHeadingAndAllAnyTagsAndSlashes")]
        [TestCase("# abc# abc# add #aaa",
            ExpectedResult = "<h1>abc# abc# add #aaa</h1>",
            TestName = "WhenMoreThanOneHeading")]
        [TestCase("a# abc", ExpectedResult = "a# abc", TestName = "WhenHeadingNotAtBegin")]
        [TestCase("_hello __world__", ExpectedResult = "_hello <strong>world</strong>", TestName = "WhenNotPairedItalicBeforePairedBold")]
        [TestCase("__hello _world_", ExpectedResult = "__hello <em>world</em>", TestName = "WhenNotPairedBoldBeforePairedItalic")]

        [TestCase("[isLink](https://vk.com/feed)", ExpectedResult = @"<a href=""https://vk.com/feed"">isLink</a>", TestName = "WhenOnlyLink")]
        [TestCase("[is Link](https://vk.com/feed)", ExpectedResult = @"<a href=""https://vk.com/feed"">is Link</a>", TestName = "WhenLinkWithSpaceInName")]
        [TestCase("[isLink] (https://vk.com/feed)", ExpectedResult = "[isLink] (https://vk.com/feed)", TestName = "WhenOnlyNotValidLink")]
        [TestCase("[isLink](https://vk .com/feed)", ExpectedResult = "[isLink](https://vk .com/feed)", TestName = "WhenLinkWithSpace")]
        [TestCase("# __abc__ _[isLink](https://vk.com/feed)_",
            ExpectedResult = @"<h1><strong>abc</strong> <em><a href=""https://vk.com/feed"">isLink</a></em></h1>", TestName = "WhenLinkWithOtherTags")]
        [TestCase("[_isLi_12](#https://vk.com/__feed__)",
            ExpectedResult = @"<a href=""#https://vk.com/__feed__"">_isLi_12</a>", TestName = "WhenOtherTagsInsideLink")]
        [TestCase("[is]Li12](https://[vk.com/feed)",
            ExpectedResult = "[is]Li12](https://[vk.com/feed)", TestName = "WhenAnyBracketsInsideLink")]
        [TestCase(@"\[isLink](https://vk.com/feed)",
            ExpectedResult = @"[isLink](https://vk.com/feed)", TestName = "WhenSquareBracketShieldedInLink")]
        [TestCase(@"[isLink\](https://vk.com/feed)",
            ExpectedResult = @"[isLink](https://vk.com/feed)", TestName = "WhenBackSquareBracketShieldedInLink")]
        [TestCase(@"[isLink]\(https://vk.com/feed)",
            ExpectedResult = @"[isLink](https://vk.com/feed)", TestName = "WhenRoundBracketShieldedInLink")]
        [TestCase(@"[isLink](https://vk.com/feed\)",
            ExpectedResult = @"[isLink](https://vk.com/feed)", TestName = "WhenBackRoundBracketShieldedInLink")]
        public string Render_CorrectResult(string input)
        {
            return Md.Render(input);
        }

        [Test]
        public void Render_ThrowException_WhenNullInput()
        {
            Assert.Throws<NullReferenceException>(() => Md.Render(null));
        }

        [Test]
        public void Render_ShouldLinearComplexity()
        {
            const int delta = 2;
            const string initialText = @"# __abc__ _[Link](https://vk.com/)_";

            var ticks = new List<double>();
            var timer = new Stopwatch();
            for (var i = 1; i <= 4096; i *= delta)
            {
                var currentText = string.Concat(Enumerable.Repeat(initialText, i));

                timer.Start();
                Md.Render(currentText);
                timer.Stop();
                ticks.Add(timer.ElapsedTicks);
                timer.Reset();
            }

            for (var i = 0; i < ticks.Count - 1; ++i)
                Assert.LessOrEqual(ticks[i + 1] / ticks[i], delta + delta);
        }
    }
}