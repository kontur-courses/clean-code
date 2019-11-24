using NUnit.Framework;
using FluentAssertions;
using System.Diagnostics;
using System.Text;

namespace Markdown
{
    class MdTests
    {
        private readonly Md md = new Md();

        [TestCase("hello _great_ world", "hello <em>great</em> world",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscores")]

        [TestCase("_hello_ great world", "<em>hello</em> great world",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscoresInStart")]

        [TestCase("hello great _world_", "hello great <em>world</em>",
            TestName = "ShouldCreateEmTag_IfTextSurroundedByUnderscoresInEnd")]

        [TestCase("hello _great world", "hello _great world",
            TestName = "ShouldNotCreateEmTag_IfMissingClosingUnderscore")]

        [TestCase("hello ____ world", "hello ____ world",
            TestName = "ShouldNotCreateTag_IfEmptyStringBetweenDoubleUnderscores")]

        [TestCase("hello __great__ world", "hello <strong>great</strong> world",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscores")]

        [TestCase("__hello__ great world", "<strong>hello</strong> great world",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscoresInStart")]

        [TestCase("hello great __world__", "hello great <strong>world</strong>",
            TestName = "ShouldCreateStrongTag_IfTextSurroundedByDoubleUnderscoresInEnd")]

        [TestCase("hello __great_ world", "hello __great_ world",
            TestName = "ShouldNotCreateStrongTag_IfMissingClosingDoubleUnderscores")]

        [TestCase("hello __a_great_a__ world", "hello <strong>a<em>great</em>a</strong> world",
            TestName = "ShouldAddEmInsideStrongTag_IfUnderscoresInsideDoubleUnderscores")]

        [TestCase("hello _a__great__a_ world", "hello <em>a__great__a</em> world",
            TestName = "ShouldNotAddStrongInsideEmTag_IfDoubleUnderscoresInsideUnderscores")]

        [TestCase("hello ___aa_great__ world", "hello <strong><em>aa</em>great</strong> world",
            TestName = "ShouldAddEmInsideStrongTag_IfUnderscoresInsideDoubleUnderscoresInStart")]

        [TestCase("hello ___great___ world", "hello <strong>_great</strong>_ world",
            TestName = "ShouldAddStrongTags_IfTripleUnderscores")]

        [TestCase("hello _great_ __beautiful__ __interesting__ _wonderful_ _lovely_ world",
            "hello <em>great</em> <strong>beautiful</strong> <strong>interesting</strong> <em>wonderful</em> <em>lovely</em> world",
            TestName = "ShouldAddCorrectTags_IfSeveralSurroundedParts")]

        [TestCase("hello _ great_ world", "hello _ great_ world",
            TestName = "ShouldNotCreateTag_IfSurroundedTextHaveSpaceInStart")]

        [TestCase("hello _great _ world", "hello _great _ world",
            TestName = "ShouldNotCreateTag_IfSurroundedTextHaveSpaceInEnd")]

        [TestCase("hello 1_2great_ world", "hello 1_2great_ world",
            TestName = "ShouldNotCreateTag_IfOpeningUnderscoreInsideNumber")]

        [TestCase("hello _great1_2 world", "hello _great1_2 world",
            TestName = "ShouldNotCreateTag_IfClosingUnderscoreInsideNumber")]

        [TestCase("hello 1__2great__ world", "hello 1__2great__ world",
            TestName = "ShouldNotCreateTag_IfOpeningDoubleUnderscoresInsideNumber")]

        [TestCase("hello __great1__2 world", "hello __great1__2 world",
            TestName = "ShouldNotCreateTag_IfClosingDoubleUnderscoresInsideNumber")]

        [TestCase(@"hello \_great\_ world", "hello _great_ world",
            TestName = "ShouldNotCreateTag_IfSlashBeforeUnderscores")]

        [TestCase(@"hello _gre\_at_ world", "hello <em>gre_at</em> world",
            TestName = "ShouldNotCloseTag_IfSlashBeforeUnderscores")]

        [TestCase(@"\hell\o great\\ \worl\d", @"hello great\ world",
            TestName = "ShouldRemoveSlashes_IfSlashesBeforeSymbols")]

        [TestCase(@"hello great world\", @"hello great world\",
            TestName = "ShouldNotRemoveSlash_IfSlashInTextEnd")]

        [TestCase("[Link](https://www.google.com/)", "<a href=\"https://www.google.com/\">Link</a>",
            TestName = "ShouldAddATag_IfSimpleLinkText")]

        [TestCase("_hello_ [Link](https://www.google.com/) __world__", "<em>hello</em> <a href=\"https://www.google.com/\">Link</a> <strong>world</strong>",
            TestName = "ShouldAddATag_IfLinkAndOtherTexts")]

        [TestCase("[Link](https://www.google.com/) [Link](https://yandex.ru/)",
            "<a href=\"https://www.google.com/\">Link</a> <a href=\"https://yandex.ru/\">Link</a>",
            TestName = "ShouldAddATags_IfTwoLinks")]

        [TestCase("[Link](https://www.google.com/)", "<a href=\"https://www.google.com/\">Link</a>",
            TestName = "ShouldAddCorrectTags_IfNotOnlyLink")]

        [TestCase("[_Link_](https://www.google.com/)", "<a href=\"https://www.google.com/\"><em>Link</em></a>",
            TestName = "ShouldAddATagWithEmTag_IfLinkTextInsideUnderscores")]

        [TestCase("[__Link__](https://www.google.com/)", "<a href=\"https://www.google.com/\"><strong>Link</strong></a>",
            TestName = "ShouldAddATagWithStrongTag_IfLinkTextInsideDoubleUnderscores")]

        [TestCase("[__Link__ _Link_](https://www.google.com/)", "<a href=\"https://www.google.com/\"><strong>Link</strong> <em>Link</em></a>",
            TestName = "ShouldAddATagWithEmAndStrongTags_IfLinkTextIsSeveralSurroundedParts")]

        [TestCase("[__L_in_k__](https://www.google.com/)", "<a href=\"https://www.google.com/\"><strong>L<em>in</em>k</strong></a>",
            TestName = "ShouldAddATagWithEmInsideStrongTag_IfLinkTextInsideUnderscoresInsideDoubleUnderscores")]

        public void Render(string mdText, string expectedHtml)
        {
            var actualHtml = md.Render(mdText);

            actualHtml.Should().Be(expectedHtml);
        }

        [TestCase("hello _great_ __beautiful__ __interesting__ _wonderful_ _lovely_ world",
            TestName = "ShouldWorkInLinearTime_IfSeveralSurroundedParts")]

        public void RenderPerformanceTest(string mdText)
        {
            var builder = new StringBuilder();
            var count = 10000;
            for (var i = 0; i < count; i++)
                builder.Append(mdText);
            var longText = builder.ToString();

            //Вроде слышал надо один раз запускать перед измерениями, чтобы точно было
            md.Render(mdText);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            md.Render(mdText);
            sw.Stop();
            var shortTextMeasure = sw.ElapsedTicks;
            sw.Restart();
            md.Render(longText);
            sw.Stop();
            var longTextMeasure = sw.ElapsedTicks;

            var actualResult = longTextMeasure / shortTextMeasure;
            actualResult.Should().BeLessThan(count);
        }
    }
}
