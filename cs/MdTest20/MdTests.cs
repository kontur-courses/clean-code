using System.Diagnostics;
using FluentAssertions;
using Markdown;
using Markdown.Tags;

namespace MdTest20;

public class MdTests
{
    private Md sut;

    private readonly Dictionary<string, TagType> tagDictionary = new()
    {
        { "_", TagType.Italic },
        { "__", TagType.Bold },
        { "# ", TagType.Heading },
        { "* ", TagType.Bulleted },
        { "## ", TagType.Heading },
        { "### ", TagType.Heading },
        { "\n", TagType.LineBreaker },
        { "\r\n", TagType.LineBreaker }
    };

    [SetUp]
    public void Setup()
    {
        sut = new Md(tagDictionary);
    }

    private static IEnumerable<TestCaseData> ConstructorParserExpectedTokenList => new[]
    {
        new TestCaseData("text",
                new List<Token> { new("text", null, TokenType.Text), new("\n", null, TokenType.LineBreaker) })
            .SetName("Parse_ShouldReturnText_WhenThereOnlyText"),
        new TestCaseData("_tex_t",
                new List<Token>
                {
                    new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag),
                    new("tex", null, TokenType.Text),
                    new("_", Tag.CreateTag(TagType.Italic, "_", new Token("tex", null, TokenType.Text), null),
                        TokenType.Tag),
                    new("t", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Parse_ShouldReturnItalicToken_WhenThereIsItalicToken"),
        new TestCaseData(@"text\_",
                new List<Token>
                {
                    new("text", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new("_", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Parse_ShouldReturnText_WhenThereIsEscapeTokenBeforeTagToken"),
        new TestCaseData(@"te\\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Parse_ShouldReturnDoubleEscapeToken_WhenThereIsDoubleEscapeTokenInText"),
        new TestCaseData(@"te\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Parse_ShouldReturnEscapeToken_WhenThereIsEscapeToken"),
        new TestCaseData("__tex_t",
                new List<Token>
                {
                    new("__", Tag.CreateTag(TagType.Bold, "__", null, "t"), TokenType.Tag),
                    new("tex", null, TokenType.Text),
                    new("_", Tag.CreateTag(TagType.Italic, "_", new Token("tex", null, TokenType.Text), null),
                        TokenType.Tag),
                    new("t", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Tag Mix in Text")
    };

    [TestCaseSource(nameof(ConstructorParserExpectedTokenList))]
    public void Parse_ShouldReturnCorrectTokenList_PareSomeTokenList(string text, List<Token> expectedTokens)
    {
        var parser = new Parser(tagDictionary);
        parser.Parse(text).Should().BeEquivalentTo(expectedTokens);
    }

    [TestCase("_text_", "<em>text</em>", TestName = "Render_ShouldReturnItalicTags_WhenThereIsItalicTags")]
    [TestCase("_te_xt", "<em>te</em>xt", TestName = "Render_ShouldReturnItalicTags_WhenThereIsItalicTagsInText")]
    [TestCase("te_xt_", "te<em>xt</em>", TestName = "Render_ShouldReturnItalicTags_WhenThereIsItalicTagsOneTagInEnd")]
    [TestCase("te_x_t", "te<em>x</em>t", TestName = "Render_ShouldReturnItalicTags_WhenThereIsItalicTwoTagsInText")]
    [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>",
        TestName = "Render_ShouldReturnItalicAndBoldTags_WhenThereIsItalicInBoldTags")]
    [TestCase("_1a_a1_", "_1a_a1_", TestName = "Render_ShouldReturnText_WhenThereIsItalicInDigitText")]
    [TestCase("_1a_", "_1a_", TestName = "Render_ShouldReturnText_WhenThereIsItalicInMixesText")]
    public void Render_HandleItalicText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"_te\\_ xt_", "<em>te\\</em> xt_",
        TestName = "Render_ShouldReturnTextWithItalicTags_WhenThereIsDoubleEscapeInText")]
    [TestCase(@"_te\xt_", @"<em>te\xt</em>",
        TestName = "Render_ShouldReturnTextWithItalicTags_WhenThereIsEscapeInText")]
    [TestCase(@"\_te\xt_", @"_te\xt_", TestName = "Render_ShouldReturnText_WhenThereEscapeBlockedOpeningItalicTag")]
    [TestCase(@"_\te\xt_", @"<em>\te\xt</em>", TestName = "Render_ShouldReturnWithTags_WhenThereEscapeAfterTag")]
    public void EscapeTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("__text__", "<strong>text</strong>", TestName = "Render_ShouldReturnTextWitBoldTags_WhenThereIsBoldTags")]
    [TestCase("__tex__t", "<strong>tex</strong>t",
        TestName = "Render_ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInStartText")]
    [TestCase("te__xt__", "te<strong>xt</strong>",
        TestName = "Render_ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInEndText")]
    [TestCase("te__x__t", "te<strong>x</strong>t",
        TestName = "Render_ShouldReturnTextWitBoldTags_WhenThereIsBoldTagsInText")]
    [TestCase("_a__b__c_", "<em>a__b__c</em>",
        TestName = "Render_ShouldReturnTextOnlyItalicTags_WhenThereIsBoldTagsInItalicTags")]
    [TestCase("__a __ b__", @"<strong>a __ b</strong>",
        TestName = "Render_ShouldReturnTextWitBoldTags_WhenThereIsBoldTag")]
    [TestCase("__11__", "__11__", TestName = "Render_ShouldReturnText_WhenThereIsItalicInDigitText")]
    public void Render_HandleBoldText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("a_a b_b", @"a_a b_b", TestName = "Render_ShouldReturnText_WhenThereIsTagsInDifferentWords")]
    [TestCase("____", "____", TestName = "Render_ShouldReturnText_WhenThereIsOnlyTags")]
    [TestCase("__ _a_ __", @"__ <em>a</em> __",
        TestName = "Render_ShouldReturnTextOnlyItalicTags_WhenThereIsBlobTagsWithSpacesAroundEdges")]
    [TestCase("a_ b_", @"a_ b_", TestName = "Render_ShouldReturnText_WhenThereIsInvalidOpeningTag")]
    [TestCase("_text__text_", "_text__text_", TestName = "Render_ShouldReturnText_WhenThereIsTagIntersection")]
    [TestCase("_a", "_a", TestName = "Render_ShouldReturnText_WhenThereIsOnlyOpeningTag")]
    [TestCase("__u _b_ _c_ u__", @"<strong>u <em>b</em> <em>c</em> u</strong>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsMultiplyItalicInBold")]
    [TestCase("a_", "a_", TestName = "Render_ShouldReturnText_WhenThereIsOnlyClosingTag")]
    [TestCase("text", "text", TestName = "Render_ShouldReturnText_WhenThereIsOnlyText")]
    [TestCase("text _", "text _", TestName = "Render_ShouldReturnText_WhenThereIsSpaceBeforeClosingTag")]
    [TestCase("_ text", "_ text", TestName = "Render_ShouldReturnText_WhenThereIsSpaceAfterOpeningTag")]
    [TestCase("____ text", "____ text", TestName = "Render_ShouldReturnText_WhenThereIsEmptyBetweenPairedTags")]
    [TestCase("", "", TestName = "Render_ShouldReturnEmpty_WhenThereIsEmptyText")]
    public void Render_HandleTextWithPairedTags_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"# a", @"<h1>a</h1>", TestName = "Render_ShouldReturnTextWithHeaderTag_WhenThereIsHeaderTag")]
    [TestCase(@"#a", @"#a", TestName = "Render_ShouldReturnText_WhenThereIsIncorrectHeaderTag")]
    [TestCase(@"\# a", @"# a", TestName = "Render_ShouldReturnText_WhenThereIsEscapeTagBeforeHeaderTag")]
    [TestCase("# _text_\n", "<h1><em>text</em></h1>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsItalicTagsInHeaderTag")]
    [TestCase("# _text_\r\n", "<h1><em>text</em></h1>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsAnotherTypeLineBreaker")]
    [TestCase("# _text\r\n_hello_\r\n", "<h1>_text</h1>\n<em>hello</em>",
        TestName = "Render_ShouldReturnTextWithTags_WhenTransferringLineTagProcessingVeryBeginning")]
    [TestCase("## _text\r\n", "<h2>_text</h2>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsLevel2HeaderTag")]
    [TestCase("### _text\r\n", "<h3>_text</h3>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsLevel3HeaderTag")]
    public void Render_HeaderTagTestsShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"\\* a", @"\* a", TestName = "Render_ShouldReturnTextWithBulletedTag_WhenThereIsBulletedTag")]
    [TestCase("* _text_\n", "<li><em>text</em></li>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsItalicTagInBulletedTag")]
    [TestCase("* # _text_\r\n", "<li><h1><em>text</em></h1></li>",
        TestName = "Render_ShouldReturnTextWithTags_WhenThereIsHeaderTagInBulletedTag")]
    [TestCase("# * _text\r\n* _hello_\r\n", "<h1>* _text</h1>\n<li><em>hello</em></li>",
        TestName = "Render_ShouldReturnTextHeaderTags_WhenThereIsBulletedTagInHeaderTag")]
    public void Render_BulletedTagTests_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        const double linearCoefficient = 2;
        var mdExpression = "# I'm __living__ life do or _die_, what can I say\n" +
                           "* I'm 23 now will I ever live to see 24\n" +
                           "* The way things is going I don't know\n" +
                           "* # Tell m_e wh_y are we so blind to see\n" +
                           "__That the ones we hurt are you and me.__";
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        sut.Render(mdExpression);
        stopwatch.Stop();
        var previous = stopwatch.ElapsedTicks;
        for (var i = 0; i < 5; i++)
        {
            mdExpression += mdExpression;
            stopwatch.Restart();
            sut.Render(mdExpression);
            stopwatch.Stop();

            Assert.That(stopwatch.ElapsedTicks / previous, Is.LessThanOrEqualTo(linearCoefficient));
            previous = stopwatch.ElapsedTicks;
        }
    }
}