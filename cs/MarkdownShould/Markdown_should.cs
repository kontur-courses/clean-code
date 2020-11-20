using FluentAssertions;
using Markdown.Converters;
using Markdown.TokenReaders;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_Should
    {
        private Markdown markdown;

        [SetUp]
        public void SetUp()
        {
            var tokenReaders = new ITokenReader[]
            {
                new HeaderTokenReader(),
                new StrongTokenReader(),
                new EmphasizedTokenReader(),
                new PlainTextTokenReader()
            };
            var textParser = new TextParser(tokenReaders);

            var tokenConverters = new TokenConverterFactory();
            var htmlConverter = new HtmlConverter(tokenConverters);

            markdown = new Markdown(htmlConverter, textParser);
        }

        [Test]
        public void ReturnCorrectHtmlString_UnderliningElement()
        {
            var text = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";
            var expectedLine = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_DoubleUnderliningElement()
        {
            var text = "__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега ";
            var expectedLine =
                "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега ";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_DoubleUnderliningElementInsideUnderliningElement()
        {
            var text = "Внутри __двойного выделения _одинарное_ тоже__ работает";
            var expectedLine = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_OneUnderliningElementInsideDoubleUnderliningElement()
        {
            var text = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
            var expectedLine = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_UnderliningNumbers()
        {
            var text =
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";
            var expectedLine =
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_UnderliningInsidePartsOfWord()
        {
            var text = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";
            var expectedLine =
                "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_UnderliningDifferentWords()
        {
            var text = "В то же время выделение в ра_зных сл_овах не работает.";
            var expectedLine = "В то же время выделение в ра_зных сл_овах не работает.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_UnpairedSymbols()
        {
            var text = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
            var expectedLine = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_NoSpaceBeforeStartingUnderlining()
        {
            var text = "Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";
            var expectedLine = "Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_NoSpaceAfterEndingUnderlining()
        {
            var text = "Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.";
            var expectedLine =
                "Иначе эти <em>подчерки _не считаются</em> окончанием выделения и остаются просто символами подчерка.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_CrossingUnderlinings()
        {
            var text =
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";
            var expectedLine =
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_NoSymbolsBetweenUnderlinings()
        {
            var text = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";
            var expectedLine = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_ShieldedSymbols()
        {
            var text = "\\_Вот это\\_, не должно выделиться тегом \\<em>.";
            var expectedLine = "_Вот это_, не должно выделиться тегом \\<em>.";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }

        [Test]
        public void ReturnCorrectHtmlString_Header()
        {
            var text = "#Заголовок __с _разными_ символами__";
            var expectedLine = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>";

            var actualLine = markdown.Render(text);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
    }
}