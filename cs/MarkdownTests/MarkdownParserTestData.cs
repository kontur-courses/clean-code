using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests;

public static class MarkdownParserTestData
{
    public static TestCaseData[] TestData =
    {
        new TestCaseData("", new Token[]
        {
            new Token("", TokenType.Text, 0)
        }).SetName("empty string"),

        new TestCaseData("_text_", new Token[]
        {
            new Token("_", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 1),
            new Token("_", TokenType.Tag, 5)
        }).SetName("Simple italic tag"),

        new TestCaseData("__text__", new Token[]
        {
            new Token("__", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 2),
            new Token("__", TokenType.Tag, 6)
        }).SetName("Simple strong tag"),

        new TestCaseData("text", new Token[]
        {
            new Token("text", TokenType.Text, 0)
        }).SetName("Text without tags"),

        new TestCaseData("\\_text_", new Token[]
        {
            new Token("_", TokenType.Text, 1),
            new Token("text", TokenType.Text, 2),
            new Token("_", TokenType.Text, 6)
        }).SetName("Escaped italic tag"),

        new TestCaseData("\\\\", new Token[]
        {
            new Token("\\", TokenType.Text, 1),
        }).SetName("EscapedEscape"),

        new TestCaseData("сим\\волы", new Token[]
        {
            new Token("сим", TokenType.Text, 0),
            new Token("\\", TokenType.Text, 3),
            new Token("волы", TokenType.Text, 4)
        }).SetName("Escape in text"),

        new TestCaseData("# text\n", new Token[]
        {
            new Token("# ", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 2),
            new Token("\n", TokenType.Tag, 6)
        }).SetName("Simple header with new string char"),

        new TestCaseData("# text", new Token[]
        {
            new Token("# ", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 2),
            new Token("\n", TokenType.Tag, 6)
        }).SetName("Simple header without new string char"),

        new TestCaseData("__text_", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("_", TokenType.Tag, 1),
            new Token("text", TokenType.Text, 2),
            new Token("_", TokenType.Tag, 6)
        }).SetName("Non pair italic tag"),

