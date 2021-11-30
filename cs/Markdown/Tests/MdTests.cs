using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown.Common;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        private Md mdParser;
        private (string Input, string Expected, string Actual) parseData;

        [SetUp]
        public void SetUp()
        {
            mdParser = new Md();
        }

        [TestCaseSource(typeof(TestsData), nameof(TestsData.ParseTagsTests))]
        [TestCaseSource(typeof(TestsData), nameof(TestsData.MarkdownSpecificationTests))]
        public void Parse_ShouldWorksCorrectly(string input, string expected)
        {
            parseData = (input, expected, mdParser.Render(input));
            parseData.Actual.Should().Be(expected);
        }
        
        [TearDown]
        public void TearDown()
        {
            Console.WriteLine($"Input:\r\n{parseData.Input}\r\n");
            Console.WriteLine($"Expected:\r\n{parseData.Expected}\r\n");
            Console.WriteLine($"Actual:\r\n{parseData.Actual}\r\n");
        }
        
        private class TestsData
        {
            public static IEnumerable<TestCaseData> ParseTagsTests
            {
                get
                {
                    yield return new TestCaseData(
                            "# Заголовок\r\n#Заголовок", "<h1> Заголовок</h1>\r\n<h1>Заголовок</h1>")
                        .SetName("Render_ShouldParseHeaderTag")
                        .SetCategory(nameof(ParseTagsTests));
                    yield return new TestCaseData(
                            "_курсивный текст_\r\n_курсивный текст_",
                            "<em>курсивный текст</em>\r\n<em>курсивный текст</em>")
                        .SetName("Render_ShouldParseEmphasisTag")
                        .SetCategory(nameof(ParseTagsTests));
                    yield return new TestCaseData(
                            "__полужирный текст__\r\n__полужирный текст__",
                            "<strong>полужирный текст</strong>\r\n<strong>полужирный текст</strong>")
                        .SetName("Render_ShouldParseStrongTag")
                        .SetCategory(nameof(ParseTagsTests));
                    yield return new TestCaseData(
                            "\\#Это не заголовок первого уровня\r\nЭто \\_не курсив,\\_ а это \\__не полужирный\\__\r\nЭто _пров\\ерка\\\\_ на __экранирование\\\\__ тегов\\\\",
                            "#Это не заголовок первого уровня\r\nЭто _не курсив,_ а это __не полужирный__\r\nЭто <em>пров\\ерка\\</em> на <strong>экранирование\\</strong> тегов\\")
                        .SetName("Render_ShouldParseBackslashTag")
                        .SetCategory(nameof(ParseTagsTests));
                    yield return new TestCaseData(
                            "# Заголовок c _курсивным текстом_ и __полужирным текстом__",
                            "<h1> Заголовок c <em>курсивным текстом</em> и <strong>полужирным текстом</strong></h1>")
                        .SetName("Render_ShouldParseAllTagsInHeader")
                        .SetCategory(nameof(ParseTagsTests));
                }
            }

            public static IEnumerable<TestCaseData> MarkdownSpecificationTests
            {
                get
                {
                    yield return new TestCaseData(
                            "#Это заголовок, а это (#) - нет.\r\nИ это #тоже# не ##заголовок##",
                            "<h1>Это заголовок, а это (#) - нет.</h1>\r\nИ это #тоже# не ##заголовок##")
                        .SetName("Render_ShouldIgnoreBlockTags_WhenNotStartsWithNewLine")
                        .SetCategory("BasicSpecification");
                    yield return new TestCaseData(
                            "Внутри _одинарного __двойное__ не работает_\r\nВнутри __двойного _одинарное_ работает__",
                            "Внутри <em>одинарного __двойное__ не работает</em>\r\nВнутри <strong>двойного <em>одинарное</em> работает</strong>")
                        .SetName("Render_ShouldIgnoreSpanTag_WhenParentTagLengthIsGreater")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "Это _заголовок1_ ,а не заголовок __1 уровня__\r\n_4 Life CJ, _Grove __123__ Street_ 4 Life_",
                            "Это _заголовок1_ ,а не заголовок __1 уровня__\r\n_4 Life CJ, <em>Grove __123__ Street</em> 4 Life_")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenPlacedWithNumbers")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "Подчерки _мо_гут вы__де__лять ча_сть_ слова\r\nНо в разных словах не могут",
                            "Подчерки <em>мо</em>гут вы<strong>де</strong>лять ча<em>сть</em> слова\r\nНо в разных словах не могут")
                        .SetName("Render_ShouldParseSpanTags_WhenPartOfWordMarked")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "Подчерки могут выделять часть слова\r\nНо в раз_ных сло_вах н__е мог__ут",
                            "Подчерки могут выделять часть слова\r\nНо в раз_ных сло_вах н__е мог__ут")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenPartsOfDifferentWordsMarked")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "За подчерками, _начинающими выделение,_ должен следовать __непробельный символ__\r\nИначе_ ничего_ не__ получится!__",
                            "За подчерками, <em>начинающими выделение,</em> должен следовать <strong>непробельный символ</strong>\r\nИначе_ ничего_ не__ получится!__")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenMarkedWordsStartsWithNonWhitespace")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "Подчерки, _заканчивающие выделение,_ должны следовать за __непробельным символом__\r\n_Иначе _ничего __не получится __!",
                            "Подчерки, <em>заканчивающие выделение,</em> должны следовать за <strong>непробельным символом</strong>\r\n_Иначе _ничего __не получится __!")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenMarkedWordsEndsWithNonWhitespace")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "__Непарные _символы в рамках\r\nодного_ абзаца не считаются__ выделением",
                            "__Непарные _символы в рамках\r\nодного_ абзаца не считаются__ выделением")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenPlacedInMultiLine")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "В случае __пересечения _двойных__ и одинарных_ подчерков ни _один из __них не_ считается__ выделением",
                            "В случае __пересечения _двойных__ и одинарных_ подчерков ни _один из __них не_ считается__ выделением")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenIntersect")
                        .SetCategory("MarkdownSpecification");
                    yield return new TestCaseData(
                            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
                            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка")
                        .SetName("Render_ShouldIgnoreSpanTags_WhenTextIsEmpty")
                        .SetCategory("MarkdownSpecification");
                }
            }
        }
    }
}