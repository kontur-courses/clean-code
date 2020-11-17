using System;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class Renderer_Should
    {
        [TestCase("", TestName = "When string is empty")]
        [TestCase(null, TestName = "When string is null")]
        public void Render_ThrowException(string originalText)
        {
            var act = new Action(() => Renderer.Render(originalText));

            act.Should().Throw<ArgumentException>();
        }

        [TestCase("_aabb_", "<em>aabb</em>", TestName = "When simple italic tag")]
        [TestCase("a_b_", "a<em>b</em>", TestName = "When italic tag in word")]
        [TestCase("_d__a__f_", "<em>d__a__f</em>", TestName = "When bold inside italic selection")]
        [TestCase(@"\\_aabb_", @"\<em>aabb</em>", TestName = "When escaping slash")]
        public void Render_Italic(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("__aabb__", "<strong>aabb</strong>", TestName = "When simple bold")]
        [TestCase("a__b__", "a<strong>b</strong>", TestName = "When bold in word")]
        [TestCase(@"\\__aabb__", @"\<strong>aabb</strong>", TestName = "When escaping slash")]
        public void Render_Bold(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("____", TestName = "When empty inside bold")]
        [TestCase("__", TestName = "When empty inside italic")]
        [TestCase("__ aabb__", TestName = "When white space at the beginning of bold")]
        [TestCase("a__ b__", TestName = "When white space at the beginning of bold in word")]
        [TestCase("_ aabb_", TestName = "When white space at the beginning of italic")]
        [TestCase("a_ b_", TestName = "When white space at the beginning of italic in word")]
        [TestCase("__aabb __", TestName = "When white space at the end of bold")]
        [TestCase("a__b __", TestName = "When white space at the end of bold in word")]
        [TestCase("_aabb _", TestName = "When white space at the end of italic")]
        [TestCase("a_b _", TestName = "When white space at the end of italic in word")]
        [TestCase("__aabb", TestName = "When not bold end")]
        [TestCase("a__b", TestName = "When bold in word and not bold end")]
        [TestCase("_aabb", TestName = "When not italic end")]
        [TestCase("a_b", TestName = "When italic in word and not italic end")]
        [TestCase("__ab_bb__c_", TestName = "When italic and bold intersection")]
        [TestCase("aa_aaa c_c", TestName = "When italic in different words")]
        [TestCase("aa__aaa c__c", TestName = "When bold in different words")]
        [TestCase("_1_", TestName = "When only digits inside italic")]
        [TestCase("__12345__", TestName = "When only digits inside bold")]
        [TestCase("car#", TestName = "When header at the end")]
        [TestCase(" #car", TestName = "When header not at the beginning")]
        [TestCase(@"\dsd\", TestName = "When nothing escape")]
        [TestCase(@" - abcd", TestName = "When marker not at the beginning")]
        [TestCase(@"-abcd", TestName = "When not white space between the marker and the text")]
        [TestCase(@"- ", TestName = "When marked line is empty")]
        public void Render_SimpleText(string originalText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(originalText);
        }

        [TestCase("__ab__ _c_", "<strong>ab</strong> <em>c</em>", TestName = "When not intersection")]
        [TestCase("__d_a_f__", "<strong>d<em>a</em>f</strong>", TestName = "When italic inside bold selection")]
        public void Render_ItalicAndBold(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("#car", "<h1>car</h1>")]
        public void Render_Title_WhenSimpleHeaderSelection(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("#car \r\n#bmw \r\n#mercedes", "<h1>car </h1>\r\n<h1>bmw </h1>\r\n<h1>mercedes</h1>")]
        public void Render_Titles_WhenManyHeaders(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("#_a_ __b__ __c_d_c__ aa", "<h1><em>a</em> <strong>b</strong> <strong>c<em>d</em>c</strong> aa</h1>")]
        [TestCase("- _a_ __b__ __c_d_c__ aa",
            "<ul>\r\n <li><em>a</em> <strong>b</strong> <strong>c<em>d</em>c</strong> aa</li>\r\n</ul>")]
        public void Render_TagsCombination_WhenManyDifferentSelections(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase(@"\_dsd_", "_dsd_", TestName = "When escaping italic at the beginning")]
        [TestCase(@"\__dsd__", "__dsd__", TestName = "When escaping bold at the beginning")]
        [TestCase(@"_dsd\_", @"_dsd_", TestName = "When escaping italic at the end")]
        [TestCase(@"__dsd\__", @"__dsd__", TestName = "When escaping bold at the end")]
        [TestCase(@"\\\\dsd\\\\", @"\\dsd\\", TestName = "When many escaping")]
        public void Render_TextWithOutEscapingSymbols(string originalText, string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase(
            @"# __Honor__ __ok_generosity_ok__ \\_persistence_\\ _1creativity1_
__ emotionality__ _excitement\_ 
#__1_culture__1_111_",
            @"<h1> <strong>Honor</strong> <strong>ok<em>generosity</em>ok</strong> \<em>persistence</em>\ <em>1creativity1</em></h1>
__ emotionality__ _excitement_ 
<h1>__1_culture__1_111_</h1>")]
        public void Renderer_ManySelectionInParagraphs_WhenDifferentSelectionEscapingAndSpaces(string originalText,
            string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("- aaa", "<ul>\r\n <li>aaa</li>\r\n</ul>", TestName = "When simple marker")]
        [TestCase("- aaa\r\n- bcd\r\naa", "<ul>\r\n <li>aaa</li>\r\n <li>bcd</li>\r\n</ul>\r\naa",
            TestName = "When multiple marked lines")]
        [TestCase("- __ab_bb__c_", "<ul>\r\n <li>__ab_bb__c_</li>\r\n</ul>",
            TestName = "When inside marked line italic and bold intersection")]
        public void Renderer_MarkedList(string originalText,
            string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [TestCase("- aaa\r\nd\r\n- aaa", "<ul>\r\n <li>aaa</li>\r\n</ul>\r\nd\r\n<ul>\r\n <li>aaa</li>\r\n</ul>")]
        public void Renderer_MarkedLists_WhenBetweenMarkersSimpleText(string originalText,
            string expectedText)
        {
            var act = Renderer.Render(originalText);

            act.Should().Be(expectedText);
        }

        [Test, Timeout(1000)]
        public void Renderer_MakeFast_WhenManyTexts()
        {
            var text = "#_a_ __b__ __c_d_c__ aa\r\n- _a_ __b__ __c_d_c__ aa";
            for (var i = 0; i < 30000; i++)
                Renderer.Render(text);
        }
    }
}