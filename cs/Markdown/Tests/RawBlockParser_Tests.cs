using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class RawBlockParser_Tests
    {
        [Test]
        public void RawBlockParser_Parse_ReturnsExpectedRawBlocksInSmallTests()
        {
            RawBlockParser.Parse("# Heading").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Heading, "Heading")]);
            RawBlockParser.Parse("# Heading _With em text_").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Heading, "Heading _With em text_")]);
            RawBlockParser.Parse("# Heading __With strong text__").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Heading, "Heading __With strong text__")]);
            RawBlockParser.Parse("abc").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Paragraph, "abc")]);
            RawBlockParser.Parse("abc _em_ __strong__").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Paragraph, "abc _em_ __strong__")]);
            RawBlockParser.Parse("# Заголовок\nА тут обычный параграф").Should().BeEquivalentTo(
                [new RawBlock(BlockType.Heading, "Заголовок"), new RawBlock(BlockType.Paragraph, "А тут обычный параграф")]);
            RawBlockParser.Parse("# Заголовок\nА тут обычный параграф\n# И снова заголовок").Should().BeEquivalentTo(
                [
                new RawBlock(BlockType.Heading, "Заголовок"),
                new RawBlock(BlockType.Paragraph, "А тут обычный параграф"),
                new RawBlock(BlockType.Heading, "И снова заголовок")
                ]);
        }

        [Test]
        public void RawBlockParser_Parse_ReturnsExpectedRawBlocksInRealTests()
        {
            string input = File.ReadAllText("RealFileTests\\methodical_text.txt");
            var expected = new RawBlock[]
            {
                new RawBlock(BlockType.Heading, @"Спецификация языка разметки"),
                new RawBlock(BlockType.Paragraph, @"Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github. Все совпадения случайны ;)"),
                new RawBlock(BlockType.Heading, @"Курсив"),
                new RawBlock(BlockType.Paragraph, @"Текст, _окруженный с двух сторон_ одинарными символами подчерка, должен помещаться в HTML-тег \<em> вот так:"),
                new RawBlock(BlockType.Paragraph, @"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка, должен помещаться в HTML-тег \<em>."),
                new RawBlock(BlockType.Heading, @"Полужирный"),
                new RawBlock(BlockType.Paragraph, @"__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \<strong>."),
                new RawBlock(BlockType.Heading, @"Экранирование"),
                new RawBlock(BlockType.Paragraph, @"Любой символ можно экранировать, чтобы он не считался частью разметки. \_Вот это\_, не должно выделиться тегом \<em>."),
                new RawBlock(BlockType.Paragraph, @"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\"),
                new RawBlock(BlockType.Paragraph, @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ \<em>"),
                new RawBlock(BlockType.Heading, @"Взаимодействие тегов"),
                new RawBlock(BlockType.Paragraph, @"Внутри __двойного выделения _одинарное_ тоже__ работает."),
                new RawBlock(BlockType.Paragraph, @"Но не наоборот — внутри _одинарного __двойное__ не_ работает."),
                new RawBlock(BlockType.Paragraph, @"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка."),
                new RawBlock(BlockType.Paragraph, @"Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._"),
                new RawBlock(BlockType.Paragraph, @"В то же время выделение в ра_зных сл_овах не работает."),
                new RawBlock(BlockType.Paragraph, @"__Непарные_ символы в рамках одного абзаца не считаются выделением."),
                new RawBlock(BlockType.Paragraph, @"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка."),
                new RawBlock(BlockType.Paragraph, @"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка."),
                new RawBlock(BlockType.Paragraph, @"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."),
                new RawBlock(BlockType.Paragraph, @"Если внутри подчерков пустая строка ____, то они остаются символами подчерка."),
                new RawBlock(BlockType.Heading, @"Заголовки"),
                new RawBlock(BlockType.Paragraph, @"Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок. В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами."),
                new RawBlock(BlockType.Paragraph, @"Таким образом"),
                new RawBlock(BlockType.Heading, @"Заголовок __с _разными_ символами__"),
                new RawBlock(BlockType.Paragraph, @"превратится в:"),
                new RawBlock(BlockType.Paragraph, @"\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>"),
            };

            RawBlockParser.Parse(input).Should().BeEquivalentTo(expected);
        }
    }
}
