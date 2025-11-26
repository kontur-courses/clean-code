using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;
namespace Markdown.Tests;

/// <summary>
/// Тесты экранирования обратным слешем
/// </summary>
public class EscapingTests
{
    private Md _markdown;

    [SetUp]
    public void Setup()
    {
        var lexer = new Lexer();
        var parser = new Parser();
        var renderer = new Renderer();
        _markdown = new Md(lexer, parser, renderer);
    }
    
    [TestCase(@"\_Вот это\_", "<p>_Вот это_</p>", 
        TestName = "Экранирование подчеркиваний")]
    [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", 
        @"<p>Здесь сим\волы экранирования\ \должны остаться.\</p>",
        TestName = "Экранирование без специальных символов остается")]
    [TestCase(@"\\_вот это будет выделено тегом_", 
        @"<p>\<em>вот это будет выделено тегом</em></p>",
        TestName = "Экранирование символа экранирования")]
    [TestCase("__Жирное с \\__ внутри__", "<p><strong>Жирное с __ внутри</strong></p>",
        TestName = "Экранирование двойного подчеркивания в полужирном")]
    [TestCase("_Привет\\_", "<p>_Привет_</p>",
        TestName = "Экранирование подчёркивания внутри курсива оставляет текст")]

    public void Test(string inputText, string expectedHtml)
    {
        var html = _markdown.Render(inputText);
        html.Should().Be(expectedHtml);
    }
}