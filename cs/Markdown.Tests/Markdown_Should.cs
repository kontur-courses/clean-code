using System;
using System.Diagnostics;
using FluentAssertions;
using NUnit.Framework;
using Markdown.Converter;
using Markdown.TokenSearcher;


namespace Markdown.Tests
{
    public class Markdown_Should
    {
        private Markdown mdProcessor;

        [SetUp]
        public void Init()
        {
            var converter = new HtmlConverter();
            var tokenSearcher = new MarkdownTokenSearcher();
            mdProcessor = new Markdown(tokenSearcher, converter);
        }

        [TestCase(null, TestName = "Throw argument exception when string is null")]
        [TestCase("", TestName = "Throw argument exception when string is empty")]
        public void ThrowArgumentException_When_InvalidInputString(string markdownText)
        {
            Action action = () => 
            {
                var htmlText = mdProcessor.Render(markdownText);
            };

            action.Should().Throw<ArgumentException>();
        }

        [TestCase("_Hello_", "<em>Hello</em>", 
            TestName = "Correct conversion of a italic tags")]
        [TestCase("# Hello", "<h1> Hello</h1>", 
            TestName = "Correct conversion of a header tags")]
        [TestCase("__Hello__", "<strong>Hello</strong>", 
            TestName = "Correct conversion of bold tags")]

        [TestCase("_ Hello_", "_ Hello_", 
            TestName = "Correct conversion when there is a space after the opening italic tag")]
        [TestCase("_Hello _", "_Hello _", 
            TestName = "Correct conversion if there is a space before the closing italic tag")]
        [TestCase("__Hello __", "__Hello __", 
            TestName = "Correct conversion if there is a space before the closing bold tag")]
        [TestCase("__ Hello__", "__ Hello__",
            TestName = "Correct conversion when there is a space after the opening bold tag")]
        [TestCase("#Hello", "#Hello", 
            TestName = "Correct conversion if there is no space after the header tag")]

        [TestCase("_Hello", "_Hello", 
            TestName = "Correct conversion if there is no closing italic tag")]
        [TestCase("Hello_", "Hello_", 
            TestName = "Correct conversion if there is no opening italic tag")]
        [TestCase("__Hello", "__Hello", 
            TestName = "Correct conversion if there is no closing bold tag")]
        [TestCase("Hello__", "Hello__", 
            TestName = "Correct conversion if there is no opening bold tag")]

        [TestCase("_Hello_world_", "<em>Hello</em>world_", 
            TestName = "Correct conversion if there are several italic tags and there are extra ones")]
        [TestCase("_Hello_wor_ld_", "<em>Hello</em>wor<em>ld</em>", 
            TestName = "Correct conversion if there are several tags in italics and all are working")]

        [TestCase("ра_зных сл_овах", "ра_зных сл_овах", 
            TestName = "No highlighting if the italic tags are in different words")]
        [TestCase("_разных сл_овах", "_разных сл_овах", 
            TestName = "No highlighting if the italic tags are in different words")]
        [TestCase("ра_зных словах_", "ра_зных словах_", 
            TestName = "No highlighting if the  italic tags are in different words")]
        [TestCase("ра__зных сл__овах", "ра__зных сл__овах", 
            TestName = "No highlighting if the bold tags are in different words")]
        [TestCase("__разных сл__овах", "__разных сл__овах", 
            TestName = "No highlighting if the bold tags are in different words")]
        [TestCase("ра__зных словах__", "ра__зных словах__", 
            TestName = "No highlighting if the bold tags are in different words")]

        [TestCase("_окруженный с двух сторон_", "<em>окруженный с двух сторон</em>", 
            TestName = "Correct tagging there are several words highlighted")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._", "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>",
            TestName = "Correct highlighting if italic tags highlight part of a word")]
        [TestCase("__нач__але, и в сер__еди__не, и в кон__це.__",
            "<strong>нач</strong>але, и в сер<strong>еди</strong>не, и в кон<strong>це.</strong>",
             TestName = "Correct highlighting if bold tags highlight part of a word")]
        [TestCase("Hello1_2 word_", "Hello1_2 word_", 
            TestName = "The correct definition of the handwriting of the numbers inside the word")]

        [TestCase("__", "__",
            TestName = "Don't convert tags if there is nothing inside them")]
        [TestCase("____", "____", 
            TestName = "Don't convert tags if there is nothing inside them")]
        [TestCase("__ __", "__ __", 
            TestName = "Don't convert tags if there is only a space inside them")]

        [TestCase("__Hello__world__", "<strong>Hello</strong>world__", 
            TestName = "Correct conversion if there are several bold tags and there are extra ones")]
        [TestCase("_111_", "<em>111</em>", 
            TestName = "Correct italic tagging of a number")]
        [TestCase("__111__", "<strong>111</strong>", 
            TestName = "Correct bold tagging of a number")]
        [TestCase("__Hello world__", "<strong>Hello world</strong>", 
            TestName = "Correct conversion if bold tags highlight multiple words")]

        [TestCase("Внутри __двойн выдел _одина_ тоже__ раб","Внутри <strong>двойн выдел <em>одина</em> тоже</strong> раб",
            TestName = "Correct converting if there are italic tags inside bold tags")]
        [TestCase("Внутри _одинарного __двойное__ не_ работает", "Внутри <em>одинарного __двойное__ не</em> работает",
            TestName = "Correct converting if there are bold tags inside italic tags")]
        [TestCase("__пересечения _двойных__ и одинарных_", "__пересечения _двойных__ и одинарных_", 
            TestName = "Correct conversion if there is an intersection of tags")]
        [TestCase("__Непарные_ символы", "__Непарные_ символы", 
            TestName = "Correct conversion of unpaired tags")]

        [TestCase(@"\Hello\", @"\Hello\", 
            TestName = "Correct conversion if the escape symbol does not escape anything")]
        [TestCase(@"\_Hello_", @"_Hello_", 
            TestName = "Correct conversion if the escaping character escapes the italic tag")]
        [TestCase(@"\__Hello__", @"__Hello__", 
            TestName = "Correct conversion if the escaping character escapes the bold tag")]
        [TestCase(@"\\_Hello_", @"\<em>Hello</em>", 
            TestName = "Correct conversion if the escaping character escapes another escaping character")]
        [TestCase(@"\# Hello", @"# Hello", 
            TestName = "Correct conversion if the escaping character escapes the header tag")]
        public void Correct_Render_When_ValidInputString(string markdownText, string htmlText)
        {
            mdProcessor.Render(markdownText).Should().Be(htmlText);
        }

        [Test]
        public void RenderForLinearTimeComplexity()
        {
            var mdTestText = "# Перед образом __горит__ _зеленая_ лампадка\n" +
                "_через __всю комнату__ от угла до угла_ __тянется веревка__\n" +
                "# __на которой висят__ пеленки _и большие_ черные панталоны";
            var linearCoefficient = 2;
            var timeMeter = new Stopwatch();
            timeMeter.Start();
            mdProcessor.Render(mdTestText);
            timeMeter.Stop();
            var previous = timeMeter.ElapsedTicks;
            for (var i = 0; i < 5; i++)
            {
                mdTestText += mdTestText;
                timeMeter.Restart();
                mdProcessor.Render(mdTestText);
                timeMeter.Stop();

                Assert.That(timeMeter.ElapsedTicks / previous, Is.LessThanOrEqualTo(linearCoefficient));
                previous = timeMeter.ElapsedTicks;
            }
        }
    }
}
