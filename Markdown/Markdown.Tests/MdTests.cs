using System.Diagnostics;
using System.Text;
using FluentAssertions;

namespace Markdown.Tests;

public class MdTests
{
    private readonly Md _md = new();
    
    [TestCase("_hello world!_", "<em>hello world!</em>", TestName = "Render_ShouldFormat_WhenSingleUnderscores")]
    [TestCase("_ hello world!_", "_ hello world!_", TestName = "Render_ShouldNotFormat_WhenSpaceAfterOpeningUnderscore")]
    [TestCase("_hello world! _", "_hello world! _", TestName = "Render_ShouldNotFormat_WhenSpaceBeforeClosingUnderscore")]
    [TestCase("_he_llo world!", "<em>he</em>llo world!", TestName = "Render_ShouldEmphasize_WhenSingleUnderscoreInsideWord")]
    [TestCase("_he_llo wo_rl_d!", "<em>he</em>llo wo<em>rl</em>d!", TestName = "Render_ShouldEmphasizeMultipleSegments_WhenMultipleSinglePairsInText")]
    [TestCase(" _hello world_", " <em>hello world</em>", TestName = "Render_ShouldItalics_WhenSpaceBeforeTag")]
    [TestCase("_hello world1_2", "_hello world1_2", TestName = "Render_ShouldIgnoreItalicsTag_WhenBetweenWordsWithDigit")]
    [TestCase("_hello world!_\nhello guys", "<em>hello world!</em>\nhello guys", TestName = "Render_ShouldHandleEmphasisAcrossLines_WhenFirstLineHasSinglePair")]
    [TestCase("hello world!\n_hello guys_", "hello world!\n<em>hello guys</em>", TestName = "Render_ShouldHandleEmphasisAcrossLines_WhenSecondLineHasSinglePair")]
    [TestCase("_hello world!\\_", "_hello world!_", TestName = "Render_ShouldIgnoreItalicsTag_WhenCloseTagIsEscaped")]
    [TestCase("\\_hello world!\\_", "_hello world!_", TestName = "Render_ShouldIgnoreItalicsTag_WhenAllTagsIsEscaped")]
    [TestCase("__", "__", TestName = "Render_ShouldIgnoreItalicsTags_WhenInnerPartIsEmpty")]
    [TestCase("he\\_llo_ world", "he_llo_ world", TestName = "Render_ShouldTreatEscapedSingleUnderscoreAsLiteral_WhenBackslashPresent")]
    [TestCase("_пересечения __одинарных_ и двойных__", "_пересечения __одинарных_ и двойных__", TestName = "Render_ShouldIgnoreTags_WhenItalicsAndBoldIntersects")]
    [TestCase("hello _world!\nhello_ guys", "hello _world!\nhello_ guys", TestName = "Render_ShouldNotEmphasize_WhenMarkersSpanNewline")]
    [TestCase("текст c цифрами_12_3", "текст c цифрами_12_3", TestName = "Render_ShouldNotEmphasize_WhenDigitsAdjacentToSingleMarkers")]
    [TestCase("выделение в ра_зных сл_овах", "выделение в ра_зных сл_овах", TestName = "Render_ShouldNotEmphasizeAcrossWords_WhenSinglePairsSplitWords")]
    [TestCase("_hell__o w__orld!_", "<em>hell__o w__orld!</em>", TestName = "Render_ShouldTreatDoubleUnderscoresAsText_WhenInsideEmphasis")]
    [TestCase("_Непарные__ символы", "_Непарные__ символы", TestName = "Render_ShouldNotFormat_WhenUnpairedDoubleInsideSingleSequence")]
    [TestCase("_нач_ало, и в сер_еди_на, и в кон_ец._", "<em>нач</em>ало, и в сер<em>еди</em>на, и в кон<em>ец.</em>", TestName = "Render_ShouldEmphasizeMultipleWords_WhenValidSinglePairsInSentence")]
    public void Render_ShouldBeAsExpected_WhenItalicsTagIsProvided(string markdown, string expectedHtml)
    {
        _md.Render(markdown).Should().Be(expectedHtml);
    }
    
