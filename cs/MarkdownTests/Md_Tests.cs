using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Converter;
using Markdown.Parser;
using Markdown.Tags;
using Markdown.Validator;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Md_Tests
    {
        private Md markdown;
        
        [SetUp]
        public void SetUp()
        {
            var availableTags = new List<Tag>()
            {
                new BoldTag(), new HeaderTag(), new ItalicsTag()
            };
            
            var parser = new TextParser(availableTags);
            var validator = new TokenValidator();
            var converter = new TokenConverter(availableTags);

            markdown = new Md(parser, validator, converter);
        }

        [Test]
        public void Render_ReturnsCorrectString_WhenItalicsTagInText()
        {
            var text = "Текст, _окруженный с двух сторон_ одинарными символами подчерка,";
            var expectedText = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка,";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenBoldTagInText()
        {
            var text = "__Выделенный двумя символами текст__ должен становиться полужирным";
            var expectedText = "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagIsEscaped()
        {
            var text = @"\_Вот это\_, не должно выделиться тегом";
            var expectedText = "_Вот это_, не должно выделиться тегом";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void Render_ReturnsCorrectString_WhenEscapeCharsInText()
        {
            var text = @"Здесь сим\волы экранирования\ \должны остаться.\";
            var expectedText = @"Здесь сим\волы экранирования\ \должны остаться.\";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenEscapeCharIsEscaped()
        {
            var text = @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_";
            var expectedText = @"Символ экранирования тоже можно экранировать: \<em>вот это будет выделено тегом</em>";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagsAreNested()
        {
            var text = "Внутри __двойного выделения _одинарное_ тоже__ работает.";
            var expectedText = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void Render_ReturnsCorrectString_WhenTagsAreWronglyNested()
        {
            var text = "Но не наоборот — внутри _одинарного __двойное__ не_ работает.";
            var expectedText = "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }

        [Test]
        public void Render_ReturnsCorrectString_WhenDigitsInsidePairTags()
        {
            var text = 
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";
            var expectedText = 
                "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagHighlightsPartOfWord()
        {
            var text = "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._";
            var expectedText = 
                "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagsInsideDifferentWords()
        {
            var text = "В то же время выделение в ра_зных сл_овах не работает.";
            var expectedText = "В то же время выделение в ра_зных сл_овах не работает.";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenNonPairTagsMeet()
        {
            var text = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";
            var expectedText = "__Непарные_ символы в рамках одного абзаца не считаются выделением.";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenWhiteSpaceAfterTagStart()
        {
            var text = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением ";
            var expectedText = "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением ";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenWhiteSpaceBeforeTagEnd()
        {
            var text = "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения  ";
            var expectedText = "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти <em>подчерки _не считаются</em> окончанием выделения  ";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagsIntersect()
        {
            var text = 
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.  ";
            var expectedText = 
                "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.  ";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenTagIsEmpty()
        {
            var text = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.  ";
            var expectedText = "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.  ";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
        
        [Test]
        public void Render_ReturnsCorrectString_WhenDifferentTagsInsideHeader()
        {
            var text = "# Заголовок __с _разными_ символами__";
            var expectedText = "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>";

            var renderedText = markdown.Render(text);

            renderedText.Should().BeEquivalentTo(expectedText);
        }
    }
}