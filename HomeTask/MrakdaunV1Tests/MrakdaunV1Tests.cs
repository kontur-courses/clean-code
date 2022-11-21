using System;
using FluentAssertions;
using MrakdaunV1;
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

        public void GetParsedText_ShouldGive_CorrectAnswer(string text, string answer)
        {
            _engine.GetParsedText(text).Should().Be(answer);
        }
    }
}