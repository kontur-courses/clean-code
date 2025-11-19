using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class MdTests
{   
    [TestCase("![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"Cat\">", TestName = "ValidUrlAndAlt")]
    [TestCase("!\n[Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "!\n[Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "NewlineAfterExclamation")]   
    [TestCase("![Ca\nt](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Ca\nt](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "AltWithNewline")] 
    [TestCase("![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6\na5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6\na5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "UrlWithNewline")]
    [TestCase("![C[]at](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"C[]at\">", TestName = "AltWithBrackets")]
    [TestCase("![C[at](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![C[at](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "UnclosedBracketInAlt")]
    [TestCase("![C]at](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![C]at](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "ExtraClosingBracketInAlt")]
    [TestCase("![Cat](https://i.pinimg.com/originals()/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals()/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"Cat\">", TestName = "UrlWithEmptyParentheses")]
    [TestCase("![Cat](https://i.pinimg.com/originals(/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals(/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "UrlWithOneOpenParentheses")]
    [TestCase("![Cat](https://i.pinimg.com/originals)/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals)/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "UrlWithOneCloseParentheses")]
    [TestCase("![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c7 6c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c7 6c038ef0c8d2502fd2f6.jpg)", TestName = "UrlWithSpace")]
    [TestCase("![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.qqq)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.qqq)", TestName = "InvalidImageFormat")]
    [TestCase("![](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"\">", TestName = "EmptyAltText")]
    [TestCase("![Cat]()", "![Cat]()", TestName = "EmptyUrl")]
    [TestCase("![Cat](Cat)", "![Cat](Cat)", TestName = "InvalidUrl")]
    [TestCase("![__Cat__](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"<strong>Cat</strong>\">", TestName = "AltWithBold")]
    [TestCase("![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a__5b2__c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a__5b2__c76c038ef0c8d2502fd2f6.jpg\" alt=\"Cat\">", TestName = "UrlWithBold")]
    [TestCase("![[][][]](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"[][][]\">", TestName = "AltWithMultipleBrackets")]
    public void Render_Image_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }
    
    [TestCase("# текст", "<h1>текст</h1>", TestName = "SimpleHeader")]
    [TestCase("# Первый \n# Второй", "<h1>Первый </h1>\n<h1>Второй</h1>", TestName = "TwoHeadersInDifferentLines")]
    [TestCase("# Первый \n просто текст \n# Второй", "<h1>Первый </h1>\n просто текст \n<h1>Второй</h1>", TestName = "HeadersWithTextBetween")]
    [TestCase("#текст", "#текст", TestName = "WithNoSpace")]
    [TestCase("# Заголовок с _курсивом_ и __жирным шрифтом__", "<h1>Заголовок с <em>курсивом</em> и <strong>жирным шрифтом</strong></h1>", TestName = "WithInlineFormatting")]
    [TestCase("# ![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "<h1><img src =\"https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg\" alt=\"Cat\"></h1>", TestName = "WithImage")]
    [TestCase("# __текст__", "<h1><strong>текст</strong></h1>", TestName = "WithBold")]
    [TestCase(" # текст", " # текст", TestName = "LeadingSpace")]
    [TestCase("# ", "<h1></h1>", TestName = "Empty")]
    [TestCase("# текст # ", "<h1>текст # </h1>", TestName = "TrailingHashIgnored")]
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
    [TestCase("_ текст_","_ текст_", TestName = "LeadingSpacePreventsItalic")]
    [TestCase("1_2_3","1_2_3", TestName = "DigitsPreventItalic")]
    [TestCase("те_кст те_кст","те_кст те_кст", TestName = "SeparateWordsUnderscores")]
    [TestCase("_текст _текст", "_текст _текст",TestName = "UnclosedItalicMultipleWords")]
    [TestCase("__","__", TestName = "OnlyDoubleUnderscore")]
    [TestCase("____","____", TestName = "OnlyFourUnderscores")]
    [TestCase("__текст _текст__ текст_","__текст _текст__ текст_", TestName = "BoldWithNestedItalic")]
    [TestCase("_текст __текст__ текст_","<em>текст __текст__ текст</em>", TestName = "ItalicWithBoldInside")]
    [TestCase("_текст т__екс__т текст_","<em>текст т__екс__т текст</em>", TestName = "ItalicWithBoldInsideWord")]
    [TestCase("__текст т_екст текст___","<strong>текст т_екст текст</strong>_", TestName = "BoldWithTrailingUnderscore")]
    [TestCase("т__е_к_с__т","т<strong>е<em>к</em>с</strong>т", TestName = "BoldInWordWithItalicInside")]
    [TestCase("__те_к_ст__","<strong>те<em>к</em>ст</strong>", TestName = "BoldWithItalicInsideInWord")]
    [TestCase("_те__к__ст_","<em>те__к__ст</em>", TestName = "ItalicWithBoldInsideInWord")]
    [TestCase("_123_","<em>123</em>", TestName = "ItalicWithNumbers")]
    [TestCase("__123__","<strong>123</strong>", TestName = "BoldWithNumbers")]
    [TestCase("__++++__","<strong>++++</strong>", TestName = "BoldWithSymbols")]
    [TestCase("__текст___текст_","<strong>текст</strong><em>текст</em>", TestName = "BoldThenItalic")]
    [TestCase("__ текст __","__ текст __", TestName = "BoldWithSpacesPrevents")]
    [TestCase("__текст __","__текст __", TestName = "BoldWithSpace")]
    [TestCase("тек__с_т те_кс__т","тек__с_т те_кс__т", TestName = "UnclosedUnderscoresInDifferentWords")]
    public void Render_Underscores_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }
    
    [TestCase(@"\_текст_", "_текст_", TestName = "EscapedUnderscore")]
    [TestCase(@"\\_текст\\_", @"\<em>текст\</em>", TestName = "DoubleEscapedUnderscoresAroundText")]
    [TestCase(@"\# текст", "# текст", TestName = "EscapedHeader")]
    [TestCase(@"\![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "EscapedExclamationInImage")] 
    [TestCase(@"!\[Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "EscapedAltInImage")]
    [TestCase(@"![Cat]\(https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)",
        "![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg)", TestName = "EscapedUrlInImage")] 
    [TestCase(@"__текст \_текст__ текст_","<strong>текст _текст</strong> текст_", TestName = "BoldWithEscapedUnderscoreInside")]
    [TestCase(@"т\екс\т",@"т\екс\т", TestName = "TextWithMultipleEscapes")]
    public void Render_Escape_CorrectHtmlText(string markdownText, string expectedResult)
    {
        var renderer = new Md();
        var html = renderer.Render(markdownText);

        html.Should().Be(expectedResult);
    }
    
    [TestCase(1000, TestName = "Parse_1000_repeat")]
    [TestCase(2000, TestName = "Parse_2000_repeats")]
    [TestCase(4000, TestName = "Parse_4000_repeats")]
    [TestCase(8000, TestName = "Parse_8000_repeats")]
    [TestCase(16000, TestName = "Parse_16000_repeats")]
    public void Render_Performance_WithAllTokens(int repeatCount)
    {
        var markdownText = "# Заголовок _курсив_ __жирный__ ![Cat](https://i.pinimg.com/originals/f5/ef/a6/f5efa6a5b2c76c038ef0c8d2502fd2f6.jpg) текст \\экранирование_\n";
        var sb = new StringBuilder();
        for (int i = 0; i < repeatCount; i++)
        {
            sb.Append(markdownText);
        }

        var input = sb.ToString();  
        var stopwatch = Stopwatch.StartNew();
        var renderer = new Md();
        var result = renderer.Render(input);
        stopwatch.Stop();

        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} ms");
    }
}