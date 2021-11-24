using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTest
{
    [TestFixture]
    public class MdShould
    {
        private Md md = new Md();
        public void SetUp()
        {
        }

        [Test]
        public void ThrowException_WhenTextIsEmpty()
        {
            Assert.Throws<ArgumentException>(() =>
                md.Render(""));
        }

        [Test]
        public void ThrowException_WhenTextINull()
        {
            Assert.Throws<ArgumentException>(() =>
                md.Render(null));
        }


        [TestCase("# Заголовок1",
            "<h1> Заголовок1</h1>")]
        [TestCase("#Заголовок 1\r\n#Заголовок 2", 
            "<h1>Заголовок 1</h1>\r\n<h1>Заголовок 2</h1>")]
        [TestCase("#Заголовок __с _разными_ символами__",
            "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
        public void RenderHeader(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Не Заголовок1",
            "<div>Не Заголовок1</div>")]
        [TestCase("Не Заголовок 1\r\nНе Заголовок 2", 
            "<div>Не Заголовок 1</div>\r\n<div>Не Заголовок 2</div>")]
        [TestCase("#Заголовок1\r\nНе Заголовок 1\r\nНе Заголовок 2",
            "<h1>Заголовок1</h1>\r\n<div>Не Заголовок 1</div>\r\n<div>Не Заголовок 2</div>")]
        public void RenderParagraphs(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("__Выделенный двумя символами текст__ должен становиться полужирным",
            "<div><strong>Выделенный двумя символами текст</strong> должен становиться полужирным</div>")]
        public void RenderStrong(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка",
            "<div>Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка</div>")]
        public void RenderItalic(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает.",
            "<div>Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает.</div>")]

        [TestCase("Но не наоборот — внутри _одинарного __двойное__ не_ работает.",
            "<div>Но не наоборот — внутри <em>одинарного __двойное__ не</em> работает.</div>")]

        [TestCase("Подчерки внутри текста c цифрами_12_3 не считаются выделением",
            "<div>Подчерки внутри текста c цифрами_12_3 не считаются выделением</div>")]

        [TestCase("Однако выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._",
            "<div>Однако выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em></div>")]

        //[TestCase("В то же время выделение в ра_зных сл_овах не работает",
        //   "<div>В то же время выделение в ра_зных сл_овах не работает</div>")]
        
        [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением",
            "<div>__Непарные_ символы в рамках одного абзаца не считаются выделением</div>")]

        [TestCase("эти_ подчерки_ не считаются выделением",
            "<div>эти_ подчерки_ не считаются выделением</div>")]

        [TestCase("эти _подчерки _не считаются _окончанием выделения",
            "<div>эти _подчерки _не считаются _окончанием выделения</div>")]

        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением",
            "<div>В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением</div>")]

        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением",
            "<div>В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением</div>")]

        [TestCase("Если внутри подчерков пустая строка ____, то они остаются символами подчерка",
            "<div>Если внутри подчерков пустая строка ____, то они остаются символами подчерка</div>")]

        public void RenderTagsWithInteraction(string text, string expected)
        {
            TestRender(text, expected);
        }

        [TestCase("\\_Вот это\\_, не должно выделиться тегом",
            "<div>_Вот это_, не должно выделиться тегом</div>")]
        [TestCase("Здесь сим\\волы экранирования\\ \\должны остаться.\\",
            "<div>Здесь сим\\волы экранирования\\ \\должны остаться.\\</div>")]
        [TestCase("\\\\_вот это будет выделено тегом_",
            "<div><em>вот это будет выделено тегом</em></div>")]

        public void RenderTagsWithEscaping(string text, string expected)
        {
            TestRender(text, expected);
        }


        public void TestRender(string text, string expected)
        {
            var res = md.Render(text);
            res.Should().Be(expected);
        }
    }























    public class TestHelper
    {
        public List<Token> GetMdTokens(List<Token> tokens)
        {
            return GetConvertedTokens(StringToMd, tokens);
        }

        public List<Token> GetHtmlTokens(List<Token> tokens)
        {
            return GetConvertedTokens(StringToHtml, tokens);
        }

        private List<Token> GetConvertedTokens
            (Dictionary<Type, Func<string, string>> convertDict, List<Token> tokens)
        {
            var res = new List<Token>();
            foreach (var token in tokens)
            {
                var resToken = new Token(GetConvertedToken(convertDict, token));
                if (token.InnerTokens != null && token.InnerTokens.Count > 0)
                    resToken.InnerTokens = GetConvertedTokens(convertDict, token.InnerTokens);
                res.Add(resToken);
            }
            
            return res;
        }

        private string GetConvertedToken<T>
            (Dictionary<Type, Func<string, string>> convertDict,T token)
            where T : Token
        {
            return convertDict[token.GetType()](token.Value);
        }

        private Dictionary<Type, Func<string, string>> StringToMd =
            new Dictionary<Type, Func<string, string>>
            {
                {typeof(Header), s => "#" + s + "\r\n"},
                {typeof(Paragraph), s => s+ "\r\n"},
                {typeof(Token), s => s},
                {typeof(StrongText), s => "__" + s + "__"},
                {typeof(ItalicText), s => "_" + s + "_"}
            };

        private Dictionary<Type, Func<string, string>> StringToHtml =
            new Dictionary<Type, Func<string, string>>
            {
                {typeof(Header), s => "<h1>" + s + "</h1\r\n>"},
                {typeof(Paragraph), s => "<div>" + s + "</div>\r\n"},
                {typeof(Token), s => s},
                {typeof(StrongText), s => "<strong>" + s + "</strong>"},
                {typeof(ItalicText), s => "<em>" + s + "</em>"}
            };
    }
}
