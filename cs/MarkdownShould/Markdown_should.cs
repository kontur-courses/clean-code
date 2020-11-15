using System.Collections.Generic;
using FluentAssertions;
using Markdown.TokenConverters;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Markdown_should
    {
        private IReadOnlyCollection<ITokenConverter> tokenConverters;
        private IReadOnlyCollection<ITokenGetter> tokenGetters;

        [SetUp]
        public void SetUp()
        {
            tokenConverters = new List<ITokenConverter>
            {
                new StrongTokenConverter(),
                new EmphasizedTokenConverter(),
                new TextTokenConverter()
            };
            tokenGetters = new ITokenGetter[]
            {
                new StrongTokenGetter(),
                new EmphasizedTokenGetter(),
                new TextTokenGetter()
            };
        }

        [Test]
        public void Main_ReturnCorrectHtmlString_TextWithEm()
        {
            var text = "Текст, _окруженный с двух сторон_ одинарными символами подчерка";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_TextWithStrong()
        {
            var text = "__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега ";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега ";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_TextWithEmInsideStrong()
        {
            var text = "Внутри __двойного выделения _одинарное_ тоже__ работает";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_StrongInsideEm()
        {
            var text = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_UnderliningNumbers()
        {
            var text = "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_EmPartsOfWords()
        {
            var text = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_UnderliningDifferentWords()
        {
            var text = "В то же время выделение в ра_зных сл_овах не работает.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "В то же время выделение в ра_зных сл_овах не работает.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_UnpairedSymbols()
        {
            var text = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_NoSpaceBeforeStartingUnderlining()
        {
            var text = "Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_NoSpaceAfterEndingUnderlining()
        {
            var text = "Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_CrossingUnderlinings()
        {
            var text = "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
        
        [Test]
        public void Main_ReturnCorrectHtmlString_NoSymbolsBetweenUnderlinings()
        {
            var text = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";
            var textParser = new TextParser(tokenGetters);
            var htmlConverter = new HTMLConverter(tokenConverters);
            var expectedLine = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.";

            var textTokens = textParser.GetTextTokens(text);
            var actualLine = htmlConverter.GetHTMLString(textTokens);

            actualLine.Should().BeEquivalentTo(expectedLine);
        }
    }
}