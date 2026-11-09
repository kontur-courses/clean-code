using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class InlineResolver_Tests
    {
        private static void AssertPieces(InlinePiece[] actual, InlinePiece[] expected)
        {
            Assert.That(actual, Has.Length.EqualTo(expected.Length),
                $"ожидалось {expected.Length} pieces, получено {actual.Length}");
            for (int i = 0; i < expected.Length; i++)
                AssertPiece(actual[i], expected[i]);
        }

        private static void AssertPiece(InlinePiece actual, InlinePiece expected)
        {
            switch (expected)
            {
                case TextInlinePiece t:
                    actual.Should().BeOfType<TextInlinePiece>()
                        .Which.Text.Should().Be(t.Text);
                    break;

                case EmInlinePiece e:
                    AssertPieces(actual.As<EmInlinePiece>().Children, e.Children);
                    break;

                case StrongInlinePiece s:
                    AssertPieces(actual.As<StrongInlinePiece>().Children, s.Children);
                    break;

                case LinkInlinePiece l:
                    actual.Should().BeOfType<LinkInlinePiece>()
                        .Which.Url.Should().Be(l.Url);
                    AssertPieces(actual.As<LinkInlinePiece>().Children, l.Children);
                    break;
            }
        }


        [Test]
        public void InlineResolver_Resolve_ReturnsExpectedPiecesSmallTests()
        {
            AssertPieces(InlineResolver.Resolve(new Token[] {
                new TextToken("Heading")
            }),
            new InlinePiece[]
            {
                new TextInlinePiece(@"Heading")
            });

            AssertPieces(InlineResolver.Resolve(new Token[] {
                new TextToken("Heading "),
                new UnderscoreToken(1, ' ', 'W'),
                new TextToken("With em text"),
                new UnderscoreToken(1, 't', null)
                }),
                new InlinePiece[]
                {
                    new TextInlinePiece(@"Heading "),
                    new EmInlinePiece( new InlinePiece[] {new TextInlinePiece(@"With em text") })
                });

            AssertPieces(InlineResolver.Resolve(new Token[] {
                new TextToken("Heading "),
                new UnderscoreToken(2, ' ', 'W'),
                new TextToken("With strong text"),
                new UnderscoreToken(2, 't', null)
                }),
                new InlinePiece[]
                {
                    new TextInlinePiece(@"Heading "),
                    new StrongInlinePiece( new InlinePiece[] {new TextInlinePiece(@"With strong text") })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken("abc "),
                    new UnderscoreToken(1, ' ', 'e'),
                    new TextToken("em"),
                    new UnderscoreToken(1, 'm', ' '),
                    new TextToken(" "),
                    new UnderscoreToken(2, ' ', 's'),
                    new TextToken("strong"),
                    new UnderscoreToken(2, 'g', null)
                }),
                new InlinePiece[]
                {
                    new TextInlinePiece(@"abc "),
                    new EmInlinePiece( new InlinePiece[] {new TextInlinePiece(@"em") }),
                    new TextInlinePiece(@" "),
                    new StrongInlinePiece( new InlinePiece[] {new TextInlinePiece(@"strong") })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new EscapedToken('_'),
                    new UnderscoreToken(1, '_', 'в', true, false),
                    new TextToken(@"вот это будет выделено Em, а не Strong тегом"),
                    new UnderscoreToken(1, 'м', '\\', false, true),
                    new EscapedToken('_')
                }),
                new InlinePiece[]
                {
                    new TextInlinePiece(@"_"),
                    new EmInlinePiece( new InlinePiece[] {new TextInlinePiece(@"вот это будет выделено Em, а не Strong тегом") }),
                    new TextInlinePiece(@"_")
                });

            InlineResolver.Resolve(
                new Token[] { })
                .Should().BeEmpty();
        }

        [Test]
        public void InlineResolver_Resolve_ReturnsExpectedPiecesTestsWithLinks()
        {
            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new BracketOpenToken(),
                    new TextToken(@"abc"),
                    new LinkTailToken(@"qwerty", true)
                }),
                new InlinePiece[]
                {
                    new LinkInlinePiece(@"qwerty", new InlinePiece[] {new TextInlinePiece(@"abc") })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new BracketOpenToken(),
                    new TextToken(@"abc"),
                    new LinkTailToken(@"qwer", false)
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"[abc](qwer")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new BracketOpenToken(),
                    new TextToken(@"abc]")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"[abc]")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new BracketOpenToken(),
                    new TextToken(@"abc"),
                    new LinkTailToken(@"qw_ert_y", true)
                })
                ,new InlinePiece[]
                {
                    new LinkInlinePiece(@"qw_ert_y", new InlinePiece[] {new TextInlinePiece(@"abc") })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new BracketOpenToken(),
                    new UnderscoreToken(1, '[', 'a'),
                    new TextToken(@"abc"),
                    new UnderscoreToken(1, 'c', ']'),
                    new LinkTailToken(@"qwerty", true)
                })
                ,new InlinePiece[]
                {
                    new LinkInlinePiece(@"qwerty", new InlinePiece[] {new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"abc") }) })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new EscapedToken('['),
                    new TextToken(@"abc"),
                    new LinkTailToken(@"qwerty", true)
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"[abc](qwerty)")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new EscapedToken('\\'),
                    new BracketOpenToken(),
                    new TextToken("abc"),
                    new LinkTailToken("qwerty", true)
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"\"),
                    new LinkInlinePiece(@"qwerty", new InlinePiece[] { new TextInlinePiece(@"abc") })
                });
        }

        [Test]
        public void InlineResolver_Resolve_ReturnsExpectedPiecesRealTests()
        {
            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Спецификация языка разметки")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Спецификация языка разметки"),
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github. Все совпадения случайны ;)")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Посмотрите этот файл в сыром виде. Сравните с тем, что показывает github. Все совпадения случайны ;)")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Курсив")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Курсив")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Текст, "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"окруженный с двух сторон"),
                    new UnderscoreToken(1, 'н', ' '),
                    new TextToken(@" одинарными символами подчерка, должен помещаться в HTML-тег \<em> вот так:")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Текст, "),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"окруженный с двух сторон") }),
                    new TextInlinePiece(@" одинарными символами подчерка, должен помещаться в HTML-тег \<em> вот так:")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка, должен помещаться в HTML-тег \<em>.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Текст, \<em>окруженный с двух сторон\</em> одинарными символами подчерка, должен помещаться в HTML-тег \<em>.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Полужирный")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Полужирный")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new UnderscoreToken(2, null, 'В'),
                    new TextToken(@"Выделенный двумя символами текст"),
                    new UnderscoreToken(2, 'т', ' '),
                    new TextToken(@" должен становиться полужирным с помощью тега \<strong>.")
                })
                ,new InlinePiece[]
                {
                    new StrongInlinePiece(new InlinePiece[] { new TextInlinePiece(@"Выделенный двумя символами текст") }),
                    new TextInlinePiece(@" должен становиться полужирным с помощью тега \<strong>.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Экранирование")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Экранирование")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Любой символ можно экранировать, чтобы он не считался частью разметки. "),
                    new EscapedToken('_'),
                    new TextToken(@"Вот это"),
                    new EscapedToken('_'),
                    new TextToken(@", не должно выделиться тегом \<em>.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Любой символ можно экранировать, чтобы он не считался частью разметки. _Вот это_, не должно выделиться тегом \<em>.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\волы экранирования\ \должны остаться.\")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Символ экранирования тоже можно экранировать: "),
                    new EscapedToken('\\'),
                    new UnderscoreToken(1, '\\', 'в', true, false),
                    new TextToken(@"вот это будет выделено тегом"),
                    new UnderscoreToken(1, 'м', ' '),
                    new TextToken(@" \<em>")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Символ экранирования тоже можно экранировать: \"),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"вот это будет выделено тегом") }),
                    new TextInlinePiece(@" \<em>")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Взаимодействие тегов")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Взаимодействие тегов")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Внутри "),
                    new UnderscoreToken(2, ' ', 'д'),
                    new TextToken(@"двойного выделения "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"одинарное"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" тоже"),
                    new UnderscoreToken(2, 'е', ' '),
                    new TextToken(@" работает.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Внутри "),
                    new StrongInlinePiece(new InlinePiece[] { 
                        new TextInlinePiece(@"двойного выделения "), 
                        new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"одинарное") }),
                        new TextInlinePiece(@" тоже")
                        }),
                    new TextInlinePiece(@" работает.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Но не наоборот — внутри "),
                    new UnderscoreToken(1, ' ', 'о'),
                    new TextToken(@"одинарного "),
                    new UnderscoreToken(2, ' ', 'д'),
                    new TextToken(@"двойное"),
                    new UnderscoreToken(2, 'е', ' '),
                    new TextToken(@" не"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" работает."),
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Но не наоборот — внутри "),
                    new EmInlinePiece(new InlinePiece[] {
                        new TextInlinePiece(@"одинарного __двойное__ не")
                        }),
                    new TextInlinePiece(@" работает.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
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
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Однако выделять часть слова они могут: и в "),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"нач") }),
                    new TextInlinePiece(@"але, и в сер"),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"еди") }),
                    new TextInlinePiece(@"не, и в кон"),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"це.") })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"В то же время выделение в ра"),
                    new UnderscoreToken(1, 'а', 'з'),
                    new TextToken(@"зных сл"),
                    new UnderscoreToken(1, 'л', 'о'),
                    new TextToken(@"овах не работает."),
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"В то же время выделение в ра_зных сл_овах не работает."),
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new UnderscoreToken(2, null, 'Н'),
                    new TextToken(@"Непарные"),
                    new UnderscoreToken(1, 'е', ' '),
                    new TextToken(@" символы в рамках одного абзаца не считаются выделением."),
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"__Непарные_ символы в рамках одного абзаца не считаются выделением."),
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" подчерки"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" не считаются выделением и остаются просто символами подчерка."),
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка."),
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти "),
                    new UnderscoreToken(1, ' ', 'п'),
                    new TextToken(@"подчерки "),
                    new UnderscoreToken(1, ' ', 'н'),
                    new TextToken(@"не считаются"),
                    new UnderscoreToken(1, 'я', ' '),
                    new TextToken(@" окончанием выделения и остаются просто символами подчерка.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки "),
                    new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"не считаются") }),
                    new TextInlinePiece(@" окончанием выделения и остаются просто символами подчерка.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"В случае "),
                    new UnderscoreToken(2, ' ', 'п'),
                    new TextToken(@"пересечения "),
                    new UnderscoreToken(1, ' ', 'д'),
                    new TextToken(@"двойных"),
                    new UnderscoreToken(2, 'х', ' '),
                    new TextToken(@" и одинарных"),
                    new UnderscoreToken(1, 'х', ' '),
                    new TextToken(@" подчерков ни один из них не считается выделением.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Если внутри подчерков пустая строка ____, то они остаются символами подчерка.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Заголовки")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Заголовки")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок. В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Абзац, начинающийся с ""# "", выделяется тегом \<h1> в заголовок. В тексте заголовка могут присутствовать все прочие символы разметки с указанными правилами.")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Таким образом")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Таким образом")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"Заголовок "),
                    new UnderscoreToken(2, ' ', 'с'),
                    new TextToken(@"с "),
                    new UnderscoreToken(1, ' ', 'р'),
                    new TextToken(@"разными"),
                    new UnderscoreToken(1, 'и', ' '),
                    new TextToken(@" символами"),
                    new UnderscoreToken(2, 'и', null)
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"Заголовок "),
                    new StrongInlinePiece(new InlinePiece[]
                    { 
                        new TextInlinePiece(@"с "),
                        new EmInlinePiece(new InlinePiece[] { new TextInlinePiece(@"разными") }),
                        new TextInlinePiece(@" символами")
                    })
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"превратится в:")
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"превратится в:")
                });

            AssertPieces(InlineResolver.Resolve(
                new Token[] {
                    new TextToken(@"\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>"),
                })
                ,new InlinePiece[]
                {
                    new TextInlinePiece(@"\<h1>Заголовок \<strong>с \<em>разными\</em> символами\</strong>\</h1>"),
                });
        }
    }
}
