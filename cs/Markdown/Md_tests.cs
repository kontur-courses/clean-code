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
        public void Md_TextWithoutSpecialSigns_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }
        [TestCase("_hello")]
        [TestCase("hello_")]
        [TestCase("_  ")]
        [TestCase("hel_lo")]
        public void Md_TextWithOneUnderScoreNotClosed_ShouldNotBeChanged(string text)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(text);
        }

        [TestCase("_hello_", "<em>hello</em>")]
        [TestCase("aaaa _hello_ aaaa", "aaaa <em>hello</em> aaaa")]
        public void Md_TextWithOneUnderScoreClosed_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_hello _", "_hello _")]
        public void Md_TextWithOneUnderScoreClosedWihtSpaceBeforeClosing_ShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello__", "<strong>hello</strong>")]
        [TestCase("aaaa __hello__ aaaa", "aaaa <strong>hello</strong> aaaa")]
        public void Md_TextWithTwoUnderScoreClosed_ShouldBeInStrong(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__hello_", "__hello_")]
        [TestCase("_hello__", "_hello__")]
        [TestCase("aaaa _hello__ aaaa", "aaaa _hello__ aaaa")]
        public void Md_TextWithTwoDifferentUnderscores_ShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_123_", "_123_")]
        [TestCase("_12_3", "_12_3")]
        [TestCase("_aaa123aaa_", "_aaa123aaa_")]
        [TestCase("_12_a", "_12_a")]
        public void Md_UnderscoreInTextWithDigits_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa 1_23 aaa_", "<em>aaa 1_23 aaa</em>")]
        [TestCase("_aaa 123_ aaa_", "<em>aaa 123_ aaa</em>")]
        public void Md_UnderscoreInTextWithDigitsAdditionalTests_ShouldNotBeSpecialSymbol(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_ aaa_", "_ aaa_")]
        [TestCase("aaa_ aaa_", "aaa_ aaa_")]
        public void Md_OpenUnderscoreWithSpaceAfter_ShouldNotAddTag(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa _", "_aaa _")]
        public void Md_CloseUnderscoreWithSpaceAfter_ShouldNotAddTag(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase(@"\\y", @"\y")]
        [TestCase(@"\\", @"\")]
        [TestCase(@"yyy\\yyy", @"yyy\yyy")]
        public void Md_OneSlashWithoutSpecialSymbols_TextShouldNotChange(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase(@"\_", "_")]
        [TestCase(@"\_aaa", "_aaa")]
        public void Md_OneSlashWithOneUnderscore_SlashShouldNotBeInText(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase(@"\_aaa\_", "_aaa_")]
        public void Md_OneSlashWithTwoUnderscoreClosed_TextShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("__aaa_bbb_ccc__", "<strong>aaa<em>bbb</em>ccc</strong>")]
        public void Md_OneUnderscoreInTwoUnderscores_ShouldBeDoubleTagged(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa__bbb__ccc_", "<em>aaa__bbb__ccc</em>")]
        public void Md_TwoUnderscoresInOneUnderscore_ShouldNotBeDoubleTagged(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("_aaa__bbb__ccc", "_aaa<strong>bbb</strong>ccc")]
        public void Md_TwoUnderscoresAndSingleOne_ShouldBeInStrong(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa_bbb_aaa_", "<em>aaa</em>bbb<em>aaa</em>")]
        [TestCase("__aaa__bbb__aaa__", "<strong>aaa</strong>bbb<strong>aaa</strong>")]
        [TestCase("_aaa_ _aaa_", "<em>aaa</em> <em>aaa</em>")]
        public void Md_MultipleSinglePairUnderscores_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("_aaa__bbb_ccc__", "<em>aaa__bbb</em>ccc__")]
        public void Md_PartialOverlapping_ShouldNotBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("[test](hello)", "<a href=\"hello\">test</a>")]
        [TestCase("[te_st](hello)", "<a href=\"hello\">te_st</a>")]
        [TestCase("_aaa[te_st](hello)", "_aaa<a href=\"hello\">te_st</a>")]
        [TestCase("[test](he_ll_o)", "<a href=\"he_ll_o\">test</a>")]
        public void Md_LinkTagsTests_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("~~aaa~~", "<s>aaa</s>")]
        public void Md_TextInTildas_ShouldBeInTags(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("<em>test</em>", "<em>test</em>")]
        public void Md_TagsInDefaultText_ShouldNotChanges(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("________", "________")]
        [TestCase("___aaa___", "___aaa___")]
        public void Md_MoreThanTwoUnderscores_ShouldNotChangeText(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase(@"\\\\\\", @"\\\")]
        public void Md_backSlashWithBackSlash_ShouldShield(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("[test](hello", "[test](hello")]
        [TestCase("[test]hello)", "[test]hello)")]
        [TestCase("[test(hello)", "[test(hello)")]
        [TestCase("test](hello)", "test](hello)")]
        [TestCase("[test]   (hello)", "[test]   (hello)")]
        public void Md_WrongLinks_ShouldNotChanges(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

        [TestCase("[test](he(ll)o)", "<a href=\"he(ll\">test</a>o)")]
        [TestCase("[test](he(llo)", "<a href=\"he(llo\">test</a>")]
        [TestCase("[te[st](he(llo)", "[te<a href=\"he(llo\">st</a>")]
        [TestCase("[test](he_ll_o)", "<a href=\"he_ll_o\">test</a>")]
        [TestCase("[t_es_t](hello)", "<a href=\"hello\">t<em>es</em>t</a>")]
        public void Md_BorderCasesWithLinks_ShouldMakeATagWithLink(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }


        [TestCase("~~a_a_a~~", "<s>a<em>a</em>a</s>")]
        [TestCase("~~a__a__a~~", "<s>a<strong>a</strong>a</s>")]
        [TestCase("_a~~a~~a_", "<em>a<s>a</s>a</em>")]
        [TestCase("[t~~es~~t](hello)", "<a href=\"hello\">t<s>es</s>t</a>")]
        public void Md_TagS_CheckInnerInOtherTags_ShouldBeTagged(string text, string expectedText)
        {
            var renderer = new Md();
            var processedText = renderer.Render(text);
            processedText.Should().Be(expectedText);
        }

    }
}
