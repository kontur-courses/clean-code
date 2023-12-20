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
        { "\n", TagType.LineBreaker },
        { "\r\n", TagType.LineBreaker },
        { "* ", TagType.Bulleted }
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
            .SetName("Text without Tags"),
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
            .SetName("Text with Italic Tags"),
        new TestCaseData(@"_text\_",
                new List<Token>
                {
                    new("_", Tag.CreateTag(TagType.Italic, "_", null, "t"), TokenType.Tag),
                    new("text", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new("_", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Text with Escape Tags"),
        new TestCaseData(@"te\\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Escape),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Text with double Escape Tags"),
        new TestCaseData(@"te\xt",
                new List<Token>
                {
                    new("te", null, TokenType.Text),
                    new(@"\", null, TokenType.Text),
                    new("xt", null, TokenType.Text),
                    new("\n", null, TokenType.LineBreaker)
                })
            .SetName("Escape Tags in Text"),
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
    public void Parse_ShouldReturnItalicToken_WhenThereIsItalicToken(string text, List<Token> expectedTokens)
    {
        var parser = new Parser(tagDictionary);
        parser.Parse(text).Should().BeEquivalentTo(expectedTokens);
    }

    [TestCase("_text_", "<em>text</em>", TestName = "Italic")]
    [TestCase("_te_xt", "<em>te</em>xt", TestName = "Italic tag end in text")]
    [TestCase("te_xt_", "te<em>xt</em>", TestName = "Italic tag start in text")]
    [TestCase("te_x_t", "te<em>x</em>t", TestName = "Italic tag in text")]
    [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>", TestName = "Italic in Bold")]
    [TestCase("_1a_a1_", "_1a_a1_", TestName = "digit word not italic")]
    [TestCase("_1a_", "_1a_", TestName = "Mixes text is not italic")]
    public void Render_HandleItalicText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"_te\\_ xt_", "<em>te\\</em> xt_", TestName = "double escaped character")]
    [TestCase(@"\# _text", "# _text", TestName = " escaped character with header tag")]
    [TestCase(@"_te\xt_", @"<em>te\xt</em>", TestName = " escaped character in text")]
    [TestCase(@"\_te\xt_", @"_te\xt_", TestName = " escaped character ith opening tag")]
    [TestCase(@"_\te\xt_", @"<em>\te\xt</em>", TestName = " escaped character after tag")]
    public void EscapeTagTests(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("__text__", "<strong>text</strong>", TestName = "Bold")]
    [TestCase("__tex__t", "<strong>tex</strong>t", TestName = "Bold tag end in text")]
    [TestCase("te__xt__", "te<strong>xt</strong>", TestName = "Bold tag start in text")]
    [TestCase("te__x__t", "te<strong>x</strong>t", TestName = "Bold tag in text")]
    [TestCase("_a__b__c_", "<em>a__b__c</em>", TestName = "Bold In Italic")]
    [TestCase("__a __ b__", @"<strong>a __ b</strong>", TestName = "Bold with lonely tag in middle")]
    [TestCase("__11__", "__11__", TestName = "digit no Bold")]
    public void Render_HandleBoldText_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase("a_a b_b", @"a_a b_b", TestName = "Different words")]
    [TestCase("____", "____", TestName = "text Without any words")]
    [TestCase("__ _a_ __", @"__ <em>a</em> __", TestName = "Triple symbol")]
    [TestCase("a_ b_", @"a_ b_", TestName = "Invalid open symbol")]
    [TestCase("_text__text_", "_text__text_", TestName = "tag intersection")]
    [TestCase("_a", "_a", TestName = "Lonely open symbol")]
    [TestCase("__u _b_ _c_ u__", @"<strong>u <em>b</em> <em>c</em> u</strong>", TestName = "Multiply italic in bold")]
    [TestCase("a_", "a_", TestName = "Lonely close symbol")]
    [TestCase("text", "text", TestName = "text without tags")]
    [TestCase("text _", "text _", TestName = "space before closing tag")]
    [TestCase("_ text", "_ text", TestName = "space after opening tag")]
    [TestCase("____ text", "____ text", TestName = "Empty between paired tags")]
    public void Render_HandleTextWithPairedTags_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"# a", @"<h1>a</h1>", TestName = "Title")]
    [TestCase(@"\# a", @"# a", TestName = "Screen title")]
    [TestCase(@"\\# a", @"\# a", TestName = "Double screen title")]
    [TestCase("# _text_\n", "<h1><em>text</em></h1>")]
    [TestCase("# _text_\r\n", "<h1><em>text</em></h1>")]
    [TestCase("# _text\r\n_hello_\r\n", "<h1>_text</h1>\n<em>hello</em>")]
    public void Render_HeaderTagTestsShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [TestCase(@"\\* a", @"\* a", TestName = "Double screen Bulleted")]
    [TestCase("* _text_\n", "<li><em>text</em></li>")]
    [TestCase("* # _text_\r\n", "<li><h1><em>text</em></h1></li>")]
    [TestCase("* _text\r\n* _hello_\r\n", "<li>_text</li>\n<li><em>hello</em></li>")]
    [TestCase("# * _text\r\n* _hello_\r\n", "<h1>* _text</h1>\n<li><em>hello</em></li>")]
    public void Render_BulletedTagTests_ShouldBeExpected(string text, string expected)
    {
        sut.Render(text).Should().Be(expected);
    }

    [Test]
    public void LinearTimeComlexityTest()
    {
        const int linearCoefficient = 2;
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