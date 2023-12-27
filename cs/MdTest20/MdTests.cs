using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MdTest20;

public class MdTests
{
    private Md sut;

    private readonly Dictionary<string?, TagType> tagDictionary = new()
    {
        { "_", TagType.Italic },
        { "__", TagType.Bold },
        { "# ", TagType.Heading },
        { "* ", TagType.Bulleted },
        { "## ", TagType.Heading },
        { "### ", TagType.Heading },
    };

    [SetUp]
    public void Setup()
    {
        sut = new Md(tagDictionary);
    }

    [TestCase("_text_", "<em>text</em>", TestName = "ShouldReturnItalicTags_WhenThereIsItalicTags")]
    [TestCase("_te_xt", "<em>te</em>xt", TestName = "ShouldReturnItalicTags_WhenThereIsItalicTagsInText")]
    [TestCase("te_xt_", "te<em>xt</em>", TestName = "ShouldReturnItalicTags_WhenThereIsItalicTagsOneTagInEnd")]
    [TestCase("te_x_t", "te<em>x</em>t", TestName = "ShouldReturnItalicTags_WhenThereIsItalicTwoTagsInText")]
    [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>",
        TestName = "ShouldReturnItalicAndBoldTags_WhenThereIsItalicInBoldTags")]
    [TestCase("_1a_a1_", "_1a_a1_", TestName = "ShouldReturnText_WhenThereIsItalicInDigitText")]
    [TestCase("_1a_", "_1a_", TestName = "ShouldReturnText_WhenThereIsItalicInMixesText")]
    public void Render_HandleItalicText(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"_te\\_ xt_", "<em>te\\</em> xt_",
        TestName = "ShouldReturnTextWithItalicTags_WhenThereIsDoubleEscapeInText")]
    [TestCase(@"_te\xt_", @"<em>te\xt</em>",
        TestName = "ShouldReturnTextWithItalicTags_WhenThereIsEscapeInText")]
    [TestCase(@"\_te\xt_", @"_te\xt_", TestName = "ShouldReturnText_WhenThereEscapeBlockedOpeningItalicTag")]
    [TestCase(@"_\te\xt_", @"<em>\te\xt</em>", TestName = "ShouldReturnWithTags_WhenThereEscapeAfterTag")]
    public void Render_EscapeTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("__text__", "<strong>text</strong>", TestName = "ShouldReturnTextWitBoldTags_WhenThereIsBoldTags")]
    [TestCase("__tex__t", "<strong>tex</strong>t",
        TestName = "ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInStartText")]
    [TestCase("te__xt__", "te<strong>xt</strong>",
        TestName = "ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInEndText")]
    [TestCase("te__x__t", "te<strong>x</strong>t",
        TestName = "ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInText")]
    [TestCase("_a__b__c_", "<em>a__b__c</em>",
        TestName = "ShouldReturnTextOnlyItalicTags_WhenThereIsBoldTagsInItalicTags")]
    [TestCase("__a __ b__", @"<strong>a __ b</strong>",
        TestName = "ShouldReturnTextWitBoldTags_WhenThereIsBoldTag")]
    [TestCase("__11__", "__11__", TestName = "ShouldReturnText_WhenThereIsItalicInDigitText")]
    public void Render_HandleBoldText(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("a_a b_b", @"a_a b_b", TestName = "ShouldReturnText_WhenThereIsTagsInDifferentWords")]
    [TestCase("____", "____", TestName = "ShouldReturnText_WhenThereIsOnlyTags")]
    [TestCase("__ _a_ __", @"__ <em>a</em> __",
        TestName = "ShouldReturnTextOnlyItalicTags_WhenThereIsBlobTagsWithSpacesAroundEdges")]
    [TestCase("a_ b_", @"a_ b_", TestName = "ShouldReturnText_WhenThereIsInvalidOpeningTag")]
    [TestCase("_text__text_", "_text__text_", TestName = "ShouldReturnText_WhenThereIsTagIntersection")]
    [TestCase("_a", "_a", TestName = "ShouldReturnText_WhenThereIsOnlyOpeningTag")]
    [TestCase("__u _b_ _c_ u__", @"<strong>u <em>b</em> <em>c</em> u</strong>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsMultiplyItalicInBold")]
    [TestCase("a_", "a_", TestName = "ShouldReturnText_WhenThereIsOnlyClosingTag")]
    [TestCase("text", "text", TestName = "ShouldReturnText_WhenThereIsOnlyText")]
    [TestCase("text _", "text _", TestName = "ShouldReturnText_WhenThereIsSpaceBeforeClosingTag")]
    [TestCase("_ text", "_ text", TestName = "ShouldReturnText_WhenThereIsSpaceAfterOpeningTag")]
    [TestCase("____ text", "____ text", TestName = "ShouldReturnText_WhenThereIsEmptyBetweenPairedTags")]
    [TestCase("", "", TestName = "ShouldReturnEmpty_WhenThereIsEmptyText")]
    public void Render_HandleTextWithPairedTags(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"# a", @"<h1>a</h1>", TestName = "ShouldReturnTextWithHeaderTag_WhenThereIsHeaderTag")]
    [TestCase(@"#a", @"#a", TestName = "ShouldReturnText_WhenThereIsIncorrectHeaderTag")]
    [TestCase(@"\# a", @"# a", TestName = "ShouldReturnText_WhenThereIsEscapeTagBeforeHeaderTag")]
    [TestCase("# _te# ext_\n", "<h1><em>te# ext</em></h1>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsItalicTagsInHeaderTag")]
    [TestCase("# _text_\r\n", "<h1><em>text</em></h1>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsAnotherTypeLineBreaker")]
    [TestCase("# _text\r\n_hello_\r\n", "<h1>_text</h1>\n<em>hello</em>",
        TestName = "ShouldReturnTextWithTags_WhenTransferringLineTagProcessingVeryBeginning")]
    [TestCase("## _text\r\n", "<h2>_text</h2>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsLevel2HeaderTag")]
    [TestCase("### _text\r\n", "<h3>_text</h3>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsLevel3HeaderTag")]
    public void Render_HeaderTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"\\* a", @"\* a", TestName = "ShouldReturnTextWithBulletedTag_WhenThereIsBulletedTag")]
    [TestCase("* _text_\n", "<li><em>text</em></li>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsItalicTagInBulletedTag")]
    [TestCase("* # _text_\r\n", "<li><h1><em>text</em></h1></li>",
        TestName = "ShouldReturnTextWithTags_WhenThereIsHeaderTagInBulletedTag")]
    [TestCase("# * _text\r\n* _hello_\r\n", "<h1>* _text</h1>\n<li><em>hello</em></li>",
        TestName = "ShouldReturnTextHeaderTags_WhenThereIsBulletedTagInHeaderTag")]
    public void Md_BulletedTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        const double linearCoefficient = 2;
        var sb = new StringBuilder();
        string testText;
        var mdExpression = "# I'm __living__ life do or _die_, what can I say\n" +
                           "* I'm 23 now will I ever live to see 24\n" +
                           "* The way things is going I don't know\n" +
                           "* # Tell m_e wh_y are we so blind to see\n" +
                           "__That the ones we hurt are you and me.__";
        sb.Append(mdExpression);
        var stopwatch = new Stopwatch();
        testText = sb.ToString();
        sut.Render(testText);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        stopwatch.Start();
        sut.Render(mdExpression);
        stopwatch.Stop();
        var previous = stopwatch.ElapsedTicks;
        for (var i = 0; i < 50; i++)
        {
            sb.Append(mdExpression);
            testText = sb.ToString();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            stopwatch.Restart();
            sut.Render( testText);
            stopwatch.Stop();
            Assert.That(stopwatch.ElapsedTicks / previous, Is.LessThanOrEqualTo(linearCoefficient));
            previous = stopwatch.ElapsedTicks;
        }
    }
}