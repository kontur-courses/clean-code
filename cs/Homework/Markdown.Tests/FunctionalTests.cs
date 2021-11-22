using FluentAssertions;
using Markdown.Lexer;
using Markdown.Parser;
using Markdown.SyntaxParser;
using Markdown.TokenFormatter;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class FunctionalTests
    {
        private Markdown sut;

        [SetUp]
        public void SetUp()
        {
            var lexer = new MarkdownLexer();
            var syntaxParser = new MarkdownSyntaxParser();
            var parser = new MarkdownParser(lexer, syntaxParser);
            var tokensFormatter = new HtmlTokensFormatter();
            sut = new Markdown(parser, tokensFormatter);
        }

        [TestCase("", "", TestName = "Empty text")]
        [TestCase("abc", "abc", TestName = "Only one word")]
        [TestCase("abc xyz", "abc xyz", TestName = "Word separated by whitespace")]
        [TestCase("\nabc\nxyz\n", "\nabc\nxyz\n", TestName = "Text with several paragraphs")]
        [TestCase("\n", "\n", TestName = "Only new line")]
        public void Render_Should_RenderText_When(string text, string expectedResult)
        {
            RenderAndAssert(text, expectedResult);
        }

        [TestCase("_abc_", "<em>abc</em>", TestName = "One italics")]
        [TestCase("_abc_ _xyz_", "<em>abc</em> <em>xyz</em>", TestName = "Two italics separated by whitespace")]
        [TestCase("_abc_\n_xyz_", "<em>abc</em>\n<em>xyz</em>", TestName = "Two italics separated by new line")]
        [TestCase("_abc_a_xyz_", "<em>abc</em>a<em>xyz</em>", TestName = "Italics covers only part of the word")]
        [TestCase("____", "____", TestName = "Two italics without text")]
        public void Render_Should_RenderItalics_When(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        [TestCase("__abc__", "<strong>abc</strong>", TestName = "One bold")]
        [TestCase("__abc__ __xyz__", "<strong>abc</strong> <strong>xyz</strong>",
            TestName = "Two bold separated by whitespace")]
        [TestCase("__abc__\n__xyz__", "<strong>abc</strong>\n<strong>xyz</strong>",
            TestName = "Two bold separated by new line")]
        [TestCase("__abc__a__xyz__", "<strong>abc</strong>a<strong>xyz</strong>",
            TestName = "Bold covers only part of the word")]
        [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>", TestName = "Italics in bold")]
        [TestCase("________", "________", TestName = "Two bold without text")]
        public void Render_Should_RenderBold_When(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        [TestCase("_a__b__c_", "<em>a__b__c</em>", TestName = "Bold in Italics")]
        [TestCase("_a__", "_a__", TestName = "Not paired tags in one line")]
        [TestCase("10_000_000", "10_000_000", TestName = "Italics in digits")]
        [TestCase("10__000__000", "10__000__000", TestName = "Bold in digits")]
        [TestCase("ab_c x_yz", "ab_c x_yz", TestName = "Italics tags in different words")]
        [TestCase("ab__c x__yz", "ab__c x__yz", TestName = "Bold tags in different words")]
        [TestCase("_ abc_", "_ abc_", TestName = "Whitespace after open italics tag")]
        [TestCase("_abc _", "_abc _", TestName = "Whitespace before closing italics tag")]
        [TestCase("__ abc__", "__ abc__", TestName = "Whitespace after open bold tag")]
        [TestCase("__abc __", "__abc __", TestName = "Whitespace before closing bold tag")]
        [TestCase("_abc __xyz_ abc__", "_abc __xyz_ abc__", TestName = "Italics intersects with bold")]
        [TestCase("__abc _xyz__ abc_", "__abc _xyz__ abc_", TestName = "Bold intersects with italics")]
        public void Render_Should_RenderItalicsAndBoldAsHtml_When(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        [TestCase("# ", "", TestName = "Only header tag")]
        [TestCase("# abc", "<h1>abc</h1>", TestName = "Header in the beginning of the text")]
        [TestCase("\n# abc", "\n<h1>abc</h1>", TestName = "Header in new line")]
        [TestCase("# _abc_ __abc__", "<h1><em>abc</em> <strong>abc</strong></h1>",
            TestName = "Header with italics and bold")]
        public void Render_Should_Render_Header1(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        [TestCase("abc # abc", "abc # abc", TestName = "Tag in the middle of line")]
        [TestCase("abc # ", "abc # ", TestName = "Tag in the end of line")]
        public void Render_Should_RenderHeader1AsHtml_When(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        [TestCase("\\", "\\", TestName = "Only escape symbol")]
        [TestCase("\\_abc_", "_abc_", TestName = "Shielding italics")]
        [TestCase("\\__abc__", "__abc__", TestName = "Shielding bold")]
        [TestCase("\\# abc", "# abc", TestName = "Shielding header")]
        [TestCase("\\a\\b\\c", "\\a\\b\\c", TestName = "Shielding text")]
        [TestCase("\\\\", "\\", TestName = "Shielding itself")]
        public void Render_Should_Consider_EscapeSymbols(string text, string expected)
        {
            RenderAndAssert(text, expected);
        }

        private void RenderAndAssert(string text, string expectedResult)
        {
            var htmlText = sut.Render(text);

            htmlText.Should().Be(expectedResult);
        }
    }
}