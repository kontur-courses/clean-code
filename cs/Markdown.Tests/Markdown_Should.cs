using System;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Converter;
using Markdown.TokenSearcher;

namespace Markdown.Tests
{
    public class Markdown_Should
    {

        [TestCase(null, TestName = "Throw argument exception when string is null")]
        [TestCase("", TestName = "Throw argument exception when string is empty")]
        public void ThrowArgumentException_When_InvalidInputString(string markdownText)
        {
            Action action = () => 
            {
                var converter = new HtmlConverter();
                var tokenSearcher = new MarkdownTokenSearcher();
                var md = new Markdown(tokenSearcher, converter);
                var htmlText = md.Render(markdownText);
            };

            action.Should().Throw<ArgumentException>();
        }

        [TestCase("_Hello_", "<em>Hello</em>")]
        [TestCase("# Hello", "<h1>Hello</h1>")]
        [TestCase("__Hello__", "<strong>Hello</strong>")]

        [TestCase("_ Hello_", "_ Hello_")]
        [TestCase("_Hello _", "_Hello _")]
        [TestCase("__Hello __", "__Hello __")]
        [TestCase("__ Hello__", "__ Hello__")]
        [TestCase("#Hello", "#Hello")]

        [TestCase("_Hello", "_Hello")]
        [TestCase("Hello_", "Hello_")]
        [TestCase("__Hello", "__Hello")]
        [TestCase("Hello__", "Hello__")]
        [TestCase("Hello__", "Hello__")]

        [TestCase("_Hello_world_", "<em>Hello</em>world_")]
        [TestCase("_Hello_wor_ld_", "<em>Hello</em>wor<em>ld</em>")]
        [TestCase("_111_", "<em>111</em>")]
        [TestCase("1_1Hellow_", "1_1Hello_")]
        [TestCase("ра_зных сл_овах", "ра_зных сл_овах")]
        [TestCase("_разных сл_овах", "_разных сл_овах")]
        [TestCase("ра_зных словах_", "ра_зных словах_")]

        [TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._", "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>")]
        [TestCase("Hello1_2 word _ word_", "<em>Hello1_2 word _ word</em>")]

        [TestCase("__", "__")]
        [TestCase("____", "____")]
        [TestCase("__ __", "__ __")]

        [TestCase("__Hello__world__", "<strong>Hello</strong>world__")]
        [TestCase("__111__", "<strong>111</strong>")]
        [TestCase("ра__зных сл__овах", "ра__зных сл__овах")]
        [TestCase("__разных сл__овах", "__разных сл__овах")]
        [TestCase("ра__зных словах__", "ра__зных словах__")]
        [TestCase("__Hello world__", "<strong>Hello world</strong>")]
        [TestCase("__нач__але, и в сер__еди__не, и в кон__це.__", 
            "<strong>нач</strong>але, и в сер<strong>еди</strong>не, и в кон<strong>це.</strong>")]

        [TestCase("Внутри __двойн выдел _одина_ тоже__ раб","Внутри <strong>двойн выдел <em>одина</em> тоже</strong> раб")]
        [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_")]
        [TestCase("Внутри _одинарного __двойное__ не_ работает", "Внутри <em>одинарного __двойное__ не</em> работает")]
        [TestCase("__Непарные_ символы", "__Непарные_ символы")]

        [TestCase(@"\Hello\", @"\Hello\")]
        [TestCase(@"\_Hello_", @"_Hello_")]
        [TestCase(@"\__Hello__", @"__Hello__")]
        [TestCase(@"\\_Hello_", @"\<em>Hello</em>")]
        [TestCase(@"\\__Hello__", @"\<strong>Hello</strong>")]
        [TestCase(@"\# Hello", @"# Hello")]
        public void Correct_Render_When_ValidInputString(string markdownText, string htmlText)
        {
            var converter = new HtmlConverter();
            var tokenSearcher = new MarkdownTokenSearcher();
            var md = new Markdown(tokenSearcher, converter);
            md.Render(markdownText).Should().Be(htmlText);
        }
    }
}
