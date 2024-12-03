using FluentAssertions;

namespace Markdown.Tests;

[TestFixture]
public class MdTest
{
    [TestCase("\\__это не курсив__", "__это не курсив__")]
    [TestCase("\\__это__ курсив", "_<em>это</em>_ курсив")]
    [TestCase("в ра_зных\tсл_овах", "в ра_зных\tсл_овах")]
    [TestCase("в ра_зных\nсл_овах", "в ра_зных\nсл_овах")]
    [TestCase("_в разных сл_овах", "_в разных сл_овах")]
    [TestCase("_Кажется, вот\nэто_ не выполняется", "_Кажется, вот\nэто_ не выполняется")]
    [TestCase("[text](link)", "<a href=link>text</a>")]
    [TestCase("[3311", "[3311")]
    [TestCase(@"_e\\_", @"<em>e\</em>")]
    [TestCase("в_нутр_и слова", "в<em>нутр</em>и слова")]
    [TestCase("__Непарные_ символы", "__Непарные_ символы")]
    [TestCase("# Hello, world!", "<h1>Hello, world!</h1>")]
    [TestCase("## Hello, world!", "<h2>Hello, world!</h2>")]
    [TestCase("### Hello, world!", "<h3>Hello, world!</h3>")]
    [TestCase("#### Hello, world!", "<h4>Hello, world!</h4>")]
    [TestCase("##### Hello, world!", "<h5>Hello, world!</h5>")]
    [TestCase("###### Hello, world!", "<h6>Hello, world!</h6>")]
    [TestCase("_Hello, world!_", "<em>Hello, world!</em>")]
    [TestCase("Hello, world!", "Hello, world!")]
    [TestCase("__Hello, world!__", "<strong>Hello, world!</strong>")]
    [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает", "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает")]
    [TestCase("_12_3", "_12_3")]
    [TestCase("в ра_зных сл_овах", "в ра_зных сл_овах")]
    [TestCase("____,", "____,")]
    [TestCase("_ подчерки_", "_ подчерки_")]
    
    public void Render_ShouldReturnHTMLString(string input, string expected)
    {
        var md = new Md();
        var result = md.Render(input);
        result.Should().Be(expected);
    }
}