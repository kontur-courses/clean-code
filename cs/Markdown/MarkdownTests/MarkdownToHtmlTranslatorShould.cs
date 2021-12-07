using System;
using FluentAssertions;
using Markdown.TagEvents;
using Markdown.TagParsers;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    public class MarkdownToHtmlTranslatorShould
    {
        [TestCase("# __Hello world!__\n", "<h1><strong>Hello world!</strong></h1>")]
        [TestCase("# __Hello world!_\n", "<h1>__Hello world!_</h1>")]
        [TestCase("# _Hello_ world!_\n", "<h1><em>Hello</em> world!_</h1>")]
        [TestCase("# __Hel_lo_ world!__\n", "<h1><strong>Hel<em>lo</em> world!</strong></h1>")]
        [TestCase("__123__", "<strong>123</strong>")]
        [TestCase("_1000_000_000_", "<em>1000_000_000</em>")]
        [TestCase("____", "<strong></strong>")]
        [TestCase("__Пересечение _не считается__ выделением_", "__Пересечение _не считается__ выделением_")]
        [TestCase("dif_ferent wo_rds", "dif_ferent wo_rds")]
        public void TranslateMarkdownCorrect(string inputMarkdown, string expectedHtml)
        {
            var tagEvents = new Taginizer(inputMarkdown).Taginize();
            new EscapingTagParser(tagEvents).Parse();
            new UnderlineTagParser(tagEvents, TagName.Underliner).Parse();
            new UnderlineTagParser(tagEvents, TagName.DoubleUnderliner).Parse();
            new UnderlineParserCorrector(tagEvents).Parse();
            new TagInteractionParser(tagEvents).Parse();

            var htmlResult = new MarkdownToHtmlTranslator(tagEvents).Translate();

            Console.WriteLine(htmlResult);
            htmlResult.Should().BeEquivalentTo(expectedHtml);
        }
    }
}
