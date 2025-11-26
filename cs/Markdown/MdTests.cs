using System.Text;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTests
    {
        private Md md;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            md = new Md();
        }
        

    [TestCase(@"# Заголовок с \_экранированием_", "<h1>Заголовок с _экранированием_</h1>")]
    [TestCase(@"# Заголовок с \\_курсивом_", "<h1>Заголовок с <em>курсивом</em></h1>")]
    [TestCase(@"# __Жирный \_текст__", "<h1><strong>Жирный _текст</strong></h1>")]
    [TestCase(@"# \# Не заголовок", "<h1># Не заголовок</h1>")]
    public void Render_HeaderWithEscapes_HandlesCorrectly(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }


    [TestCase(@"_курсив _ с пробелом_", "_курсив _ с пробелом_")]
    [TestCase(@"__жирный __ с пробелом__", "__жирный __ с пробелом__")]
    [TestCase(@"_ курссив с пробелом в начале_", "_ курссив с пробелом в начале_")]
    [TestCase(@"_курсив с пробелом в конце _", "_курсив с пробелом в конце _")]
    [TestCase(@"__ жирный с пробелом__", "__ жирный с пробелом__")]
    [TestCase(@"__жирный с пробелом __", "__жирный с пробелом __")]
    public void Render_SpacesInFormatting_HandlesCorrectly(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }
    
    
    [TestCase("__жирный\nс переносом__", "__жирный\nс переносом__")]
    [TestCase("# Заголовок\n_курсив_", "<h1>Заголовок</h1>\n<em>курсив</em>")]
    [TestCase("_курсив_\n__жирный__", "<em>курсив</em>\n<strong>жирный</strong>")]
    public void Render_NewLinesInFormatting_HandlesCorrectly(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }


    
    [TestCase(@"____", "____")]
    [TestCase(@"__  __", "__  __")]
    [TestCase(@"_ _", "_ _")]
    [TestCase(@"-", "-")]
    public void Render_EmptyAndMinimalCases_HandlesCorrectly(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }




    [TestCase(@"# _курсивный заголовок__", "<h1>_курсивный заголовок__</h1>")]
    [TestCase(@"_пересечение __одинарных_ и двойных__", "_пересечение __одинарных_ и двойных__")]
    [TestCase(@"- _пункт __с жирным_ текстом__", "<ul>\n<li>_пункт __с жирным_ текстом__</li>\n</ul>")]
    public void Render_ComplexInteraction_HandlesCorrectly(string input, string expected)
    {
        md.Render(input).Should().Be(expected);
    }
        
        [TestCase("- пункт списка", "<ul>\n<li>пункт списка</li>\n</ul>")]
        [TestCase("- первый\n- второй\n- третий", "<ul>\n<li>первый</li>\n<li>второй</li>\n<li>третий</li>\n</ul>")]
        [TestCase("- _курсивный_ пункт", "<ul>\n<li><em>курсивный</em> пункт</li>\n</ul>")]
        [TestCase("- __жирный__ пункт", "<ul>\n<li><strong>жирный</strong> пункт</li>\n</ul>")]
        [TestCase("Текст \n- первый пункт\n- второй пункт", "Текст \n<ul>\n<li>первый пункт</li>\n<li>второй пункт</li>\n</ul>")]
        [TestCase("- первый список\n- тот же список\n\n- новый список\n- другой список", "<ul>\n<li>первый список</li>\n<li>тот же список</li>\n</ul>\n<ul>\n<li>новый список</li>\n<li>другой список</li>\n</ul>")]
        [TestCase("\\- не пункт списка", "- не пункт списка")]
        [TestCase("Текст - не пункт", "Текст - не пункт")]
        [TestCase("-пункт без пробела", "-пункт без пробела")]
        [TestCase("# Заголовок\n- пункт\n- еще пункт", "<h1>Заголовок</h1>\n<ul>\n<li>пункт</li>\n<li>еще пункт</li>\n</ul>")]
        [TestCase("- пункт\n# Заголовок\n- еще пункт", "<ul>\n<li>пункт</li>\n</ul><h1>Заголовок</h1>\n<ul>\n<li>еще пункт</li>\n</ul>")]
        public void Render_ListItemTag_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("", "")]
        [TestCase("   ", "   ")]
        [TestCase("___", "___")]
        [TestCase("_   _", "_   _")]
        public void Render_EdgeCases_ReturnsExpected(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("простой текст", "простой текст")]
        public void Render_PlainText_ReturnsAsIs(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("_курсив_", "<em>курсив</em>")]
        [TestCase("в _нач_але, сер_еди_не, кон_це._", "в <em>нач</em>але, сер<em>еди</em>не, кон<em>це.</em>")]
        [TestCase("_непарные символы", "_непарные символы")]
        [TestCase("эти_ подчерки_", "эти_ подчерки_")]
        [TestCase("эти _подчерки _не выделяются", "эти _подчерки _не выделяются")]
        public void Render_EmphasisTag_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("__жирный__", "<strong>жирный</strong>")]
        [TestCase("в __нач__але, сер__еди__не, кон__це.__", "в <strong>нач</strong>але, сер<strong>еди</strong>не, кон<strong>це.</strong>")]
        [TestCase("__непарные символы", "__непарные символы")]
        [TestCase("эти__ подчерки__", "эти__ подчерки__")]
        [TestCase("эти __подчерки __не выделяются", "эти __подчерки __не выделяются")]
        public void Render_StrongTag_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("# Заголовок", "<h1>Заголовок</h1>")]
        [TestCase("# Заголовок __с _разными_ символами__", "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        [TestCase("# Первый\n\n# Второй", "<h1>Первый</h1>\n\n<h1>Второй</h1>")]
        [TestCase("#Заголовок", "#Заголовок")]
        [TestCase("Текст # не заголовок", "Текст # не заголовок")]
        [TestCase("#  Заголовок", "#  Заголовок")]
        [TestCase("Текст\n# Заголовок", "Текст\n<h1>Заголовок</h1>")]
        public void Render_HeaderTag_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase(@"\_невыделенный\_", "_невыделенный_")]
        [TestCase(@"\\текст\\", @"текст")]
        [TestCase("\\_текст\\_", "_текст_")]
        [TestCase("текст\\", "текст\\")]
        [TestCase("сим\\волы", "сим\\волы")]
        public void Render_EscapeSequences_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает.", 
            "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.")]
        [TestCase("Но не наоборот — внутри _одинарного __двойное__ не_ работает.", 
            "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.")]
        [TestCase("__жирный _и курсив_ текст__", "<strong>жирный <em>и курсив</em> текст</strong>")]
        public void Render_NestedTags_HandlesCorrectly(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }

        [TestCase("цифры_12_3 текст", "цифры_12_3 текст")]
        [TestCase("ра_зных сл_овах", "ра_зных сл_овах")]
        [TestCase("_ текст _", "_ текст _")]
        [TestCase("__пересечение _двойных__ и одинарных_", "__пересечение _двойных__ и одинарных_")]
        [TestCase("_пересечение __одинарных_ и двойных__", "_пересечение __одинарных_ и двойных__")]
        [TestCase("_курсив __без жирного__ текст_", "<em>курсив __без жирного__ текст</em>")]
        public void Render_InvalidFormatting_NotProcessed(string input, string expected)
        {
            md.Render(input).Should().Be(expected);
        }
        

        [TestCase("- __жирный _курсив__ и текст после_", "<ul>\n<li>__жирный _курсив__ и текст после_</li>\n</ul>")]
        [TestCase("- _курсив __жирный_ и текст после__", "<ul>\n<li>_курсив __жирный_ и текст после__</li>\n</ul>")]
        [TestCase("__жирный _курсив__ текст_", "__жирный _курсив__ текст_")]
        [TestCase("- _незакрытый курсив\n- нормальный пункт\n\n- новый список\n- тоже нормальный", 
            "<ul>\n<li>_незакрытый курсив</li>\n<li>нормальный пункт</li>\n</ul>\n<ul>\n<li>новый список</li>\n<li>тоже нормальный</li>\n</ul>")]
        [TestCase(@"- пункт с \_экранированием_ и нормальным курсивом", 
            @"<ul>" + "\n" + @"<li>пункт с _экранированием_ и нормальным курсивом</li>" + "\n" + @"</ul>")]
        public void Render_InvalidFormatting(string input, string expected)
        {
            var actual = md.Render(input);
            actual.Should().Be(expected);
        }

        [Test]
        public void Render_OnlyNewLines_ReturnsEmpty()
        {
            md.Render("\n\n\n").Should().Be("\n\n\n");
        }

        [Test]
        public void Render_Performance_CheckLinearComplexity()
        {
            var random = new Random(42);
            var sizes = new[] { 1000000, 2000000 };
            var executionTimes = new long[sizes.Length];

            for (int i = 0; i < sizes.Length; i++)
            {
                var size = sizes[i];
                var text = GenerateTextWithMarkup(random, size);
        
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                md.Render(text);
                stopwatch.Stop();
        
                executionTimes[i] = stopwatch.ElapsedMilliseconds;
                Console.WriteLine($"Size: {size}, Time: {executionTimes[i]}ms");
            }
    
            for (int i = 1; i < sizes.Length; i++)
            {
                var sizeRatio = (double)sizes[i] / sizes[i - 1];
                var timeRatio = (double)executionTimes[i] / Math.Max(executionTimes[i - 1], 1);

                Console.WriteLine($"Size: {sizes[i-1]}->{sizes[i]}, Time: x{timeRatio:F1}");
                
                if (timeRatio > sizeRatio * 2.5)
                {
                    Assert.Fail($"Нелинейная сложность: время выросло в {timeRatio:F1} раз при увеличении данных в {sizeRatio} раз )");
                }
            }

            Assert.Pass("Линейная или почти линейная сложность");
        }

        private string GenerateTextWithMarkup(Random random, int length)
        {
            var sb = new StringBuilder();
            var words = new[] { "текст", "слово", "разметка", "тест" };
    
            while (sb.Length < length)
            {
    
                if (random.Next(0, 4) == 0) 
                {
                    if (random.Next(0, 2) == 0)
                        sb.Append("_курсив_ ");
                    else
                        sb.Append("__жирный__ ");
                }
                else
                {
                    sb.Append(words[random.Next(words.Length)] + " ");
                }
            }
    
            return sb.ToString().Substring(0, Math.Min(sb.Length, length));
        }




        [Test]
        public void Render_SpecificationText_OutputsToConsole()
        {

            var specText = """
                           # Спецификация языка разметки

                           Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github.
                           Все совпадения случайны ;)



                           # Курсив

                           Текст, _окруженный с двух сторон_ одинарными символами подчерка,
                           должен помещаться в HTML-тег <em> вот так:

                           Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка,
                           должен помещаться в HTML-тег <em>.



                           # Полужирный

                           __Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>.



                           # Экранирование

                           Любой символ можно экранировать, чтобы он не считался частью разметки.
                           \_Вот это\_, не должно выделиться тегом <em>.

                           Символ экранирования исчезает из результата, только если экранирует что-то.
                           Здесь сим\волы экранирования\ \должны остаться.\

                           Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ <em>


                           # Маркированный список

                           Абзац, начинающийся с "- ", выделяется как элемент маркированного списка.
                           Смежные элементы списка группируются в один список.

                           - первый пункт
                           - второй пункт
                           - третий пункт

                           превратится в:

                           <ul>
                           <li>первый пункт</li>
                           <li>второй пункт</li>
                           <li>третий пункт</li>
                           </ul>

                           В тексте элементов списка могут присутствовать все прочие символы разметки с указанными правилами.

                           - пункт с _курсивом_
                           - пункт с __жирным__ текстом
                           - пункт с __жирным _и курсивным_ текстом__

                           превратится в:

                           <ul>
                           <li>пункт с <em>курсивом</em></li>
                           <li>пункт с <strong>жирным</strong> текстом</li>
                           <li>пункт с <strong>жирный <em>и курсивным</em> текстом</strong></li>
                           </ul>

                           Элементы списка, разделенные пустыми строками, создают отдельные списки.

                           - первый список
                           - тот же список

                           - новый список
                           - другой список

                           превратится в:

                           <ul>
                           <li>первый список</li>
                           <li>тот же список</li>
                           </ul>
                           <ul>
                           <li>новый список</li>
                           <li>другой список</li>
                           </ul>

                           Символ "-" в середине строки не считается началом списка.
                           Текст - не пункт списка

                           Экранирование также работает для символа списка:
                           \- не пункт списка


                           # Взаимодействие тегов

                           Внутри __двойного выделения _одинарное_ тоже__ работает.

                           Но не наоборот — внутри _одинарного __двойное__ не_ работает.

                           Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.

                           Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._

                           В то же время выделение в ра_зных сл_овах не работает.

                           __Непарные_ символы в рамках одного абзаца не считаются выделением.

                           За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением
                           и остаются просто символами подчерка.

                           Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения
                           и остаются просто символами подчерка.

                           В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.

                           Если внутри подчерков пустая строка ____, то они остаются символами подчерка.



                           # Заголовки

                           Абзац, начинающийся с "# ", выделяется тегом <h1> в заголовок.
                           В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.

                           Таким образом

                           # Заголовок __с _разными_ символами__

                           превратится в:

                           <h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>

                           # Маркированный список

                           Абзац, начинающийся с "- ", выделяется как элемент маркированного списка.
                           Смежные элементы списка группируются в один список.

                           - первый
                           - второй
                           - третий

                           превратится в:

                           <ul>
                           <li>первый</li>
                           <li>второй</li>
                           <li>третий</li>
                           </ul>

                           В тексте элементов списка могут присутствовать другие символы разметки с указанными правилами.

                           - пункт с _курсивом_
                           - пункт с __жирным__ текстом
                           - пункт с __жирным _и курсивным_ текстом__

                           превратится в:

                           <ul>
                           <li>пункт с <em>курсивом</em></li>
                           <li>пункт с <strong>жирным</strong> текстом</li>
                           <li>пункт с <strong>жирный <em>и курсивным</em> текстом</strong></li>
                           </ul>

                           Элементы списка, разделенные пустыми строками, создают отдельные списки.

                           - первый
                           - второй

                           - первый
                           - второй

                           превратится в:

                           <ul>
                           <li>первый</li>
                           <li>второй</li>
                           </ul>
                           <ul>
                           <li>первый</li>
                           <li>второй</li>
                           </ul>

                           Символ "-" в середине строки не считается началом списка.
                           Текст - не пункт списка

                           Экранирование также работает для символа списка:
                           \- не пункт списка
                           """;

            var result = md.Render(specText);

            Console.WriteLine(result);

        }

    }
}