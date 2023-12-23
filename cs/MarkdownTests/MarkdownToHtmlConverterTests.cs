using FluentAssertions;
using Markdown;
using Markdown.Generators;
using Markdown.Parsers;
using NUnit.Framework;
using System.Text;

namespace MarkdownTests;

public class MarkdownToHtmlConverterTests
{
    private MarkdownToHtmlConverter sut;
    
    [SetUp] 
    public void SetUp() 
    {
        var markdownParser = new MarkdownParser();
        var htmlGenerator = new HtmlGenerator();
        sut = new MarkdownToHtmlConverter(markdownParser, htmlGenerator);
    }

    [TestCase("", TestName = "Empty string")]
    [TestCase(null, TestName = "String is null")]
    public void Convert_EmptyString_NotThrowsArgumentException(string text)
    {
        var action = () => sut.Convert(text);
        action.Should().NotThrow<ArgumentException>();
    }

    [TestCase(@"\", @"\", TestName = "Only escape symbol")]
    [TestCase(@"\\\text\\", @"\\text\", TestName = "Many escaped symbols")]
    [TestCase(@"сим\волы экранирования\ \должны остаться.\", @"сим\волы экранирования\ \должны остаться.\",
        TestName = "Many not escaped symbols")]
    [TestCase("_12_", "_12_", TestName = "Digits in italic")]
    [TestCase("_text_", "<em>text</em>", TestName = "Symple italic")]
    [TestCase("_te_xt", "_te_xt", TestName = "Italic in word")]
    [TestCase("te_xt tex_t", "te_xt tex_t", TestName = "Italic in different words")]
    [TestCase("_ text_", "_ text_", TestName = "White space before italic tag")]
    [TestCase("_text _", "_text _", TestName = "White space after italic tag")]
    [TestCase("_text _text_", "_text <em>text</em>",
        TestName = "non pair first open italic tag")]
    [TestCase("_text_ text_", "<em>text</em> text_", TestName = "Non pair last italic tag")]
    [TestCase("__text_", "_<em>text</em>", TestName = "Additional italic symbol")]
    [TestCase("__text__", "<strong>text</strong>", TestName = "Simple strong tag")]
    [TestCase("__ text__", "__ text__", TestName = "White space after open strong tag")]
    [TestCase("__text __", "__text __", TestName = "White space before closed strong tag")]
    [TestCase("__text text text__", "<strong>text text text</strong>", TestName = "Many words in strong tag")]
    [TestCase("____text___", "_<strong><em>text</em></strong>", TestName = "Em in strong with additional symbol")]
    [TestCase("____", "____", TestName = "Many underscores without text")]
    [TestCase("__text _text_ text__", "<strong>text <em>text</em> text</strong>",
        TestName = "Em in strong")]
    [TestCase("_text __text__ text_", "<em>text __text__ text</em>", 
        TestName = "Strong in em")]
    [TestCase("\\_text_", "_text_", TestName = "Escaped open italic tag")]
    [TestCase("# text\n", "<h1>text</h1>", TestName = "Simple header tag")]
    [TestCase("# Заголовок __с _разными_ символами__\n", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
        TestName = "Header with strong and italic in strong")]
    [TestCase("#text\n", "#text\n", TestName = "Header without space before text")]
    [TestCase("# text", "<h1>text</h1>", TestName = "Header without new string symbol")]
    [TestCase("[text](text)", "<a href=\"text\">text</a>",
        TestName = "Simple link")]
    [TestCase("[]()", "<a href=\"\"></a>", TestName = "Empty link")]
    [TestCase("[_text_](text)", "<a href=\"text\"><em>text</em></a>", 
        TestName = "Em in link description")]
    [TestCase("[[(text)]](text)", "<a href=\"text\">[(text)]</a>",
        TestName = "Brackets in link")]
    [TestCase("_[text_](text)", "_<a href=\"text\">text_</a>",
        TestName = "Tags out of link")]
    [TestCase("[text](_text_)", "<a href=\"_text_\">text</a>",
        TestName = "Italic in link")]
    [TestCase("[text] (text)", "[text] (text)", TestName = "Space between link and description")]
    [TestCase("[text][text](text)", "[text]<a href=\"text\">text</a>",
        TestName = "Two link descriptions")]
    [TestCase("[text](text)(text)", "<a href=\"text\">text</a>(text)",
        TestName = "Two links")]
    [TestCase("[[text](text)]", "[<a href=\"text\">text</a>]", 
        TestName = "Link and description in additional brackets")]
    [TestCase("[text](text) text [text](text)", "<a href=\"text\">text</a> text <a href=\"text\">text</a>",
        TestName = "Some links in row")]
    [TestCase("# [text](text)\n", "<h1><a href=\"text\">text</a></h1>", 
        TestName = "Header with link")]
    public void Convert_DifferentText_ReturnsCorrectString(string text, string expected)
    {
        var result = sut.Convert(text);
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    [Timeout(10000)]
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
