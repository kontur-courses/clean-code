namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        [SetUp]
        public void Setup()
        {
            
        }
        
        public static IEnumerable<TestCaseData> UnderlineSymbolsTests
        {
            get
            {
                yield return new TestCaseData("Текст, _окруженный с двух сторон_ одинарными символами подчерка").
                    SetName("Текст, _окруженный с двух сторон_ одинарными символами подчерка  должен помещаться в HTML-тег <em>").
                    Returns("Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка");
                yield return new TestCaseData("__Выделенный двумя символами текст__").
                    SetName("__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>").
                    Returns("<strong>Выделенный двумя символами текст</strong>");
            }
        }        
        
        public static IEnumerable<TestCaseData> EscapeSymbolTests
        {
            get
            {
                yield return new TestCaseData(@"\_Вот это\_, не должно выделиться тегом").
                    SetName(@"\_Вот это\_, не должно выделиться тегом \<em>.").
                    Returns(@"_Вот это_, не должно выделиться тегом");
                yield return new TestCaseData(@"Здесь сим\волы экранирования\ \должны остаться.\").
                    SetName("Символ экранирования исчезает из результата, только если экранирует что-то.").
                    Returns(@"Здесь сим\волы экранирования\ \должны остаться.\");                
                yield return new TestCaseData(@"\\_вот это будет выделено тегом_").
                    SetName("Символ экранирования тоже можно экранировать").
                    Returns(@"\<em>вот это будет выделено тегом</em>");
            }
        }
        
        public static IEnumerable<TestCaseData> HeaderSymbolTests
        {
            get
            {
                yield return new TestCaseData(@"# Заголовки").
                    SetName("Абзац, начинающийся с \"#\", выделяется тегом <h1> в заголовок.").
                    Returns("<h1> Заголовки</h1>\n");
            }
        }
        
        public static IEnumerable<TestCaseData> SymbolsCombinationTests
        {
            get
            {
                yield return new TestCaseData(@"Внутри __двойного выделения _одинарное_ тоже__ работает.").
                    SetName("Внутри двойного выделения одинарное тоже работает.").
                    Returns(@"Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.");
                yield return new TestCaseData(@"Но не наоборот — внутри _одинарного __двойное__ не_ работает.").
                    SetName("Внутри _одинарного выделения __двойное__ не_ работает").
                    Returns(@"Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.");                
                yield return new TestCaseData(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.").
                    SetName("Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка").
                    Returns(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.");                
                yield return new TestCaseData(@"Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._").
                    SetName("Подчерки могут выделять часть слова: и в _нач_але, и в сер_еди_не, и в кон_це._").
                    Returns(@"Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>");                
                yield return new TestCaseData(@"В то же время выделение в ра_зных сл_овах не работает.").
                    SetName("Выделение в ра_зных сл_овах не работает.").
                    Returns(@"В то же время выделение в ра_зных сл_овах не работает.");                
                yield return new TestCaseData(@"__Непарные_ символы в рамках одного абзаца не считаются выделением.").
                    SetName("__Непарные_ символы в рамках одного абзаца не считаются выделением.").
                    Returns(@"__Непарные_ символы в рамках одного абзаца не считаются выделением.");                
                yield return new TestCaseData(@"Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.").
                    SetName("За подчерками, начинающими выделение, должен следовать непробельный символ.").
                    Returns(@"Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.");                
                yield return new TestCaseData(@"Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка").
                    SetName("Подчерки, заканчивающие выделение, должны следовать за непробельным символом.").
                    Returns(@"Иначе эти <em>подчерки _не считаются</em> окончанием выделения и остаются просто символами подчерка");                
                yield return new TestCaseData(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.").
                    SetName("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.").
                    Returns(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.");                
                yield return new TestCaseData(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.").
                    SetName("Если внутри подчерков пустая строка ____, то они остаются символами подчерка.").
                    Returns(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.");
            }
        }

        [TestCaseSource(nameof(UnderlineSymbolsTests), Category = nameof(UnderlineSymbolsTests))]
        [TestCaseSource(nameof(EscapeSymbolTests), Category = nameof(EscapeSymbolTests))]
        [TestCaseSource(nameof(HeaderSymbolTests), Category = nameof(HeaderSymbolTests))]
        [TestCaseSource(nameof(SymbolsCombinationTests), Category = nameof(SymbolsCombinationTests))]
        public string MdRender_ShouldReturnRightString(string text)
        {
            return Md.Render(text);
        }
    }
}
