using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public static class MarkdownParserTestData
    {
        public static TestCaseData[] TestData =
        {
            new TestCaseData("_text_", new Token[]
            {
                new Token("_", TokenType.Tag, 0),
                new Token("text", TokenType.Text, 1),
                new Token("_", TokenType.Tag, 5)
            }).SetName("ParseText_WithItalic_ReturnsCorrectTokens"),

            new TestCaseData("__text__", new Token[]
            {
                new Token("__", TokenType.Tag, 0),
                new Token("text", TokenType.Text, 2),
                new Token("__", TokenType.Tag, 6)
            }).SetName("ParseText_WithStrong_ReturnsCorrectTokens"),

            new TestCaseData("text", new Token[]
            {
                new Token("text", TokenType.Text, 0)
            }).SetName("ParseText_WithoutTags_ReturnsTokenWithThisString"),

            new TestCaseData("\\_text_", new Token[]
            {
                new Token("_", TokenType.Text, 1),
                new Token("text", TokenType.Text, 2),
                new Token("_", TokenType.Text, 6)
            }).SetName("ParseText_WithEscapeSymbolAndTags_ReturnsCorrectTokens"),

            new TestCaseData("\\\\", new Token[]
            {
                new Token("\\", TokenType.Text, 1),
            }).SetName("ParseText_WithEscapedEscape_ReturnsTokenWithEscapeText"),

            new TestCaseData("сим\\волы", new Token[]
            {
                new Token("сим", TokenType.Text, 0),
                new Token("\\", TokenType.Text, 3),
                new Token("волы", TokenType.Text, 4)
            }).SetName("ParseText_WithNotEscapingEscape_ReturnsAllTextTokens"),

            new TestCaseData("# text\n", new Token[]
            {
                new Token("# ", TokenType.Tag, 0),
                new Token("text", TokenType.Text, 2),
                new Token("\n", TokenType.Tag, 6)
            }).SetName("ParseText_WithHeader_ReturnsCorrectTokens"),

            new TestCaseData("__text_", new Token[]
            {
                new Token("_", TokenType.Text, 0),
                new Token("_", TokenType.Tag, 1),
                new Token("text", TokenType.Text, 2),
                new Token("_", TokenType.Tag, 6)
            }).SetName("ParseText_WithNonPairTags_ReturnsCorrectTokens"),

            new TestCaseData("_text _text text_", new Token[]
            {
                new Token("_", TokenType.Text, 0),
                new Token("text ", TokenType.Text, 1),
                new Token("_", TokenType.Tag, 6),
                new Token("text text", TokenType.Text, 7),
                new Token("_", TokenType.Tag, 16),
            }).SetName("ParseText_WithWhiteSpacesBetweenTags_ReturnsItalicWithTokensWithoutWhiteSpacesBetween"),

            new TestCaseData("эти_ подчерки_ не", new Token[]
            {
                new Token("эти", TokenType.Text, 0),
                new Token("_", TokenType.Text, 3),
                new Token(" подчерки", TokenType.Text, 4),
                new Token("_", TokenType.Text, 13),
                new Token(" не", TokenType.Text, 14),
            }).SetName("ParseText_WithWhiteSpacesBeforeOpenTag_ReturnsAllTextTokens"),

            new TestCaseData("_123_", new Token[]
            {
                new Token("_", TokenType.Text, 0),
                new Token("123", TokenType.Text, 1),
                new Token("_", TokenType.Text, 4)
            }).SetName("ParseText_DigitsInItalic_ReturnsAllTextTokens"),

            new TestCaseData("_ab_c", new Token[]
            {
                new Token("_", TokenType.Text, 0),
                new Token("ab", TokenType.Text, 1),
                new Token("_", TokenType.Text, 3),
                new Token("c", TokenType.Text, 4),
            }).SetName("ParseText_ItalicInWord_ReturnsAllTextTokens"),

            new TestCaseData("____", new Token[]
            {
                new Token("_", TokenType.Text, 0),
                new Token("_", TokenType.Text, 1),
                new Token("_", TokenType.Text, 2),
                new Token("_", TokenType.Text, 3),
            }).SetName("ParseText_NoSymbolsBetweenTokens_ReturnsAllTextTokens"),

            new TestCaseData("ра_зных сл_овах", new Token[]
            {
                new Token("ра", TokenType.Text, 0),
                new Token("_", TokenType.Text, 2),
                new Token("зных сл", TokenType.Text, 3),
                new Token("_", TokenType.Text, 10),
                new Token("овах", TokenType.Text, 11)
            }).SetName("ParseText_ItalicInDifferentWords_ReturnsAllTextTokens"),

            new TestCaseData("# _text\n_", new Token[]
            {
                new Token("# ", TokenType.Tag, 0),
                new Token("_", TokenType.Text, 2),
                new Token("text", TokenType.Text, 3),
                new Token("\n", TokenType.Tag, 7),
                new Token("_", TokenType.Text, 8)
            }).SetName("ParseText_IncorrectItalicInHeader_ReturnsItalicInText"),

            new TestCaseData("_t __text__ t_", new Token[]
            {
                new Token("_", TokenType.Tag, 0),
                new Token("t ", TokenType.Text, 1),
                new Token("__", TokenType.Text, 3),
                new Token("text", TokenType.Text, 5),
                new Token("__", TokenType.Text, 9),
                new Token(" t", TokenType.Text, 11),
                new Token("_", TokenType.Tag, 13),
            }).SetName("ParseText_StrongInItalic_ReturnsStrongInTextTokens"),

            new TestCaseData("__t _text_ t__", new Token[]
            {
                new Token("__", TokenType.Tag, 0),
                new Token("t ", TokenType.Text, 2),
                new Token("_", TokenType.Tag, 4),
                new Token("text", TokenType.Text, 5),
                new Token("_", TokenType.Tag, 9),
                new Token(" t", TokenType.Text, 10),
                new Token("__", TokenType.Tag, 12),
            }).SetName("ParseText_ItalicInStrong_ReturnsStrongAndItalicTagTokens"),
        };
    }
}
