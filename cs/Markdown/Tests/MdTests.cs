using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{
    [TestCase("![Cat](https://example.com/image.jpg)",
        "<img src =\"https://example.com/image.jpg\" alt=\"Cat\">", TestName = "ValidUrlAndAlt")]
    [TestCase("!\n[Cat](https://example.com/image.jpg)",
        "!\n[Cat](https://example.com/image.jpg)", TestName = "NewlineAfterExclamation")]
    [TestCase("![Ca\nt](https://example.com/image.jpg)",
        "![Ca\nt](https://example.com/image.jpg)", TestName = "AltWithNewline")]
    [TestCase("![](https://example.com/image.jpg)",
        "<img src =\"https://example.com/image.jpg\" alt=\"\">", TestName = "EmptyAltText")]
    [TestCase("![__Cat__](https://example.com/image.jpg)",
        "<img src =\"https://example.com/image.jpg\" alt=\"<strong>Cat</strong>\">", TestName = "AltWithBold")]
    [TestCase("![Cat](https://example.com/i__m__age.jpg)",
        "<img src =\"https://example.com/i__m__age.jpg\" alt=\"Cat\">", TestName = "UrlWithBold")]
    public void Render_Image_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }

    [TestCase("# текст", "<h1>текст</h1>", TestName = "SimpleHeader")]
    [TestCase("# Первый \n# Второй", "<h1>Первый </h1>\n<h1>Второй</h1>", TestName = "TwoHeadersInDifferentLines")]
    [TestCase("# Первый \n просто текст \n# Второй", "<h1>Первый </h1>\n просто текст \n<h1>Второй</h1>",
        TestName = "HeadersWithTextBetween")]
    [TestCase("#текст", "#текст", TestName = "WithNoSpace")]
    [TestCase("# Заголовок с _курсивом_ и __жирным шрифтом__",
        "<h1>Заголовок с <em>курсивом</em> и <strong>жирным шрифтом</strong></h1>", TestName = "WithInlineFormatting")]
    [TestCase("# ![Cat](https://example.com/image.jpg)",
        "<h1><img src =\"https://example.com/image.jpg\" alt=\"Cat\"></h1>", TestName = "WithImage")]
    [TestCase("# __текст__", "<h1><strong>текст</strong></h1>", TestName = "WithBold")]
    [TestCase(" # текст", " # текст", TestName = "LeadingSpace")]
    [TestCase("# ", "<h1></h1>", TestName = "Empty")]
    [TestCase("# текст # ", "<h1>текст # </h1>", TestName = "TrailingHashIgnored")]
    [TestCase("#   текст", "<h1>  текст</h1>", TestName = "ManySpaceAfterHash")]
    public void Render_Header_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }

    [TestCase("_текст_", "<em>текст</em>", TestName = "SimpleItalic")]
    [TestCase("_те\nкст_", "_те\nкст_", TestName = "ItalicWithNewLineInside")]
    [TestCase("__текст__", "<strong>текст</strong>", TestName = "SimpleBold")]
    [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает",
        "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает",
        TestName = "BoldWithItalicInside")]
    [TestCase("_те_кст", "<em>те</em>кст", TestName = "ItalicAtStartWord")]
    [TestCase("т_екс_т", "т<em>екс</em>т", TestName = "ItalicInMiddleWord")]
    [TestCase("текс_т_", "текс<em>т</em>", TestName = "ItalicAtEndWord")]
    [TestCase("__текст_", "__текст_", TestName = "UnpairedUnderscores")]
    [TestCase("_ текст_", "_ текст_", TestName = "LeadingSpacePreventsItalic")]
    [TestCase("1_2_3", "1_2_3", TestName = "DigitsPreventItalic")]
    [TestCase("те_кст те_кст", "те_кст те_кст", TestName = "SeparateWordsUnderscores")]
    [TestCase("__", "__", TestName = "OnlyDoubleUnderscore")]
    [TestCase("____", "____", TestName = "OnlyFourUnderscores")]
    [TestCase("__текст _текст__ текст_", "__текст _текст__ текст_", TestName = "BoldWithNestedItalic")]
    [TestCase("_текст __текст__ текст_", "<em>текст __текст__ текст</em>", TestName = "ItalicWithBoldInside")]
    [TestCase("_текст т__екс__т текст_", "<em>текст т__екс__т текст</em>", TestName = "ItalicWithBoldInsideWord")]
    [TestCase("т__е_к_с__т", "т<strong>е<em>к</em>с</strong>т", TestName = "BoldInWordWithItalicInside")]
    [TestCase("__те_к_ст__", "<strong>те<em>к</em>ст</strong>", TestName = "BoldWithItalicInsideInWord")]
    [TestCase("_те__к__ст_", "<em>те__к__ст</em>", TestName = "ItalicWithBoldInsideInWord")]
    [TestCase("_123_", "<em>123</em>", TestName = "ItalicWithNumbers")]
    [TestCase("__123__", "<strong>123</strong>", TestName = "BoldWithNumbers")]
    [TestCase("__++++__", "<strong>++++</strong>", TestName = "BoldWithSymbols")]
    [TestCase("__ текст __", "__ текст __", TestName = "BoldWithSpacesPrevents")]
    [TestCase("__текст __", "__текст __", TestName = "BoldWithSpace")]
    public void Render_Underscores_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }

    [TestCase(@"\_текст_", "_текст_", TestName = "EscapedUnderscore")]
    [TestCase(@"\\_текст\\_", @"\<em>текст\</em>", TestName = "DoubleEscapedUnderscoresAroundText")]
    [TestCase(@"\# текст", "# текст", TestName = "EscapedHeader")]
    [TestCase(@"\![Cat](https://example.com/image.jpg)",
        "![Cat](https://example.com/image.jpg)", TestName = "EscapedExclamationInImage")]
    [TestCase(@"__текст \_текст__ текст_", "<strong>текст _текст</strong> текст_",
        TestName = "BoldWithEscapedUnderscoreInside")]
    [TestCase(@"т\екс\т", @"т\екс\т", TestName = "TextWithMultipleEscapes")]
    public void Render_Escape_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }


}