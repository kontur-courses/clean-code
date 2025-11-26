using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class TextTests
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

    [TestCase("Абракадабра", "<p>Абракадабра</p>",
        TestName = "<p>Простой текст без выделений</p>")]
    [TestCase("Привет, как дела?", "<p>Привет, как дела?</p>",
        TestName = "<p>Простой текст без выделений</p>")]
    [TestCase("Email: test@example.com", "<p>Email: test@example.com</p>",
        TestName = "<p>Текст с различными символами email</p>")]
    [TestCase("Ссылка: https://example.com", "<p>Ссылка: https://example.com</p>", 
        TestName = "Текст с URL")]
    [TestCase("Первый параграф\nВторой параграф", "<p>Первый параграф</p><p>Второй параграф</p>", 
        TestName = "Два параграфа")]
    [TestCase("Первый\n\nТретий", "<p>Первый</p><p>Третий</p>", 
        TestName = "Параграфы с пустой строкой")]
    
    public void Test(string inputText, string expectedText)
    {
        
        var html = _markdown.Render(inputText);
        
        html.Should().Be(expectedText);
    }
    
        
    
    
}