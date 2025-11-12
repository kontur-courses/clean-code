using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

public class MdTests
{
    private Md md = null!;

    [SetUp]
    public void SetUp()
        => md = new Md();

    private string R(string markdown)
        => Md.Render(markdown);

    [Test]
    public void Md_ShouldRenderH1_WhenLineStartsWithHashAndSpace_Test()
    {
        R("# Hello world")
            .Should().Be("<h1>Hello world</h1>");
    }

    [Test]
    public void Md_ShouldNotRenderH1_WhenHashNotFollowedBySpace_Test()
    {
        R("#Hello")
            .Should().Be("<p>#Hello</p>");
    }

    [Test]
    public void Md_ShouldRenderParagraph_WhenTextWithoutHeader_Test()
    {
        R("hello")
            .Should().Be("<p>hello</p>");
    }

    [Test]
    public void Md_ShouldRenderTwoParagraphs_WhenSeparatedByEmptyLine_Test()
    {
        R("hello\n\nworld")
            .Should().Be("<p>hello</p><p>world</p>");
    }

    [Test]
    public void Md_ShouldRenderEm_WhenSingleUnderscorePair_Test()
    {
        R("a _b_ c")
            .Should().Be("<p>a <em>b</em> c</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenSpacesAroundOpening_Test()
    {
        R("a _ b_")
            .Should().Be("<p>a _ b_</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenSpacesBeforeClosing_Test()
    {
        R("a _b _")
            .Should().Be("<p>a _b _</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenUnderscoreInsideDigits_Test()
    {
        R("12_3")
            .Should().Be("<p>12_3</p>");
    }

    [Test]
    public void Md_ShouldRenderEm_WhenInsideWord_Test()
    {
        R("a_b_c")
            .Should().Be("<p>a<em>b</em>c</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenBetweenWords_Test()
    {
        R("a _b c_")
            .Should().Be("<p>a _b c_</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenInsideDigitsExtended_Test()
    {
        R("12_34_56")
            .Should().Be("<p>12_34_56</p>");
    }

    [Test]
    public void Md_ShouldRenderStrong_WhenDoubleUnderscorePair_Test()
    {
        R("a __b__ c")
            .Should().Be("<p>a <strong>b</strong> c</p>");
    }

    [Test]
    public void Md_ShouldNotRenderStrong_WhenEmptyContent_Test()
    {
        R("____")
            .Should().Be("<p>____</p>");
    }

    [Test]
    public void Md_ShouldAllowStrongInsideStrong_WhenValid_Test()
    {
        R("__a __b__ c__")
            .Should().Be("<p><strong>a <strong>b</strong> c</strong></p>");
    }

    [Test]
    public void Md_ShouldRenderNestedEmInsideStrong_WhenValidStructure_Test()
    {
        R("__a _b_ c__")
            .Should().Be("<p><strong>a <em>b</em> c</strong></p>");
    }

    [Test]
    public void Md_ShouldRenderNestedEmInsideStrong_MultipleContent_Test()
    {
        R("__a _b c_ d__")
            .Should().Be("<p><strong>a <em>b c</em> d</strong></p>");
    }

    [Test]
    public void Md_ShouldNotRenderNested_WhenIntersectionInvalid_Test()
    {
        R("_a __b_ c__")
            .Should().Be("<p>_a __b_ c__</p>");
    }

    [Test]
    public void Md_ShouldNotRender_WhenDoubleSingleIntersectOpposite_Test()
    {
        R("__a _b__ c_")
            .Should().Be("<p>__a _b__ c_</p>");
    }

    [Test]
    public void Md_ShouldEscapeUnderscore_WhenBackslashBefore_Test()
    {
        R(@"\_a\_")
            .Should().Be("<p>_a_</p>");
    }

    [Test]
    public void Md_ShouldOutputBackslashLiteral_WhenEscapeWithoutNextChar_Test()
    {
        R(@"\")
            .Should().Be("<p>\\</p>");
    }

    [Test]
    public void Md_ShouldEscapeBackslash_WhenDoubleBackslash_Test()
    {
        R(@"\\")
            .Should().Be("<p>\\</p>");
    }

    [Test]
    public void Md_ShouldEscapeUnderscoreInsideWord_Test()
    {
        R("hel\\_lo")
            .Should().Be("<p>hel_lo</p>");
    }

    [Test]
    public void Md_ShouldEscapeDoubleUnderscore_Test()
    {
        R(@"\__a__")
            .Should().Be("<p>__a__</p>");
    }

    [Test]
    public void Md_ShouldEscapeUnderscore_WhenAtEnd_Test()
    {
        R("a \\_")
            .Should().Be("<p>a _</p>");
    }

    [Test]
    public void Md_ShouldHandleEscapeBeforeLetters_AsLiteral_Test()
    {
        R(@"\hello")
            .Should().Be("<p>\\hello</p>");
    }

    [Test]
    public void Md_ShouldRenderLink_WhenValidMarkdownLink_Test()
    {
        R("[google](https://google.com)")
            .Should().Be("<p><a href=\"https://google.com\">google</a></p>");
    }

    [Test]
    public void Md_ShouldNotRenderLink_WhenMissingRightBracket_Test()
    {
        R("[text(https://url)")
            .Should().Be("<p>[text(https://url)</p>");
    }

    [Test]
    public void Md_ShouldNotRenderLink_WhenMissingParenthesis_Test()
    {
        R("[text]url)")
            .Should().Be("<p>[text]url)</p>");
    }

    [Test]
    public void Md_ShouldRenderLinkInsideStrong_Test()
    {
        R("__hello [g](u)__")
            .Should().Be("<p><strong>hello <a href=\"u\">g</a></strong></p>");
    }

    [Test]
    public void Md_ShouldHandleEscapeInsideLinkText_Test()
    {
        R("[go\\_og\\_le](url)")
            .Should().Be("<p><a href=\"url\">go_og_le</a></p>");
    }

    [Test]
    public void Md_ShouldRenderMixedStyles_WhenMultipleInlineTokens_Test()
    {
        R("Hello _world_ and __strong__ text")
            .Should().Be("<p>Hello <em>world</em> and <strong>strong</strong> text</p>");
    }

    [Test]
    public void Md_ShouldKeepUnderscoresLiteral_WhenUnmatched_Test()
    {
        R("_a")
            .Should().Be("<p>_a</p>");
    }

    [Test]
    public void Md_ShouldRenderEmptyString_AsEmptyString_Test()
    {
        R("").Should().Be("");
    }

    [Test]
    public void Md_ShouldThrowException_WhenGettingNull_Test()
    {
        Action act = () => Md.Render(null);
        act.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Md_ShouldKeepMultipleNewlines_AsParagraphBreaks_Test()
    {
        R("a\n\n\nb")
            .Should().Be("<p>a</p><p>b</p>");
    }

    [Test]
    public void Md_ShouldTreatHashInsideText_AsLiteral_Test()
    {
        R("a # b")
            .Should().Be("<p>a # b</p>");
    }

    [Test]
    public void Md_ShouldNotRenderLink_WhenNoClosingParenthesis_Test()
    {
        R("[text](url")
            .Should().Be("<p>[text](url</p>");
    }

    [Test]
    public void Md_ShouldNotRenderLink_WhenNoClosingBracket_Test()
    {
        R("[text(url)")
            .Should().Be("<p>[text(url)</p>");
    }

    [Test]
    public void Md_ShouldAllowTextBeforeLink_Test()
    {
        R("hello [x](y)")
            .Should().Be("<p>hello <a href=\"y\">x</a></p>");
    }

    [Test]
    public void Md_ShouldAllowTextAfterLink_Test()
    {
        R("[x](y) world")
            .Should().Be("<p><a href=\"y\">x</a> world</p>");
    }

    [Test]
    public void Md_ShouldNotBreak_WhenLinkInsideEscapedBrackets_Test()
    {
        R(@"\[link](u)")
            .Should().Be("<p>[link](u)</p>");
    }

    [Test]
    public void Md_ShouldNotRenderStrong_WhenFollowedByWhitespace_Test()
    {
        R("__ a__")
            .Should().Be("<p>__ a__</p>");
    }

    [Test]
    public void Md_ShouldNotRenderStrong_WhenEndingWithWhitespace_Test()
    {
        R("__a __")
            .Should().Be("<p>__a __</p>");
    }

    [Test]
    public void Md_ShouldRenderStrongOverMultipleInlines_Test()
    {
        R("__a [x](y) b__")
            .Should().Be("<p><strong>a <a href=\"y\">x</a> b</strong></p>");
    }

    [Test]
    public void Md_ShouldRenderEmInsideWord_MultipleTimes_Test()
    {
        R("a_b_c_d_e")
            .Should().Be("<p>a<em>b</em>c<em>d</em>e</p>");
    }

    [Test]
    public void Md_ShouldNotRenderEm_WhenSingleUnderscoreAtStartAndSpaceAfter_Test()
    {
        R("_ a_")
            .Should().Be("<p>_ a_</p>");
    }

    [Test]
    public void Md_ShouldHandleEscapedHash_AsLiteral_Test()
    {
        R(@"\# header")
            .Should().Be("<p># header</p>");
    }

    [Test]
    public void Md_ShouldRenderHeader_WithInlineFormatting_Test()
    {
        R("# __a _b_ c__")
            .Should().Be("<h1><strong>a <em>b</em> c</strong></h1>");
    }

    [Test]
    public void Md_ShouldTreatSingleBackslashAtEnd_AsLiteral_Test()
    {
        R("abc\\")
            .Should().Be("<p>abc\\</p>");
    }

    [Test]
    public void Md_ShouldIgnoreUnmatchedOpeningBracket_Test()
    {
        R("text [abc")
            .Should().Be("<p>text [abc</p>");
    }

    [Test]
    public void Md_ShouldIgnoreUnmatchedClosingBracket_Test()
    {
        R("text ]abc")
            .Should().Be("<p>text ]abc</p>");
    }

    [Test]
    public void Md_ShouldHandleMultipleLinksInRow_Test()
    {
        R("[a](1)[b](2)[c](3)")
            .Should().Be("<p><a href=\"1\">a</a><a href=\"2\">b</a><a href=\"3\">c</a></p>");
    }

    [Test]
    public void Md_ShouldHandleMultipleEscapesInRow_Test()
    {
        R("\\\\\\\\__a__")
            .Should().Be("<p>\\\\<strong>a</strong></p>");
    }

    [Test]
    public void Md_ShouldNotRenderCrossingEmStrong_ComplexCase_Test()
    {
        R("_a __b c_ d__")
            .Should().Be("<p>_a __b c_ d__</p>");
    }

    [Test]
    public void Md_ShouldRenderLoneHash_AsLiteral_Test()
    {
        R("#")
            .Should().Be("<p>#</p>");
    }

    [Test]
    public void Md_ShouldRenderSpaceThenHash_AsParagraph_Test()
    {
        R(" # heading")
            .Should().Be("<p> # heading</p>");
    }

    [Test]
    public void Md_ShouldNotBreakOnVeryLongGarbage_Test()
    {
        var s = new string('[', 2000) + new string(')', 2000);
        R(s)
            .Should().Be("<p>" + s + "</p>");
    }
}