    [TestCase("__hello world!__", "<strong>hello world!</strong>", TestName = "Render_ShouldReturnStrongTags_WhenDoubleUnderscores")]
    [TestCase("__ hello world!__", "__ hello world!__", TestName = "Render_ShouldNotFormat_WhenSpaceAfterOpeningDoubleUnderscore")]
    [TestCase("__hello world! __", "__hello world! __", TestName = "Render_ShouldNotFormat_WhenSpaceBeforeClosingDoubleUnderscore")]
    [TestCase("__he__llo world!", "<strong>he</strong>llo world!", TestName = "Render_ShouldStrong_WhenDoubleUnderscoreInsideWord")]
    [TestCase("__he__llo wo__rl__d!", "<strong>he</strong>llo wo<strong>rl</strong>d!", TestName = "Render_ShouldStrongMultipleSegments_WhenMultipleDoublePairsInText")]
    [TestCase("__hell_o_ world!__", "<strong>hell<em>o</em> world!</strong>", TestName = "Render_ShouldAllowEmInsideStrong_WhenMixedMarkers")]
    [TestCase("__Непарные_ символы", "__Непарные_ символы", TestName = "Render_ShouldNotFormat_WhenUnpairedSingleInsideDoubleSequence")]
    [TestCase("выделение в ра__зных сл__овах", "выделение в ра__зных сл__овах", TestName = "Render_ShouldNotStrongAcrossWords_WhenDoublePairsSplitWords")]
    [TestCase("__нач__ало, и в сер__еди__на, и в кон__ец.__", "<strong>нач</strong>ало, и в сер<strong>еди</strong>на, и в кон<strong>ец.</strong>", TestName = "Render_ShouldStrongMultipleWords_WhenValidDoublePairsInSentence")]
    [TestCase("__hello world!__\nhello guys", "<strong>hello world!</strong>\nhello guys", TestName = "Render_ShouldHandleStrongAcrossLines_WhenFirstLineHasDoublePair")]
    [TestCase("hello world!\n__hello guys__", "hello world!\n<strong>hello guys</strong>", TestName = "Render_ShouldHandleStrongAcrossLines_WhenSecondLineHasDoublePair")]
    [TestCase("текст c цифрами__12__3", "текст c цифрами__12__3", TestName = "Render_ShouldNotStrong_WhenDigitsAdjacentToDoubleMarkers")]
    [TestCase("текст c __цифрами12__3", "текст c <strong>цифрами12</strong>3", TestName = "Render_ShouldStrong_WhenAlnumContentIsProperlyBounded")]
    [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_", TestName = "Render_ShouldIgnoreTags_WhenBoldAndItalicsIntersects")]
    [TestCase("____", "____", TestName = "Render_ShouldIgnoreBoldTags_WhenInnerPartIsEmpty")]
    [TestCase("__hello world!\\__", "__hello world!__", TestName = "Render_ShouldIgnoreBoldTag_WhenCloseTagIsEscaped")]
    [TestCase("\\__hello world!\\__", "__hello world!__", TestName = "Render_ShouldIgnoreBoldTag_WhenAllTagsIsEscaped")]
    [TestCase("__he\\llo world!\\__", "__he\\llo world!__", TestName = "Render_ShouldNotEscape_WhenRandomSymbolIsEscaped")]
    [TestCase("__hello world1__2", "__hello world1__2", TestName = "Render_ShouldIgnoreBoldTag_WhenBetweenWordsWithDigit")]
    [TestCase(" __hello world__", " <strong>hello world</strong>", TestName = "Render_ShouldBold_WhenSpaceBeforeTag")]
    [TestCase("he\\__llo__ world", "he__llo__ world", TestName = "Render_ShouldTreatEscapedDoubleUnderscoreAsLiteral_WhenBackslashPresent")]
    [TestCase("he\\\\__llo__ world", "he\\<strong>llo</strong> world", TestName = "Render_ShouldHandleEscapedBackslash_BeforeDoubleUnderscoreStrong")]
    public void Render_ShouldBeAsExpected_WhenBoldTagIsProvided(string markdown, string expectedHtml)
    {
        _md.Render(markdown).Should().Be(expectedHtml);
    }
    
    [TestCase("# hello world!", "<h1>hello world!</h1>", TestName = "Render_ShouldRenderH1_WhenHashFollowedBySpace")]
    [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "Render_ShouldRenderH1WithInlineFormatting_WhenMixedMarkersInsideHeader")]
    [TestCase("#hello world", "#hello world", TestName = "Render_ShouldNotRenderHeader_WhenNoSpaceAfterHash")]
    [TestCase("# hello world\nhello guys", "<h1>hello world</h1>\nhello guys", TestName = "Render_ShouldRenderHeaderOnlyForLine_WhenNextLineIsPlainText")]
    [TestCase("\\# hello world", "# hello world", TestName = "Render_ShouldTreatEscapedHashAsLiteral_WhenBackslashPresent")]
    [TestCase("#+ hello world!", "#+ hello world!", TestName = "Render_ShouldTreatHashPlusAsText_WhenNotHeaderPattern")]
    public void Render_ShouldBeAsExpected_WhenHeaderTagIsProvided(string markdown, string expectedHtml)
    {
        _md.Render(markdown).Should().Be(expectedHtml);
    }
    