        new TestCaseData("_text _text text_", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("text ", TokenType.Text, 1),
            new Token("_", TokenType.Tag, 6),
            new Token("text text", TokenType.Text, 7),
            new Token("_", TokenType.Tag, 16),
        }).SetName("Italic tag without pair"),

        new TestCaseData("эти_ подчерки_ не", new Token[]
        {
            new Token("эти", TokenType.Text, 0),
            new Token("_", TokenType.Text, 3),
            new Token(" подчерки", TokenType.Text, 4),
            new Token("_", TokenType.Text, 13),
            new Token(" не", TokenType.Text, 14),
        }).SetName("Space before open tag"),

        new TestCaseData("_123_", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("123", TokenType.Text, 1),
            new Token("_", TokenType.Text, 4)
        }).SetName("Digits in italic"),

        new TestCaseData("_ab_c", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("ab", TokenType.Text, 1),
            new Token("_", TokenType.Text, 3),
            new Token("c", TokenType.Text, 4),
        }).SetName("Italic in word"),

        new TestCaseData("____", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("_", TokenType.Text, 1),
            new Token("_", TokenType.Text, 2),
            new Token("_", TokenType.Text, 3),
        }).SetName("No symbols between Tokens"),

        new TestCaseData("ра_зных сл_овах", new Token[]
        {
            new Token("ра", TokenType.Text, 0),
            new Token("_", TokenType.Text, 2),
            new Token("зных сл", TokenType.Text, 3),
            new Token("_", TokenType.Text, 10),
            new Token("овах", TokenType.Text, 11)
        }).SetName("Italic in different words"),

        new TestCaseData("# _text\n_", new Token[]
        {
            new Token("# ", TokenType.Tag, 0),
            new Token("_", TokenType.Text, 2),
            new Token("text", TokenType.Text, 3),
            new Token("\n", TokenType.Tag, 7),
            new Token("_", TokenType.Text, 8)
        }).SetName("Closed italic symbol after header"),

        new TestCaseData("_t __text__ t_", new Token[]
        {
            new Token("_", TokenType.Tag, 0),
            new Token("t ", TokenType.Text, 1),
            new Token("__", TokenType.Text, 3),
            new Token("text", TokenType.Text, 5),
            new Token("__", TokenType.Text, 9),
            new Token(" t", TokenType.Text, 11),
            new Token("_", TokenType.Tag, 13),
        }).SetName("Strong in italic"),

        new TestCaseData("__t _text_ t__", new Token[]
        {
            new Token("__", TokenType.Tag, 0),
            new Token("t ", TokenType.Text, 2),
            new Token("_", TokenType.Tag, 4),
            new Token("text", TokenType.Text, 5),
            new Token("_", TokenType.Tag, 9),
            new Token(" t", TokenType.Text, 10),
            new Token("__", TokenType.Tag, 12),
        }).SetName("Italic in strong"),

        new TestCaseData("____text___", new Token[]
        {
            new Token("_", TokenType.Text, 0),
            new Token("__", TokenType.Tag, 1),
            new Token("_", TokenType.Tag, 3),
            new Token("text", TokenType.Text, 4),
            new Token("_", TokenType.Tag, 8),
            new Token("__", TokenType.Tag, 9)
        }).SetName("Italic in strong with first non pair tag"),

        new TestCaseData("[text](text)", new Token[]
        {
            new Token("[", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 1),
            new Token("]", TokenType.Tag, 5),
            new Token("(", TokenType.Tag, 6),
            new Token("text", TokenType.Text, 7),
            new Token(")", TokenType.Tag, 11),
        }).SetName("Simple link tags"),

        new TestCaseData("[text]text)", new Token[]
        {
            new Token("[", TokenType.Text, 0),
            new Token("text", TokenType.Text, 1),
            new Token("]", TokenType.Text, 5),
            new Token("text", TokenType.Text, 6),
            new Token(")", TokenType.Text, 10),
        }).SetName("Link tags without link open tag"),

        new TestCaseData("[text](_text_)", new Token[]
        {
            new Token("[", TokenType.Tag, 0),
            new Token("text", TokenType.Text, 1),
            new Token("]", TokenType.Tag, 5),
            new Token("(", TokenType.Tag, 6),
            new Token("_", TokenType.Text, 7),
            new Token("text", TokenType.Text, 8),
            new Token("_", TokenType.Text, 12),
            new Token(")", TokenType.Tag, 13),
        }).SetName("Italic in link"),

        new TestCaseData("[_text_](text)", new Token[]
        {
            new Token("[", TokenType.Tag, 0),
            new Token("_", TokenType.Tag, 1),
            new Token("text", TokenType.Text, 2),
            new Token("_", TokenType.Tag, 6),
            new Token("]", TokenType.Tag, 7),
            new Token("(", TokenType.Tag, 8),
            new Token("text", TokenType.Text, 9),
            new Token(")", TokenType.Tag, 13),
        }).SetName("Italic in link description"),

        new TestCaseData("[_text](text_)", new Token[]
        {
            new Token("[", TokenType.Tag, 0),
            new Token("_", TokenType.Text, 1),
            new Token("text", TokenType.Text, 2),
            new Token("]", TokenType.Tag, 6),
            new Token("(", TokenType.Tag, 7),
            new Token("text", TokenType.Text, 8),
            new Token("_", TokenType.Text, 12),
            new Token(")", TokenType.Tag, 13),
        }).SetName("Italic between link tags"),

        new TestCaseData("[[(text)]](text)", new Token[]
        {
            new Token("[", TokenType.Tag, 0),
            new Token("[", TokenType.Text, 1),
            new Token("(", TokenType.Text, 2),
            new Token("text", TokenType.Text, 3),
            new Token(")", TokenType.Text, 7),
            new Token("]", TokenType.Text, 8),
            new Token("]", TokenType.Tag, 9),
            new Token("(", TokenType.Tag, 10),
            new Token("text", TokenType.Text, 11),
            new Token(")", TokenType.Tag, 15),
        }).SetName("Some brackets in description link")
    };
}