using System;
using System.Text;
using FluentAssertions;
using MrakdaunV1;
using MrakdaunV1.MrakdounEngine;
using NUnit.Framework;

namespace MrakdaunV1Tests
{
    [TestFixture]
    public class Tests
    {
        private static MrakdaunEngine _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new MrakdaunEngine();
        }

        [TestCase("_a\na_", "_ _ _ _ _", TestName = "в разных строках нет выделения")]
        [TestCase("____", "_ _ _ _", TestName = "пустой текст в жире как 4 земли")]
        [TestCase("__aaaa__", "BS BS B B B B BS BS", TestName = "выделить все слово жирным")]
        [TestCase("_a_", "IS I IS", TestName = "выделить все слово")]
        [TestCase("_aa_aa", "IS I I IS _ _", TestName = "выделить часть слова")]
        [TestCase("_a _a", "_ _ _ _ _", TestName = "ничего (в конце должен быть пробел)")]
        [TestCase("a_ a_ a", "_ _ _ _ _ _ _", TestName = "ничего (за началом должен быть НЕ пробел)")]
        [TestCase("_a a_a", "_ _ _ _ _ _", TestName = "ничего не делать")]
        [TestCase("_a a_ a", "IS I I I IS _ _", TestName = "выделить два слова")]
        [TestCase("_a a_a_", "_ _ _ _ IS I IS", TestName = "выделить часть второго слова")]
        [TestCase("_a_a_", "IS I IS _ _", TestName = "выделить первую часть слова")]
        [TestCase("__aa_", "_ _ _ _ _", TestName = "ничего")]
        [TestCase("_aaa1_", "IS I I I I IS", TestName = "выделение полностью")]
        [TestCase("a_1_1a", "_ _ _ _ _ _", TestName = "ничего")]
        [TestCase("_1_1", "_ _ _ _", TestName = "ничего")]
        [TestCase("_1_", "IS I IS", TestName = "выделение")]
        [TestCase("_aa __bb__ aa_", "IS I I I I I I I I I I I I IS", TestName = "bb НЕ жирный, остальное курсив")]
        [TestCase("__aa _bb_ aa__", "BS BS B B B BIS BI BI BIS B B B BS BS", TestName = "bb курсив, и еще все жирное")]
        [TestCase("__aa _bb_ _cc_ aa__", "BS BS B B B BIS BI BI BIS B BIS BI BI BIS B B B BS BS", TestName = "весь текст жирный с элементами курсива")]
        [TestCase("__aa _bb_ aa _c aa__ c_", "_ _ _ _ _ IS I I IS _ _ _ _ _ _ _ _ _ _ _ _ _ _", TestName = "bb будут курсив, а жира нигде не будет, сс не будет выделен")]
        [TestCase("_b __aa b_ _b aa__ b_", "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _", TestName = "вообще нет выделения")]
        [TestCase("__aa_aa__aa_", "_ _ _ _ _ _ _ _ _ _ _ _", TestName = "ничего (пересечение)")]
        [TestCase("__aaa _aa__ a aa_", "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _", TestName = "ничего (пересечение) 2")]
        public void Engine_ShouldCorrectParse_ItalicAndStrong(string text, string answer)
        {
            _engine.GetCharStatesString(_engine.GetParsedTextStates(text)).Should().Be(answer);
        }

        [TestCase(@"\_a_", "S _ _ _", TestName = "тест экранирования 1")]
        [TestCase(@"\_a_a_", "S _ _ IS I IS", TestName = "тест экранирования 2")]
        [TestCase(@"\\", "S _", TestName = "экранирование слеша")]
        [TestCase(@"\\_a_", "S _ IS I IS", TestName = "экранирования слеша 2")]
        [TestCase(@"\\_a a_", "S _ IS I I I IS", TestName = "экранирования слеша 3")]
        [TestCase(@"\__a__", "S _ _ _ _ _", TestName = "экранирование ломает нормальный жир")]
        [TestCase(@"\__a_", "S _ IS I IS", TestName = "экранирование чинит курсив")]
        [TestCase(@"a\a", "_ _ _", TestName = "экранирования нет")]
        [TestCase(@"_a\a_", "IS I I I IS", TestName = "экранирования не должно нисего сломать")]
        [TestCase(@"a\ a", "_ _ _ _", TestName = "экранирования нет (даже у разделителя)")]
        public void Engine_ShouldCorrectParse_Screen(string text, string answer)
        {
            _engine.GetCharStatesString(_engine.GetParsedTextStates(text)).Should().Be(answer);
        }

        [TestCase("# a __a _a_ a__\n", "SH1 SH1 H1 H1 BSH1 BSH1 BH1 BH1 BISH1 BIH1 BISH1 BH1 BH1 BSH1 BSH1 _", TestName = "Заголовок с разными символами")]
        [TestCase("# __aa _bb_ aa__", "SH1 SH1 BSH1 BSH1 BH1 BH1 BH1 BISH1 BIH1 BIH1 BISH1 BH1 BH1 BH1 BSH1 BSH1", TestName = "Заголовок не ломает остальную разметку")]
        [TestCase("a# aa", "_ _ _ _ _", TestName = "Тег заголовка не первый в строке")]
        [TestCase("#aaa", "_ _ _ _", TestName = "Заголовка нет, потому что нет пробела")]
        [TestCase("# aaa", "SH1 SH1 H1 H1 H1", TestName = "Обычный вариант заголовка")]
        [TestCase("# aa\na", "SH1 SH1 H1 H1 _ _", TestName = "Остутствие заголовка на новой строке")]
        public void Engine_ShouldCorrectParse_Header(string text, string answer)
        {
            _engine.GetCharStatesString(_engine.GetParsedTextStates(text)).Should().Be(answer);
        }
        
        [TestCase("", "")]
        [TestCase("_a_", "<em>a</em>")]
        [TestCase("__a__", "<strong>a</strong>")]
        [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em>  символами</h1></strong>")]
        public void HtmlRenderer_ShouldParseCorrectly(string text, string answer)
        {
            _engine.GetParsedText(text).Should().Be(answer);
        }

        [Test]
        [Timeout(5000)]
        public void TestPerformance50K()
        {
            StringBuilder sb = new();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append("aa _aaa_ a");
                sb.Append("__aa_a__a_");
                sb.Append(@"aaa \_aa_a");
                sb.Append("__aa_a__a_");
                sb.Append(@"aaa \_aa_a");
            }
            Action a = () => _engine.GetParsedText(sb.ToString());
            a.Should().NotThrow();
        }
    }
}