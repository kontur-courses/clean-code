using FluentAssertions;
using Markdown;
using Markdown.Generators;
using Markdown.Parsers;
using NUnit.Framework;
using System.Text;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownToHtmlConverterTests
    {
        private static readonly HashSet<string> tagsSymbols = new HashSet<string>
        {
            "_", "__", "# ", "\n", "\\"
        };
        private MarkdownToHtmlConverter sut;
        
        [SetUp] 
        public void SetUp() 
        {
            var markdownParser = new MarkdownParser(tagsSymbols);
            var htmlGenerator = new HtmlGenerator();
            sut = new MarkdownToHtmlConverter(markdownParser, htmlGenerator);
        }

        [TestCase("", TestName = "Convert_EmptyString_ThrowsArgumentException")]
        [TestCase(null, TestName = "Convert_StringIsNull_ThrowsArgumentException")]
        public void Convert_IncorrectString_ThrowsArgumentException(string text)
        {
            var action = () => sut.Convert(text);
            action.Should().Throw<ArgumentException>();
        }

        [TestCase(@"\", @"\", TestName = "Convert_OneEscapeSymbol_ReturnsThisSymbol")]
        [TestCase(@"\\\text\\", @"\\text\", TestName = "Convert_ManyEscapeSymbol_ReturnsNonEscapedSymbol")]
        [TestCase(@"сим\волы экранирования\ \должны остаться.\", @"сим\волы экранирования\ \должны остаться.\",
            TestName = "Convert_ManyNonEscapedSymbols_ReturnsThisSymbolsAsString")]
        [TestCase("_12_", "_12_", TestName = "Convert_ItalicInDigits_ReturnsNonItalicDigits")]
        [TestCase("_text_", "<em>text</em>", TestName = "Convert_TextInItalic_ReturnsItalicTags")]
        [TestCase("_te_xt", "_te_xt", TestName = "Convert_ItalicInWord_ReturnsThisText")]
        [TestCase("te_xt tex_t", "te_xt tex_t", TestName = "Convert_ItalicInDifferentWords_ReturnsThisText")]
        [TestCase("_ text_", "_ text_", TestName = "Convert_WhiteSpaceBeforeItalicTag_ReturnsThisText")]
        [TestCase("_text _", "_text _", TestName = "Convert_WhiteSpaceAfterItalicTag_ReturnsThisText")]
        [TestCase("_text _text_", "_text <em>text</em>",
            TestName = "Convert_NonPairFirstItalicTag_ReturnsFirstItalicSymbolInTextAndItalicTag")]
        [TestCase("_text_ text_", "<em>text</em> text_", TestName = "Convert_WhiteSpaceAfterItalicTag_ReturnsThisText")]
        [TestCase("__text_", "_<em>text</em>", TestName = "Convert_AdditionalItalicSymbol_ReturnsItalicTagWithAdditionalSymbolWithouItalic")]
        [TestCase("__text__", "<strong>text</strong>", TestName = "Convert_WordStrongTag_ReturnsWordInHtmlStrongTag")]
        [TestCase("__ text__", "__ text__", TestName = "Convert_StrongTagWithWhiteSpaceAfter_ReturnsThisString")]
        [TestCase("__text __", "__text __", TestName = "Convert_StrongTagWithWhiteSpaceBefore_ReturnsThisString")]
        [TestCase("__text text__", "<strong>text text</strong>", TestName = "Convert_StrongTagWithSomeWords_ReturnsWordsInTag")]
        [TestCase("____text___", "_<strong><em>text</em></strong>", TestName = "Convert_EmInStrongWithExtraSymbol_ReturnsEmTagInStrongTag")]
        [TestCase("____", "____", TestName = "Convert_ManyUnderscoresWithoutText_ReturnsAllTextTags")]
        [TestCase("__text _text_ text__", "<strong>text <em>text</em> text</strong>",
            TestName = "Convert_EmInStrong_ReturnsEmTagInStrongTag")]
        [TestCase("_text __text__ text_", "<em>text __text__ text</em>", 
            TestName = "Convert_StrongInEm_ReturnsNotWorkingStrong")]
        [TestCase("\\_text_", "_text_", TestName = "Convert_EscapeEm_ReturnsAllTextTags")]
        public void Convert_DifferentText_ReturnsCorrectString(string text, string expected)
        {
            var result = sut.Convert(text);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        [Timeout(12500)]
        public void TimeTest()
        {
            var text = new StringBuilder();
            for (var i = 0; i < 1000000; i++)
            {
                text.Append("__text__");
            }

            sut.Convert(text.ToString());
        }
    }
}
