using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parsers.Tokens;
using Markdown.Parsers.Tokens.Tags;
using Markdown.Parsers.Tokens.Tags.Enum;
using Markdown.Parsers.Tokens.Tags.Markdown;
using Markdown.Renderers;
using NUnit.Framework;


namespace Markdown
{

    public class Tests
    {
        [Test]
        public void ConvertMarkdownToHtml()
        {
            var a = new List<IToken>
            {
                new MdItalicTag(TagPosition.Start),
                new TextToken("abc"),
                new MdItalicTag(TagPosition.End)
            };
            var b = a.ConvertAll(tag => tag.ToHtml());
            Assert.AreEqual("<em>", b.First().ToString());
            Assert.AreEqual("abc", b.Skip(1).First().ToString());
            Assert.AreEqual("</em>", b.Skip(2).First().ToString());
        }

        [TestCase("", ExpectedResult = "", TestName = "Empty text")]
        [TestCase(null, ExpectedResult = null, TestName = "Null text")]
        [TestCase("__t_t_t__", ExpectedResult = "<strong>t<em>t</em>t</strong>", TestName = "Text1")]
        [TestCase("__t_t_t_t__", ExpectedResult = "<strong>t<em>t</em>t_t</strong>", TestName = "Text2")]
        [TestCase("__t_t___t__", ExpectedResult = "<strong>t_t___t</strong>", TestName = "Text3")]
        [TestCase(@"\__t_t___t__", ExpectedResult = @"__t_t___t__", TestName = "Text4")]
        //TODO: проанализировать, это нарушает логику
        //[TestCase(@"внутри _одинарного __двойное__ не_ работает", ExpectedResult = @"внутри _одинарного __двойное__ не_ работает", TestName = "Text5")]
        
        //[TestCase("#__t_t_t_t__", ExpectedResult = "#<strong>t<em>t</em>t_t</strong>", TestName = "Text2 with header")]
        [TestCase("# __t_t_t_t__", ExpectedResult = "<h1><strong>t<em>t</em>t_t</strong></h1>", TestName = "Text2 with header 2")]

        [TestCase("Текст, _окруженный с двух сторон_ одинарными символами подчерка", ExpectedResult = "Текст, <em>окруженный с двух сторон</em> одинарными символами подчерка", TestName = "Italic 1")]
        [TestCase("внутри текста c цифрами_12_3 не считаются выделением", ExpectedResult = "внутри текста c цифрами_12_3 не считаются выделением", TestName = "Italic 2")]
        [TestCase("Можно выделять часть слова они могут: и в _нач_але, и в сер_еди_не, и в кон_це._", ExpectedResult = "Можно выделять часть слова они могут: и в <em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>", TestName = "Italic 3")]
        
        [TestCase(@"__Выделенный двумя символами текст__ должен становиться полужирным с помощью тега <strong>.", ExpectedResult = @"<strong>Выделенный двумя символами текст</strong> должен становиться полужирным с помощью тега <strong>.", TestName = "Bold 1")]

        //[TestCase(@"\_Вот это\_, не должно выделиться тегом <em>", ExpectedResult = @"_Вот это_, не должно выделиться тегом <em>", TestName = "Comment 1")]
        [TestCase(@"Здесь сим\волы экранирования\ \должны остаться.\", ExpectedResult = @"Здесь сим\волы экранирования\ \должны остаться.\", TestName = "Comment 2")]
        public string MdRender_ConvertText(string markdownText)
        {
            var md = new Md(new HtmlRenderer());
            return md.Render(markdownText);
        }

    }
}
