﻿using System;
using System.Diagnostics;
using FluentAssertions;
using Markdown.TagRenderer;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MarkdownConverterTests
    {
        private MarkdownConverter sut;

        [SetUp]
        public void SetUp()
        {
            var lexer = new Lexer.Lexer();
            var parser = new TokenParser.TokenParser();
            var renderer = new HtmlTagRenderer();
            sut = new MarkdownConverter(lexer, parser, renderer);
        }

        [TestCase("", "")]
        [TestCase(
            "Текст, _окруженный с двух сторон_ одинарными символами подчерка, должен помещаться в HTML-тег <em>",
            "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка, должен помещаться в HTML-тег <em>"
        )]
        [TestCase(
            "__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>",
            "<strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега <strong>"
        )]
        [TestCase(
            "Любой символ можно экранировать, чтобы он не считался частью разметки. \\_Вот это\\_, не должно выделиться тегом <em>",
            "Любой символ можно экранировать, чтобы он не считался частью разметки. _Вот это_, не должно выделиться тегом <em>"
        )]
        [TestCase(
            "Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            "Символ экранирования исчезает из результата, только если экранирует что-то. Здесь сим\\волы экранирования\\ \\должны остаться.\\"
        )]
        [TestCase(
            @"Символ экранирования тоже можно экранировать: \\_вот это будет выделено тегом_ <em>",
            @"Символ экранирования тоже можно экранировать: \<em>вот это будет выделено тегом</em> <em>"
        )]
        [TestCase(
            "Внутри __двойного выделения _одинарное_ тоже__ работает.",
            "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает."
        )]
        [TestCase(
            "Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
            "Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает."
        )]
        [TestCase(
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.",
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка."
        )]
        [TestCase(
            "Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це_.",
            "Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це</em>."
        )]
        [TestCase(
            "В то же время выделение в ра_зных сл_овах не работает.",
            "В то же время выделение в ра_зных сл_овах не работает."
        )]
        [TestCase(
            "__Непарные_ символы в рамках одного абзаца не считаются выделением.",
            "__Непарные_ символы в рамках одного абзаца не считаются выделением."
        )]
        [TestCase(
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка.",
            "За подчерками, начинающими выделение, должен следовать непробельный символ. Иначе эти_ подчерки_ не считаются выделением и остаются просто символами подчерка."
        )]
        [TestCase(
            "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка.",
            "Подчерки, заканчивающие выделение, должны следовать за непробельным символом. Иначе эти _подчерки _не считаются_ окончанием выделения и остаются просто символами подчерка."
        )]
        [TestCase(
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.",
            "В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением."
        )]
        [TestCase(
            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка.",
            "Если внутри подчерков пустая строка ____, то они остаются символами подчерка."
        )]
        [TestCase(
            "# Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>"
        )]
        [TestCase(
            "[__Ссылка__ с _форматированием_](http://markdown.com)",
            "<a href=\"http://markdown.com\"><strong>Ссылка</strong> с <em>форматированием</em></a>"
        )]
        public void Render_ShouldReturnCorrectText(string mdText, string htmlText)
        {
            var actualHtmlText = sut.Render(mdText);

            actualHtmlText.Should().Be(htmlText);
        }

        [TestCase(10_000, 500)]
        public void Render_ShouldBeFast(int count, int maxAverageMilliseconds)
        {
            const string text
                = "# Header _then __bold__ with cursive_ and, \n then new line. Dot before __on__ly b_ol_d o_r_?";

            GC.Collect();
            var sw = Stopwatch.StartNew();
            for (var i = 0; i < count; i++) sut.Render(text);

            sw.Stop();
            var measure = sw.ElapsedMilliseconds / count;
            measure.Should().BeLessOrEqualTo(maxAverageMilliseconds);
        }
    }
}