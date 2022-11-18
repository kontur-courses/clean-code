using NUnit.Framework;
using Markdown;
using Microsoft.VisualBasic;
using FluentAssertions;

namespace Markdown_tests
{
    [TestFixture]
    public class Tests
    {
        Md markdown = new Md();
        Parser parser = new Parser();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Italic()
        {
            var concs = parser.ParseMdToHTML(@"Текст, _окруженный с двух сторон_ одинарными символами подчерка");
        }

        [Test]
        public void InnerItalic()
        {
            var concs = parser.ParseMdToHTML(@"Внутри __двойного выделения _одинарное_ тоже__ работает.");
        }

        [Test]
        public void InnerItalicInOneWordOnBoard()
        {
            var concs = parser.ParseMdToHTML(@"Внутри __двойного выделения _одинарн_ое тоже__ работает.");
        }

        [Test]
        public void InnerItalicInOneWord()
        {
            var concs = parser.ParseMdToHTML(@"Внутри __двойного выделения од_инарн_ое тоже__ работает.");
        }

        [Test]
        public void InnerBold()
        {
            var concs = parser.ParseMdToHTML(@"Но не наоборот — внутри _одинарного __двойное__ не_ работает.");
        }


        [Test]
        public void Test2()
        {
            var concs = parser.ParseMdToHTML(@"__Непарные_ символы в рамках одного абзаца не _считаются_ выделением.");

        }

        [Test]
        public void TestBold()
        {
            var concs = parser.ParseMdToHTML(@"__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \<strong>.");

        }

        [Test]
        public void TesEscapeCharacter()
        {
            var concs = parser.ParseMdToHTML(@"Любой символ можно экранировать, чтобы он не считался частью разметки. \_Вот это\_, не должно выделиться тегом \<em>.");
        }

        [Test]
        public void TesEscapeCharacter1()
        {
            var concs = parser.ParseMdToHTML(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\");
        }

        [Test]
        public void TesEscapeCharacter2()
        {
            var concs = parser.ParseMdToHTML(@"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ \<em>");
        }

        [Test]
        public void TripleSlash()
        {
            var concs = parser.ParseMdToHTML(@"\\\_вот это не будет выделено тегом_");
        }

        [Test]
        public void ConcatinationWithDigits()
        {
            var concs = parser.ParseMdToHTML(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.");
        }

        [Test]
        public void PartOfWord()
        {
            var concs = parser.ParseMdToHTML(@"Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._");
        }

        [Test]
        public void DifferentWords()
        {
            var concs = parser.ParseMdToHTML(@"В то же время выделение в ра_зных сл_овах не работает.");
        }
        
        [Test]
        public void DifferentWordsAndRightWrap()
        {
            var concs = parser.ParseMdToHTML(@"Строка _состоит из верного открывающего символа, не_верного слово_сочетания и верного закрывающего_");
        }

        [Test]
        public void NoChange()
        {
            var concs = parser.ParseMdToHTML(@"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.");
        }

        [Test]
        public void ModifierAfterSpace()
        {
            var concs = parser.ParseMdToHTML(@"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.");
        }

        [Test]
        public void OverlapModifiers()
        {
            var concs = parser.ParseMdToHTML(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.");
        }

        [Test]
        public void NoSymbolsBetweenTwoBoldMods()
        {
            var concs = parser.ParseMdToHTML(@"Если внутри Bold-модификаторов пустая строка ____, то они остаются символами подчерка.");
        }

        [Test]
        public void NoSymbolsBetweenTwoItalicMods()
        {
            var concs = parser.ParseMdToHTML(@"Если внутри Italic-модификаторов пустая строка __, то они остаются символами подчерка.");
        }

        [Test]
        public void Title()
        {
            var concs = parser.ParseMdToHTML(@"# Этот абзац должен преобразоваться в <h1> Этот абзац должен преобразоваться в </h1>");
        }

        [Test]
        public void TitleWithModifiers()
        {
            var concs = parser.ParseMdToHTML(@"# Заголовок __с _разными_ символами__");
        }
    }
}