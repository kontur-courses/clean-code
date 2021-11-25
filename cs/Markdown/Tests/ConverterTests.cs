using FluentAssertions;
using Markdown.TagStore;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ConverterTests
    {
        [TestCase("_Lorem _ipsum dolor_ sit_ amet", "<em>Lorem <em>ipsum dolor</em> sit</em> amet",
            TestName = "Two nested paired tags")]
        [TestCase("Lorem _ipsum dolor_ sit_ amet", "Lorem <em>ipsum dolor</em> sit_ amet",
            TestName = "One paired tag and one unnecessary")]
        [TestCase("Lorem _ipsum_ _dolor_ sit amet", "Lorem <em>ipsum</em> <em>dolor</em> sit amet",
            TestName = "Two consecutive paired tags")]
        [TestCase("__Lorem__", "<strong>Lorem</strong>", TestName = "Strong tag")]
        [TestCase("\\_Lorem_", "_Lorem_", TestName = "Escaped tag")]
        [TestCase("\\_L\\orem_", "_L\\orem_", TestName = "Should not escape if not tag after slash")]
        [TestCase("\\\\_Lorem_", "\\<em>Lorem</em>", TestName = "Should not escape tag if double slash")]
        [TestCase("A __Lorem _ipsum dolor_ sit__ amet", "A <strong>Lorem <em>ipsum dolor</em> sit</strong> amet",
            TestName = "Emphasized tag into strong")]
        [TestCase("A _Lorem __ipsum dolor__ sit_ amet", "A <em>Lorem __ipsum dolor__ sit</em> amet",
            TestName = "Strong tag into emphasized shouldn't be converted")]
        [TestCase("Подчерки внутри текста c цифрами_12_3 не считаются выделением",
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением",
            TestName = "Text with numbers shouldn't be converted")]
        [TestCase("в _нач_але", "в <em>нач</em>але", TestName = "Convert start of word")]
        [TestCase("в сер_еди_не", "в сер<em>еди</em>не", TestName = "Convert middle of word")]
        [TestCase("в кон_це._", "в кон<em>це.</em>", TestName = "Convert end of word")]
        [TestCase("выделение в ра_зных сл_овах", "выделение в ра_зных сл_овах",
            TestName = "Tags inside different words")]
        [TestCase("__Непарные_ символы в рамках одного абзаца", "__Непарные_ символы в рамках одного абзаца",
            TestName = "Unpaired tags in one paragraph isn't tags")]
        [TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением",
            TestName = "After opening tag shouldn't be whitespace")]
        [TestCase("эти _подчерки _не считаются окончанием выделения",
            "эти _подчерки _не считаются окончанием выделения",
            TestName = "Before closing tag shouldn't be whitespace")]
        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",
            TestName = "Intersecting tags shouldn't be converted")]
        [TestCase("Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
            TestName = "Tags without text ")]
        [TestCase("#Lorem ipsum dolor\r\n sit amet",
            "<h1>Lorem ipsum dolor</h1> sit amet",
            TestName = "Header tag")]
        [TestCase("#Заголовок __с _разными_ символами__\n",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>",
            TestName = "Header tag")]
        public void Convert_ShouldReplaceMdTagToHtml(string originalText, string convertedText)
        {
            var from = new MdTagStore();
            var to = new HtmlTagStore();
            var sut = new Converter(from, to);

            var converted = sut.Convert(originalText);

            converted.Should().Be(convertedText);
        }
    }
}