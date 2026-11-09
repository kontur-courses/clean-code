using FluentAssertions;
using Markdown.Tokens;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class InlineTokenizer_Tests
    {
        [Test]
        public void InlineTokenizer_Tokenize_ReturnsExpectedTokensSmallTests()
        {
            InlineTokenizer.Tokenize("Heading").Should().Equal([new TextToken("Heading")]);
            InlineTokenizer.Tokenize("Heading _With em text_").Should().Equal(new Token[] {
                new TextToken("Heading "),
                new UnderscoreToken(1, ' ', 'W'),
                new TextToken("With em text"),
                new UnderscoreToken(1, 't', null)
            });
            InlineTokenizer.Tokenize("Heading __With strong text__").Should().Equal(new Token[] {
                new TextToken("Heading "),
                new UnderscoreToken(2, ' ', 'W'),
                new TextToken("With strong text"),
                new UnderscoreToken(2, 't', null)
            });
            InlineTokenizer.Tokenize("abc _em_ __strong__").Should().Equal(new Token[] {
                new TextToken("abc "),
                new UnderscoreToken(1, ' ', 'e'),
                new TextToken("em"),
                new UnderscoreToken(1, 'm', ' '),
                new TextToken(" "),
                new UnderscoreToken(2, ' ', 's'),
                new TextToken("strong"),
                new UnderscoreToken(2, 'g', null)
            });

            InlineTokenizer.Tokenize(@"\__вот это будет выделено Em, а не Strong тегом_\_").Should().Equal(new Token[]
            {
                new EscapedToken('_'),
                new UnderscoreToken(1, '_', 'в', true, false),
                new TextToken(@"вот это будет выделено Em, а не Strong тегом"),
                new UnderscoreToken(1, 'м', '\\', false, true),
                new EscapedToken('_')
            });

            InlineTokenizer.Tokenize("").Should().BeEmpty();
            InlineTokenizer.Tokenize("   ").Should().Equal(new TextToken("   "));
        }

        [Test]
        public void InlineTokenizer_Tokenize_ReturnsExpectedTokensTestsWIthLinks()
        {
            InlineTokenizer.Tokenize("[abc](qwerty)").Should().Equal(new Token[] {
                new BracketOpenToken(),
                new TextToken(@"abc"),
                new LinkTailToken(@"qwerty", true)
            });
            InlineTokenizer.Tokenize("[abc](qwer").Should().Equal(new Token[] {
                new BracketOpenToken(),
                new TextToken(@"abc"),
                new LinkTailToken(@"qwer", false)
            });
            InlineTokenizer.Tokenize("[abc]").Should().Equal(new Token[] {
                new BracketOpenToken(),
                new TextToken(@"abc]")
            });
            InlineTokenizer.Tokenize("[abc](qw_ert_y)").Should().Equal(new Token[] {
                new BracketOpenToken(),
                new TextToken(@"abc"),
                new LinkTailToken(@"qw_ert_y", true)
            });
            InlineTokenizer.Tokenize("[_abc_](qwerty)").Should().Equal(new Token[] {
                new BracketOpenToken(),
                new UnderscoreToken(1, '[', 'a'),
                new TextToken(@"abc"),
                new UnderscoreToken(1, 'c', ']'),
                new LinkTailToken(@"qwerty", true)
            });
            InlineTokenizer.Tokenize(@"\[abc](qwerty)").Should().Equal(new Token[] {
                new EscapedToken('['),
                new TextToken(@"abc"),
                new LinkTailToken(@"qwerty", true)
            });
            InlineTokenizer.Tokenize(@"\\[abc](qwerty)").Should().Equal( new Token[] {
                new EscapedToken('\\'),
                new BracketOpenToken(),
                new TextToken("abc"),
                new LinkTailToken("qwerty", true) 
            });
        }

        [Test]
        public void InlineTokenizer_Tokenize_ReturnsExpectedTokensRealTests()
        {
            InlineTokenizer.Tokenize(@"Спецификация языка разметки")
                .Should().Equal([new TextToken(@"Спецификация языка разметки")]);
            InlineTokenizer.Tokenize(@"Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github. Все совпадения случайны ;)")
                .Should().Equal([new TextToken(@"Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github. Все совпадения случайны ;)")]);
            InlineTokenizer.Tokenize(@"Курсив")
                .Should().Equal([new TextToken(@"Курсив")]);
            InlineTokenizer.Tokenize(@"Текст, _окруженный с двух сторон_ одинарными символами подчерка, должен помещаться в HTML-тег \<em> вот так:")
                .Should().Equal(new Token[] {
                    new TextToken(@"Текст, "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"окруженный с двух сторон"),
                    new UnderscoreToken(1, 'н', ' '),
                    new TextToken(@" одинарными символами подчерка, должен помещаться в HTML-тег \<em> вот так:")
                    });
            InlineTokenizer.Tokenize(@"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка, должен помещаться в HTML-тег \<em>.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка, должен помещаться в HTML-тег \<em>.")
                });
            InlineTokenizer.Tokenize(@"Полужирный")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Полужирный")
                });
            InlineTokenizer.Tokenize(@"__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега \<strong>.")
                .Should().Equal(new Token[]
                {
                    new UnderscoreToken(2, null, 'В'),
                    new TextToken(@"Выделенный двумя символами текст"),
                    new UnderscoreToken(2, 'т', ' '),
                    new TextToken(@" должен становиться полужирным с помощью тега \<strong>.")
                });
            InlineTokenizer.Tokenize(@"Экранирование")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Экранирование")
                });
            InlineTokenizer.Tokenize(@"Любой символ можно экранировать, чтобы он не считался частью разметки. \_Вот это\_, не должно выделиться тегом \<em>.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Любой символ можно экранировать, чтобы он не считался частью разметки. "),
                    new EscapedToken('_'),
                    new TextToken(@"Вот это"),
                    new EscapedToken('_'),
                    new TextToken(@", не должно выделиться тегом \<em>.")
                });
            InlineTokenizer.Tokenize(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\"),
                });
            InlineTokenizer.Tokenize(@"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ \<em>")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Символ экранирования тоже можно экранировать: "),
                    new EscapedToken('\\'),
                    new UnderscoreToken(1, '\\', 'в', true, false),
                    new TextToken(@"вот это будет выделено тегом"),
                    new UnderscoreToken(1, 'м', ' '),
                    new TextToken(@" \<em>")
                });
            InlineTokenizer.Tokenize(@"Взаимодействие тегов")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Взаимодействие тегов")
                });
            InlineTokenizer.Tokenize(@"Внутри __двойного выделения _одинарное_ тоже__ работает.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Внутри "),
                    new UnderscoreToken(2, ' ', 'д'),
                    new TextToken(@"двойного выделения "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"одинарное"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" тоже"),
                    new UnderscoreToken(2, 'е', ' '),
                    new TextToken(@" работает.")
                });
            InlineTokenizer.Tokenize(@"Но не наоборот — внутри _одинарного __двойное__ не_ работает.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Но не наоборот — внутри "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"одинарного "),
                    new UnderscoreToken(2, ' ', 'д'),
                    new TextToken(@"двойное"),
                    new UnderscoreToken(2, 'е', ' '),
                    new TextToken(@" не"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" работает."),
                });
            InlineTokenizer.Tokenize(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка."),
                });
            InlineTokenizer.Tokenize(@"Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Однако выделять часть слова они могут: и в "),
                    new UnderscoreToken(1, ' ', 'н'),
                    new TextToken(@"нач"),
                    new UnderscoreToken(1, 'ч', 'а'),
                    new TextToken(@"але, и в сер"),
                    new UnderscoreToken(1, 'р', 'е'),
                    new TextToken(@"еди"),
                    new UnderscoreToken(1, 'и', 'н'),
                    new TextToken(@"не, и в кон"),
                    new UnderscoreToken(1, 'н', 'ц'),
                    new TextToken(@"це."),
                    new UnderscoreToken(1, '.', null)
                });
            InlineTokenizer.Tokenize(@"В то же время выделение в ра_зных сл_овах не работает.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"В то же время выделение в ра"),
                    new UnderscoreToken(1, 'а', 'з'),
                    new TextToken(@"зных сл"),
                    new UnderscoreToken(1, 'л', 'о'),
                    new TextToken(@"овах не работает."),
                });
            InlineTokenizer.Tokenize(@"__Непарные_ символы в рамках одного абзаца не считаются выделением.")
                .Should().Equal(new Token[]
                {
                    new UnderscoreToken(2, null, 'Н'),
                    new TextToken(@"Непарные"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" символы в рамках одного абзаца не считаются выделением."),
                });
            InlineTokenizer.Tokenize(@"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" подчерки"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" не считаются выделением и остаются просто символами подчерка."),
                });
            InlineTokenizer.Tokenize(@"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти "),
                    new UnderscoreToken(1, ' ', 'п'),
                    new TextToken(@"подчерки "),
                    new UnderscoreToken(1, ' ', 'н'),
                    new TextToken(@"не считаются"),
                    new UnderscoreToken(1, 'я', ' '),
                    new TextToken(@" окончанием выделения и остаются просто символами подчерка.")
                });
            InlineTokenizer.Tokenize(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"В случае "),
                    new UnderscoreToken(2, ' ', 'п'),
                    new TextToken(@"пересечения "),
                    new UnderscoreToken(1, ' ', 'д'),
                    new TextToken(@"двойных"),
                    new UnderscoreToken(2, 'х', ' '),
                    new TextToken(@" и одинарных"),
                    new UnderscoreToken(1, 'х', ' '),
                    new TextToken(@" подчерков ни один из них не считается выделением.")
                });
            InlineTokenizer.Tokenize(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка."),
                });
            InlineTokenizer.Tokenize(@"Заголовки")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Заголовки"),
                });
            InlineTokenizer.Tokenize(@"Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок. В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок. В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами."),
                });
            InlineTokenizer.Tokenize(@"Таким образом")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Таким образом"),
                });
            InlineTokenizer.Tokenize(@"Заголовок __с _разными_ символами__")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"Заголовок "),
                    new UnderscoreToken(2, ' ', 'с'),
                    new TextToken(@"с "),
                    new UnderscoreToken(1, ' ', 'р'),
                    new TextToken(@"разными"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" символами"),
                    new UnderscoreToken(2, 'и', null)
                });
            InlineTokenizer.Tokenize(@"превратится в:")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"превратится в:"),
                });
            InlineTokenizer.Tokenize(@"\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>")
                .Should().Equal(new Token[]
                {
                    new TextToken(@"\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>"),
                });
        }
    }
}
