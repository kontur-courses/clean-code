using System;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MdRenderShould
    {
        [TestCase("# Hello _World_!! __Th_is__ i_s __a__ __markdown _test_ sentence__", 
            ExpectedResult = "<h1>Hello <em>World</em>!! __Th_is__ i_s <strong>a</strong> <strong>markdown <em>test</em> sentence</strong></h1>")]
        [TestCase("# _a \n\n a_\n\na", ExpectedResult = "<h1>_a </h1>\n\n a_\n\na")]
        [TestCase("# _a \n\n# a_\n\na", ExpectedResult = "<h1>_a </h1>\n\n<h1>a_</h1>\n\na")]
        [TestCase("_a \n\n\n a_\n\na", ExpectedResult = "_a \n\n\n a_\n\na")]
        
        [TestCase("", ExpectedResult = "")]
        [TestCase("a", ExpectedResult = "a")]
        [TestCase("abcdef", ExpectedResult = "abcdef")]
        
        [TestCase("_abcdef_", ExpectedResult = "<em>abcdef</em>")]
        [TestCase("__abcdef__", ExpectedResult = "<strong>abcdef</strong>")]
        [TestCase("__a__ bcdef", ExpectedResult = "<strong>a</strong> bcdef")]
        [TestCase("bc __a__ def", ExpectedResult = "bc <strong>a</strong> def")]
        [TestCase("bc__a__ def", ExpectedResult = "bc<strong>a</strong> def")]
        [TestCase("__a__bcde__f__", ExpectedResult = "<strong>a</strong>bcde<strong>f</strong>")]
        
        // Если внутри подчерков пустая строка ____, то они остаются символами подчерка.
        [TestCase("__", ExpectedResult = "__")]
        [TestCase("_ _", ExpectedResult = "_ _")]  
        [TestCase("____", ExpectedResult = "____")]  
        [TestCase("__ __", ExpectedResult = "__ __")]
        [TestCase("a__ __aaa_ _a", ExpectedResult = "a__ __aaa_ _a")]

        // _Непарные символы в рамках одного абзаца не считаются выделением.
        [TestCase("_abcdef", ExpectedResult = "_abcdef")]
        [TestCase("_", ExpectedResult = "_")]
        [TestCase("__abcdef", ExpectedResult = "__abcdef")]
        [TestCase("abcdef__", ExpectedResult = "abcdef__")]
        [TestCase("ab__cdef", ExpectedResult = "ab__cdef")]

        // За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.
        [TestCase("a_ bc_de", ExpectedResult = "a_ bc_de")]
        [TestCase("a__ bc__de", ExpectedResult = "a__ bc__de")]
        
        // Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки не считаются окончанием выделения и остаются просто символами подчерка.
        [TestCase("a_bc _de", ExpectedResult = "a_bc _de")]
        [TestCase("a _bc _de", ExpectedResult = "a _bc _de")]
        [TestCase("a__bc __de", ExpectedResult = "a__bc __de")]
        [TestCase("a __bc __de", ExpectedResult = "a __bc __de")]
        
        [TestCase("a _ bc _ de", ExpectedResult = "a _ bc _ de")]
        [TestCase("a_ bc _de", ExpectedResult = "a_ bc _de")]
        [TestCase("a__ bc __de", ExpectedResult = "a__ bc __de")]
        [TestCase("a __ bc __ de", ExpectedResult = "a __ bc __ de")]

        // Вложенность
        [TestCase(" __a__b_cd_e__f__ ", ExpectedResult = " <strong>a</strong>b<em>cd</em>e<strong>f</strong> ")]
        [TestCase("a__b_cd_e__f", ExpectedResult = "a<strong>b<em>cd</em>e</strong>f")]
        [TestCase("a_b__cd__e_f", ExpectedResult = "a<em>b__cd__e</em>f")]
        [TestCase("__a_a__a_a __a _a_ a__", ExpectedResult = "__a_a__a_a <strong>a <em>a</em> a</strong>")]
        
        // Пересечение
        [TestCase("a_b__cd_e__f", ExpectedResult = "a_b__cd_e__f")]
        [TestCase("a__b_cd__e_f", ExpectedResult = "a__b_cd__e_f")]
        [TestCase("__Th_is__ i_s", ExpectedResult = "__Th_is__ i_s")]
        [TestCase(" __Th_is__ i_s", ExpectedResult = " __Th_is__ i_s")]
        [TestCase("__Th_is__ a", ExpectedResult = "<strong>Th_is</strong> a")]
        [TestCase("__a_b__c_d __a__", ExpectedResult = "__a_b__c_d <strong>a</strong>")]
        
        // Части слов
        [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах")]
        [TestCase("ра__зных сл__овах", ExpectedResult = "ра__зных сл__овах")]
        [TestCase("abcde__f__", ExpectedResult = "abcde<strong>f</strong>")]
        [TestCase("__a__bcdef", ExpectedResult = "<strong>a</strong>bcdef")]
        [TestCase("def__a__bc", ExpectedResult = "def<strong>a</strong>bc")]
        
        // Числа
        [TestCase("цифрами_12_3", ExpectedResult = "цифрами_12_3")]
        [TestCase("a_1_bc de", ExpectedResult = "a_1_bc de")]
        [TestCase("a_2_a", ExpectedResult = "a_2_a")]
        [TestCase("a_3_a", ExpectedResult = "a_3_a")]
        [TestCase("a_4_", ExpectedResult = "a_4_")]
        [TestCase("a_18_", ExpectedResult = "a_18_")]
        [TestCase("a_ 5_5", ExpectedResult = "a_ 5_5")]
        [TestCase("5", ExpectedResult = "5")]
        [TestCase("_18_", ExpectedResult = "<em>18</em>")]
        
        // Шарп
        [TestCase("#_a ", ExpectedResult = "#_a ")]
        [TestCase("#_ab_", ExpectedResult = "#<em>ab</em>")]
        [TestCase("# _ab_", ExpectedResult = "<h1><em>ab</em></h1>")]
        [TestCase(" # _ab_", ExpectedResult = " # <em>ab</em>")]
        [TestCase("d# _ab_", ExpectedResult = "d# <em>ab</em>")]
        [TestCase("d# d_ab_", ExpectedResult = "d# d<em>ab</em>")]
        [TestCase("# d_ab_", ExpectedResult = "<h1>d<em>ab</em></h1>")]
        
        // Экранирование
        [TestCase(@"\a_a_", ExpectedResult = @"\a<em>a</em>")]
        [TestCase(@"\_a_", ExpectedResult = @"_a_")]
        [TestCase(@"\a \_a_", ExpectedResult = @"\a _a_")]
        [TestCase(@"\a \\\\\_a_", ExpectedResult = @"\a \\_a_")]
        [TestCase(@"\a \\_a_", ExpectedResult = @"\a \<em>a</em>")]
        public string CorrectRender(string text)
        {
            var actual = Md.Render(text);
            return actual;
        }
        
        [TestCase(" __ab__ ", 100, 6)]
        [TestCase("2a1b", 100, 2)]
        [TestCase(" ab ", 100, 8)]
        [TestCase(" _a_ \n\n __a__  ", 100, 8)]
        public void HaveCorrectRenderTime(string text, int textLenghtFactor, int timeFactor)
        {
            var firstTextBuilder = new StringBuilder();
            for (var i = 0; i < textLenghtFactor; i++)
                firstTextBuilder.Append(text);

            var secondTextBuilder = new StringBuilder();
            for (var i = 0; i < textLenghtFactor * timeFactor; i++)
                secondTextBuilder.Append(text);

            var firstText = firstTextBuilder.ToString();
            var secondText = secondTextBuilder.ToString();
            
            var firstTime = new TimeSpan();
            var secondTime = new TimeSpan();

            Md.Render("");
            for (var i = 0; i < 1000; i++)
            {
                var firstTimer = new Stopwatch();
                firstTimer.Start();
                Md.Render($"# {firstText}");
                firstTimer.Stop();
                firstTime += firstTimer.Elapsed;

                var secondTimer = new Stopwatch();
                secondTimer.Start();
                Md.Render($"# {secondText}");
                secondTimer.Stop();
                secondTime += secondTimer.Elapsed;
            }

            secondTime.Should().BeLessThan(firstTime * (1 + timeFactor));
        }
    }
}