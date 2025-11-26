using FluentAssertions;
using Markdown.Core.Lexing;
using Markdown.Core.Parsing;
using Markdown.Core.Rendering;
using NUnit.Framework;

namespace Markdown.Tests;

public class EmphasisTests
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
    
    [TestCase("_курсив_", "<p><em>курсив</em></p>", 
        TestName = "Курсив одинарными подчеркиваниями")]
    [TestCase("Текст, _окруженный с двух сторон_ одинарными символами", 
        "<p>Текст, <em>окруженный с двух сторон</em> одинарными символами</p>",
        TestName = "Курсив в середине текста")]
    [TestCase("Этот _подчерк _ работает", "<p>Этот <em>подчерк </em> работает</p>",
        TestName = "Одинарное выделение допускает пробел внутри")]
    public void Test(string inputText, string expectedHtml)
    {
        var html = _markdown.Render(inputText);
        html.Should().Be(expectedHtml);
    }
}
