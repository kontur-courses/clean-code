using FluentAssertions;
using FluentAssertions.Extensions;
using MarkdownTask.MarkdownParsers;
using NUnit.Framework;

namespace MarkdownTask.Tests
{
    [TestFixture]
    public class MarkdownTests
    {
        [TestCase("Simple text", "Simple text", TestName = "Parse text without tags")]
        [TestCase("_Italic text_", "<em>Italic text</em>", TestName = "Parse _ tag")]
        [TestCase("__Halfbold text__", "<strong>Halfbold text</strong>", TestName = "Parse __ tag")]
        [TestCase("# This is a header", "<h1>This is a header</h1>", TestName = "Parse '# ' tag")]
        [TestCase("[Google](https://google.com)", "<a href=Google>https://google.com)</a>", TestName = "ParseLinkTag")]
        [TestCase("", "", TestName = "Parse empty string")]
        [TestCase("# Header __with _different_ tags__ [Google](https://google.com)", "<h1>Header <strong>with <em>different</em> tags</strong> <a href=Google>https://google.com)</a></h1>", TestName = "All tags together")]
        [TestCase(@"\_Вот это\_, не должно выделиться тегом <em>", @"_Вот это_, не должно выделиться тегом <em>", TestName = "Сharacter escaping")]
        [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", @"Здесь сим\волы экранирования\ \должны остаться.\", TestName = "Character escaping stay if escaspe nothing")]
        [TestCase(@"\\_вот это будет выделено тегом_ ", @"\<em>вот это будет выделено тегом</em> ", TestName = "Escaping escape char")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает", @"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает", TestName = "Italic work inside strong")]
        [TestCase("внутри _одинарного __двойное__ не_ работает", "внутри <em>одинарного __двойное__ не</em> работает", TestName = "Strong not work inside italic")]
        [TestCase("c цифрами_12_3 не считаются выделением", "c цифрами_12_3 не считаются выделением", TestName = "Italic not work with digits")]
        [TestCase("в _нач_але, и в сер_еди_не, и в кон_це._", "в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>", TestName = "Selection work in single word")]
        [TestCase("в ра_зных сл_овах", "в ра_зных сл_овах", TestName = "Selection not work in different words")]
        [TestCase("эти_ подчерки_ не считаются выделением", "эти_ подчерки_ не считаются выделением", TestName = "Opening tag should be before nonspace char")]
        [TestCase("эти _подчерки _ не считаются выделением", "эти _подчерки _ не считаются выделением", TestName = "Closing tag should be after nonspace char")]
        [TestCase("случае __пересечения _двойных__ и одинарных_ подчерков", "случае __пересечения _двойных__ и одинарных_ подчерков", TestName = "Tags intersection are not allowed")]
        [TestCase("____", "____", TestName = "Tags without text not parsed")]
        [TestCase("[Goo_gle](https://goog_le.com)", "<a href=Goo_gle>https://goog_le.com)</a>", TestName = "Tags inside link markdown are ignored")]
        [TestCase("_[Google](https://google.com)_", "<em><a href=Google>https://google.com)</a></em>", TestName = "Tags works outside link markdown")]
        public void Markdown_SimpleTags_ParsedCorrectly(string markdownString, string htmlString)
        {
            var md = new Markdown(new ITagParser[]{
                new HeaderTagParser(),
                new ItalicTagParser(),
                new StrongTagParser(),
                new EscapingParsing(),
                new LinkTagParser()
            });

            md.Render(markdownString).Should().Be(htmlString);
        }

        [TestCase(500, TestName = "Markdown parsing should be fast")]
        public void Markdown_Parsing_ShouldBeFast(int wordsCount)
        {
            var textBlocks = new string[] { "text", "__ strong__", "_italic_" };

            var text = new List<string>();
            var rnd = new Random();
            for (int i = 0; i < wordsCount; i++)
            {
                text.Add(textBlocks[rnd.Next(3)]);
            }

            var md = new Markdown(new ITagParser[]
            {
                new HeaderTagParser(),
                new ItalicTagParser(),
                new StrongTagParser()
            });

            md.ExecutionTimeOf(s => s.Render(string.Join(" ", text))).Should().BeLessThanOrEqualTo(500.Milliseconds());
        }
    }
}
