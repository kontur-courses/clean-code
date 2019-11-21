using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    class Md_tests
    {

        [TestCase("hello")]
        [TestCase("")]
        [TestCase("aaa aaa")]
        [TestCase("     ")]
        [TestCase("  aaa   ")]
        [TestCase("1234567890")]
        [TestCase("aaa123aaa")]
        [TestCase("aaa 123 aaa")]
        public void TextWithoutSpecialSigns_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }
        [TestCase("_hello")]
        [TestCase("hello_")]
        [TestCase("_  ")]
        [TestCase("hel_lo")]
        public void TextWithOneUnderScoreNotClosed_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }

        [TestCase("_hello_", "<em>hello</em>")]
        [TestCase("aaaa _hello_ aaaa", "aaaa <em>hello</em> aaaa")]
        public void TextWithOneUnderScoreClosed_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_hello _", "_hello _")]
        public void TextWithOneUnderScoreClosedWihtSpaceBeforeClosing_ShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello__", "<strong>hello</strong>")]
        [TestCase("aaaa __hello__ aaaa", "aaaa <strong>hello</strong> aaaa")]
        public void TextWithTwoUnderScoreClosed_ShouldBeInStrong(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello_", "__hello_")]
        [TestCase("_hello__", "_hello__")]
        [TestCase("aaaa _hello__ aaaa", "aaaa _hello__ aaaa")]
        public void TextWithTwoDifferentUnderscores_ShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_123_", "_123_")]
        [TestCase("_12_3", "_12_3")]
        [TestCase("_aaa123aaa_", "_aaa123aaa_")]
        [TestCase("_12_a", "_12_a")]
        public void UnderscoreInTextWithDigits_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa 1_23 aaa_", "<em>aaa 1_23 aaa</em>")]
        [TestCase("_aaa 123_ aaa_", "<em>aaa 123_ aaa</em>")]
        public void UnderscoreInTextWithDigitsAdditionalTests_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_ aaa_", "_ aaa_")]
        [TestCase("aaa_ aaa_", "aaa_ aaa_")]
        public void OpenUnderscoreWithSpaceAfter_ShouldNotAddTag(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa _", "_aaa _")]
        public void CloseUnderscoreWithSpaceAfter_ShouldNotAddTag(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase(@"\\y", @"\y")]
        [TestCase(@"\\", @"\")]
        [TestCase(@"yyy\\yyy", @"yyy\yyy")]
        public void OneSlashWithoutSpecialSymbols_TextShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase(@"\_", "_")]
        [TestCase(@"\_aaa", "_aaa")]
        public void OneSlashWithOneUnderscore_SlashShouldNotBeInText(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase(@"\_aaa\_", "_aaa_")]
        public void OneSlashWithTwoUnderscoreClosed_TextShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__aaa_bbb_ccc__", "<strong>aaa<em>bbb</em>ccc</strong>")]
        public void OneUnderscoreInTwoUnderscores_ShouldBeDoubleTagged(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa__bbb__ccc_", "<em>aaa__bbb__ccc</em>")]
        public void TwoUnderscoresInOneUnderscore_ShouldNotBeDoubleTagged(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        /*
         * этот тест не проходится потому, что в стеке после анализа остаются
         * закрытые теги и надо их тоже добавить в выходной список
         */

        [TestCase("_aaa__bbb__ccc", "_aaa<strong>bbb</strong>ccc")]
        public void TwoUnderscoresAndSingleOne_ShouldBeInStrong(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa_bbb_aaa_", "<em>aaa</em>bbb<em>aaa</em>")]
        [TestCase("__aaa__bbb__aaa__", "<strong>aaa</strong>bbb<strong>aaa</strong>")]
        [TestCase("_aaa_ _aaa_", "<em>aaa</em> <em>aaa</em>")]
        public void MultipleSinglePairUnderscores_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa__bbb_ccc__", "<em>aaa__bbb</em>ccc__")]
        public void PartialOverlapping_ShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("[test](hello)", "<a href=hello>test</a>")]
        [TestCase("[te_st](hello)", "<a href=hello>te_st</a>")]
        [TestCase("_aaa[te_st](hello)", "_aaa<a href=hello>te_st</a>")]
        [TestCase("[test](he_ll_o)", "<a href=he_ll_o>test</a>")]
        public void LinkTagsTests_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("~~aaa~~", "<s>aaa</s>")]
        public void TextInTildas_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase(@"\\\\\\", @"\\\")]
        [TestCase("________", "________")]
        [TestCase("_ _", "_ _")]
        [TestCase(@"\_ _test_", "_ <em>test</em>")]
        [TestCase(@"\__test_", "_<em>test</em>")]
        [TestCase("<em>test</em>", "<em>test</em>")]
        public void TemporaryTestsWithoutCategory(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        /*здесь собраны тесты которые пока что не относятся ни к какой категории
         * некоторые тесты на ситуацию когда поведение парсера
         * в таких случаях пока что не определено правилами
         * некоторые тесты - граничные случаи которые надо обработать
         */


    }
}
