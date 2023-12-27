namespace MdTest20.TestData;

public static class MdTestData
{
    public static IEnumerable<TestCaseData> ItalicTagsTestsConstructor => new[]
    {
        new TestCaseData("_text_", "<em>text</em>").SetName("ShouldReturnItalicTags_WhenThereIsItalicTags"),
        new TestCaseData("_te_xt", "<em>te</em>xt").SetName("ShouldReturnItalicTags_WhenThereIsItalicTagsInText"),
        new TestCaseData("te_xt_", "te<em>xt</em>").SetName("ShouldReturnItalicTags_WhenThereIsItalicTagsOneTagInEnd"),
        new TestCaseData("te_x_t", "te<em>x</em>t").SetName("ShouldReturnItalicTags_WhenThereIsItalicTwoTagsInText"),
        new TestCaseData("__a_b_c__", "<strong>a<em>b</em>c</strong>").SetName("ShouldReturnItalicAndBoldTags_WhenThereIsItalicInBoldTags"),
        new TestCaseData("_1a_a1_", "_1a_a1_").SetName("ShouldReturnText_WhenThereIsItalicInDigitText"),
        new TestCaseData("_1a_", "_1a_").SetName("ShouldReturnText_WhenThereIsItalicInMixesText")
    };
    public static IEnumerable<TestCaseData> EscapeTagsTestsConstructor => new[]
    {
        new TestCaseData(@"_te\\_ xt_", "<em>te\\</em> xt_").SetName("ShouldReturnTextWithItalicTags_WhenThereIsDoubleEscapeInText"),
        new TestCaseData(@"_te\xt_", @"<em>te\xt</em>").SetName("ShouldReturnTextWithItalicTags_WhenThereIsEscapeInText"),
        new TestCaseData(@"\_te\xt_", @"_te\xt_").SetName("ShouldReturnText_WhenThereEscapeBlockedOpeningItalicTag"),
        new TestCaseData(@"_\te\xt_", @"<em>\te\xt</em>").SetName("ShouldReturnWithTags_WhenThereEscapeAfterTag")
    };
    public static IEnumerable<TestCaseData> BlobTagsTestsConstructor => new[]
    {
        new TestCaseData("__text__", "<strong>text</strong>").SetName("ShouldReturnTextWitBoldTags_WhenThereIsBoldTags"),
        new TestCaseData("__tex__t", "<strong>tex</strong>t").SetName("ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInStartText"),
        new TestCaseData("te__xt__", "te<strong>xt</strong>").SetName("ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInEndText"),
        new TestCaseData("te__x__t", "te<strong>x</strong>t").SetName("ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInText"),
        new TestCaseData("_a__b__c_", "<em>a__b__c</em>").SetName("ShouldReturnTextOnlyItalicTags_WhenThereIsBoldTagsInItalicTags"),
        new TestCaseData("__a __ b__", @"<strong>a __ b</strong>").SetName("ShouldReturnTextWitBoldTags_WhenThereIsBoldTag"),
        new TestCaseData("__11__", "__11__").SetName("ShouldReturnText_WhenThereIsItalicInDigitText")
    };
    public static IEnumerable<TestCaseData> PairedTagsTestsConstructor => new[]
    {
        new TestCaseData("a_a b_b", @"a_a b_b").SetName("ShouldReturnText_WhenThereIsTagsInDifferentWords"),
        new TestCaseData("____", "____").SetName("ShouldReturnText_WhenThereIsOnlyTags"),
        new TestCaseData("__ _a_ __", @"__ <em>a</em> __").SetName("ShouldReturnTextOnlyItalicTags_WhenThereIsBlobTagsWithSpacesAroundEdges"),
        new TestCaseData("a_ b_", @"a_ b_").SetName("ShouldReturnText_WhenThereIsInvalidOpeningTag"),
        new TestCaseData("_text__text_", "_text__text_").SetName("ShouldReturnText_WhenThereIsTagIntersection"),
        new TestCaseData("_a", "_a").SetName("ShouldReturnText_WhenThereIsOnlyOpeningTag"),
        new TestCaseData("__u _b_ _c_ u__", @"<strong>u <em>b</em> <em>c</em> u</strong>").SetName("ShouldReturnTextWithTags_WhenThereIsMultiplyItalicInBold"),
        new TestCaseData("a_", "a_").SetName("ShouldReturnText_WhenThereIsOnlyClosingTag"),
        new TestCaseData("text", "text").SetName("ShouldReturnText_WhenThereIsOnlyText"),
        new TestCaseData("text _", "text _").SetName("ShouldReturnText_WhenThereIsSpaceBeforeClosingTag"),
        new TestCaseData("_ text", "_ text").SetName("ShouldReturnText_WhenThereIsSpaceAfterOpeningTag"),
        new TestCaseData("____ text", "____ text").SetName("ShouldReturnText_WhenThereIsEmptyBetweenPairedTags"),
        new TestCaseData("", "").SetName("ShouldReturnEmpty_WhenThereIsEmptyText")
    };

    public static IEnumerable<TestCaseData> HeaderTagsTestsConstructor => new[]
    {
         new TestCaseData(@"# a", @"<h1>a</h1>").SetName("ShouldReturnTextWithHeaderTag_WhenThereIsHeaderTag"),
         new TestCaseData(@"#a", @"#a").SetName("ShouldReturnText_WhenThereIsIncorrectHeaderTag"),
         new TestCaseData(@"\# a", @"# a").SetName("ShouldReturnText_WhenThereIsEscapeTagBeforeHeaderTag"),
         new TestCaseData("# _te# ext_\n", "<h1><em>te# ext</em></h1>").SetName("ShouldReturnTextWithTags_WhenThereIsItalicTagsInHeaderTag"),
         new TestCaseData("# _text_\r\n", "<h1><em>text</em></h1>").SetName("ShouldReturnTextWithTags_WhenThereIsAnotherTypeLineBreaker"),
         new TestCaseData("# _text\r\n_hello_\r\n", "<h1>_text</h1>\n<em>hello</em>").SetName("ShouldReturnTextWithTags_WhenTransferringLineTagProcessingVeryBeginning"),
         new TestCaseData("## _text\r\n", "<h2>_text</h2>").SetName("ShouldReturnTextWithTags_WhenThereIsLevel2HeaderTag"),
         new TestCaseData("### _text\r\n", "<h3>_text</h3>").SetName("ShouldReturnTextWithTags_WhenThereIsLevel3HeaderTag"),
    };

    public static IEnumerable<TestCaseData> BulletedTagsTestsConstructor => new[]
    {
         new TestCaseData(@"\\* a", @"\* a").SetName("ShouldReturnTextWithBulletedTag_WhenThereIsBulletedTag"),
         new TestCaseData("* _text_\n", "<li><em>text</em></li>").SetName("ShouldReturnTextWithTags_WhenThereIsItalicTagInBulletedTag"),
         new TestCaseData("* # _text_\r\n", "<li><h1><em>text</em></h1></li>").SetName("ShouldReturnTextWithTags_WhenThereIsHeaderTagInBulletedTag"),
         new TestCaseData("# * _text\r\n* _hello_\r\n", "<h1>* _text</h1>\n<li><em>hello</em></li>").SetName("ShouldReturnTextHeaderTags_WhenThereIsBulletedTagInHeaderTag")
        
    };

}