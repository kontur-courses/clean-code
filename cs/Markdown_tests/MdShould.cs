using Markdown;
using Microsoft.VisualBasic;
using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics;
using System.Linq;

namespace Markdown_tests
{
    [TestFixture]
    public class Tests
    {
        [TestCase(@"Текст, _окруженный с двух сторон_ одинарными символами подчерка", @"Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка", TestName = "{m}_Italic")]
        [TestCase(@"Внутри __двойного выделения _одинарное_ тоже__ работает.",@"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.", TestName = "{m}_InnerItalic")]
        [TestCase(@"Внутри __двойного выделения _одинарн_ое тоже__ работает.",@"Внутри <strong>двойного выделения <em>одинарн</em>ое тоже</strong> работает.", TestName = "{m}_InnerItalicInOneWordOnBoard")]
        [TestCase(@"Внутри __двойного выделения од_инарн_ое тоже__ работает.",@"Внутри <strong>двойного выделения од<em>инарн</em>ое тоже</strong> работает.", TestName = "{m}_InnerItalicInOneWord")]
        [TestCase(@"Но не наоборот — внутри _одинарного __двойное__ не_ работает.",@"Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.", TestName = "{m}_InnerBold")]
        [TestCase(@"__Выделенный текст__ должен тегом \<strong>.", @"<strong>Выделенный текст</strong> должен тегом \<strong>.", TestName = "{m}_Bold")]
        [TestCase(@"Любой символ можно экранировать, чтобы он не считался частью разметки. \_Вот это\_, не должно выделиться тегом \<em>.",@"Любой символ можно экранировать, чтобы он не считался частью разметки. _Вот это_, не должно выделиться тегом \<em>.", TestName = "{m}_ShieldingSlash")]
        [TestCase(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\",@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\", TestName = "{m}_NonShieldingSlash")]
        [TestCase(@"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ \<em>",@"Символ экранирования тоже можно экранировать: \<em>вот это будет выделено тегом</em> \<em>", TestName = "{m}_SlashShieldingSlash")]
        [TestCase(@"\\\_вот это не будет выделено тегом_",@"\_вот это не будет выделено тегом_", TestName = "{m}_TripleSlash")]
        [TestCase(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.",@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.", TestName = "{m}_ConcatinationWithDigits")]
        [TestCase(@"Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._",@"Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>", TestName = "{m}_ItalicInSameWord")]
        [TestCase(@"В то же время выделение в ра_зных сл_овах не работает.",@"В то же время выделение в ра_зных сл_овах не работает.", TestName = "{m}_ItalicInDifferentWord")]
        [TestCase(@"Строка _состоит из верного открывающего символа, не_верного слово_сочетания и верного закрывающего_",@"Строка <em>состоит из верного открывающего символа, не_верного слово_сочетания и верного закрывающего</em>", TestName = "{m}_ItalicDifferentWordsAndRightWrap")]
        [TestCase(@"За подчерками должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются",@"За подчерками должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются", TestName = "{m}_WhiteSpaceAfterOpeningModifier")]
        [TestCase(@"Эти _подчерки _не считаются окончанием выделения",@"Эти _подчерки _не считаются окончанием выделения", TestName = "{m}_ModifierAfterSpace")]
        [TestCase(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.", TestName = "{m}_OverlapModifiers")]
        [TestCase(@"Если внутри Bold-модификаторов пустая строка ____, то они остаются символами подчерка.",@"Если внутри Bold-модификаторов пустая строка ____, то они остаются символами подчерка.", TestName = "{m}_NoSymbolsBetweenTwoBoldMods")]
        [TestCase(@"Если внутри Italic-модификаторов пустая строка __, то они остаются символами подчерка.",@"Если внутри Italic-модификаторов пустая строка __, то они остаются символами подчерка.", TestName = "{m}_NoSymbolsBetweenTwoItalicMods")]
        [TestCase(@"# Этот абзац должен преобразоваться в <h1> Этот абзац должен преобразоваться в </h1>",@"<h1> Этот абзац должен преобразоваться в <h1> Этот абзац должен преобразоваться в </h1></h1>", TestName = "{m}_Title")]
        [TestCase(@"# Заголовок __с _разными_ символами__",@"<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "{m}_TitleWithModifiers")]
        public void Md_CommonInput_ShouldBeExpected(string md, string exp)
        {
            var markdown = new Md();

            var html = markdown.Render(md);

            html.Should().Be(exp);
        }

        [TestCase(@"", @"", TestName = "EmptyInput")]
        [TestCase(@"  ", @"  ", TestName = "OnlyWhitespacesInput")]
        [TestCase(@"_", @"_", TestName = "OnlyItalic")]
        [TestCase(@"__", @"__", TestName = "OnlyBold")]
        public void Md_BorderlineCases_ShouldBeExpected(string md, string exp)
        {
            var markdown = new Md();

            var html = markdown.Render(md);

            html.Should().Be(exp);
        }

        [Test]
        public void Md_Performance_ShouldBeLinear()
        {
            var markdown = new Md();
            var line = @"_простой_ __текст__ \_имеющий\_ разные символы";
            long lastTime = 0;

            for (int i = 1; i < 10; i++)
            {
                var testLine = string.Concat(Enumerable.Repeat(line, i));

                var sw = new Stopwatch();
                sw.Start();

                var html = markdown.Render(testLine);

                sw.Stop();

                if (i == 1) lastTime = sw.ElapsedTicks;
                else
                {
                    var currentTime = sw.ElapsedTicks;
                    var dif = currentTime - lastTime;

                    dif.Should().BeLessThan(currentTime);
                }
            }
        }
    }
}