     [TestCase("\\* ", "hello world!", "* hello world!", TestName = "Render_ShouldTreatEscapedAsteriskAsLiteral_WhenBackslashPresent")]
    [TestCase("\\- ", "hello world!", "- hello world!", TestName = "Render_ShouldTreatEscapedDashAsLiteral_WhenBackslashPresent")]
    [TestCase("\\+ ", "hello world!", "+ hello world!", TestName = "Render_ShouldTreatEscapedPlusAsLiteral_WhenBackslashPresent")]
    [TestCase("- ", "hello world!", "<ul><li>hello world!</li></ul>", TestName = "Render_ShouldRenderUnorderedList_WhenDashBullet")]
    [TestCase("+ ", "hello world!", "<ul><li>hello world!</li></ul>", TestName = "Render_ShouldRenderUnorderedList_WhenPlusBullet")]
    [TestCase("* ", "hello world!\n\n* hello guys!", "<ul><li>hello world!</li></ul>\n\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldEndList_WhenBlankLineBetweenAsteriskItems")]
    [TestCase("* ", "hello world!", "<ul><li>hello world!</li></ul>", TestName = "Render_ShouldRenderUnorderedList_WhenAsteriskBullet")]
    [TestCase("* ", "hello world!\n* hello guys!", "<ul><li>hello world!</li>\n<li>hello guys!</li></ul>", TestName = "Render_ShouldGroupConsecutiveItems_WhenSameBulletType")]
    [TestCase("* ", "hello world!\n- hello guys!", "<ul><li>hello world!</li></ul>\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldSplitLists_WhenBulletTypeChanges_AsteriskToDash")]
    [TestCase("+ ", "hello world!\n* hello guys!", "<ul><li>hello world!</li></ul>\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldSplitLists_WhenBulletTypeChanges_PlusToAsterisk")]
    [TestCase("+ ", "hello world!\n- hello guys!", "<ul><li>hello world!</li></ul>\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldSplitLists_WhenBulletTypeChanges_PlusToDash")]
    [TestCase("+ ", "hello world!\n- hello guys!\n* hello all!", "<ul><li>hello world!</li></ul>\n<ul><li>hello guys!</li></ul>\n<ul><li>hello all!</li></ul>", TestName = "Render_ShouldCreateSeparateLists_WhenThreeDifferentBulletTypes")]
    [TestCase("- ", "hello world!\n\n- hello guys!", "<ul><li>hello world!</li></ul>\n\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldEndList_WhenBlankLineBetweenDashItems")]
    [TestCase("+ ", "hello world!\n\n+ hello guys!", "<ul><li>hello world!</li></ul>\n\n<ul><li>hello guys!</li></ul>", TestName = "Render_ShouldEndList_WhenBlankLineBetweenPlusItems")]
    [TestCase("*", "hello world!", "*hello world!", TestName = "Render_ShouldNotRenderList_WhenNoSpaceAfterAsterisk")]
    [TestCase("+", "hello world!", "+hello world!", TestName = "Render_ShouldNotRenderList_WhenNoSpaceAfterPlus")]
    [TestCase("-", "hello world!", "-hello world!", TestName = "Render_ShouldNotRenderList_WhenNoSpaceAfterDash")]
    [TestCase("+ ", "_hello world!_", "<ul><li><em>hello world!</em></li></ul>", TestName = "Render_ShouldFormatInlineInsideListItem_WhenEmphasisIsPresent")]
    [TestCase("+ ", "__hello world!__", "<ul><li><strong>hello world!</strong></li></ul>", TestName = "Render_ShouldFormatInlineInsideListItem_WhenStrongIsPresent")]
    public void Render_ShouldBeAsExpected_WhenMarkedListTagIsProvided(string mdTag, string markdown, string expectedHtml)
    {
        _md.Render($"{mdTag}{markdown}").Should().Be(expectedHtml);
    }
    
    [Test]
    public void Render_ShouldRunInLinearTime()
    {
        var smallSize = 1000;
        var largeSize = 100000;
        var errorFactor = 1.5f;

        var smallInput = GenerateMarkdown(smallSize);
        var largeInput = GenerateMarkdown(largeSize);

        var smallTime = MeasureExecutionTime(smallInput, 10);
        var largeTime = MeasureExecutionTime(largeInput, 10);

        Console.WriteLine(smallTime);
        Console.WriteLine(largeTime);
        
        var growth = (float)largeTime / smallTime;
        growth.Should().BeLessThan(largeSize / smallTime * errorFactor);
    }

    private string GenerateMarkdown(int size)
    {
        var md = new StringBuilder();

        for (var i = 0; i < size; i++)
        {
            md.Append("_hello world!_\n");
            md.Append("__hello world!__\n");
            md.Append("# hello world!\n");
            md.Append("\\\\hello world!\n");
            md.Append("* hello world!\n");
            md.Append("- hello world!\n");
            md.Append("+ hello world!\n");
        }

        return md.ToString();
    }

    private long MeasureExecutionTime(string md, int iterations)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        var watch = Stopwatch.StartNew();
        for (var i = 0; i < iterations; i++)
            _md.Render(md);
        watch.Stop();
        return watch.ElapsedMilliseconds / iterations;
    }
}