using System;
using System.Security.Cryptography;
using Markdown.Interfaces;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Tests
    {
        public Md md;
        [SetUp]
        public void Setup()
        {
            md = new Md(new HtmlConverter());
        }
        [TestCase("_text text text_", ExpectedResult = "<em>text text text</em>", TestName = "{m}_Return_TextInItalicTag_OnSingleUnderscores")]
        [TestCase("__text text text__", ExpectedResult = "<strong>text text text</strong>", TestName = "{m}_Return_Text_In_StrongTag_On_DoubleUnderscores")]
        [TestCase("#a\n", ExpectedResult = "<h1>a</h1>\n", TestName = "{m}Return_Text_in_h1_Tag_On_Line_Starts_With_Sharp")]
        [TestCase(@"\_Вот это\_", ExpectedResult = "_Вот это_", TestName = "{m}_Return_Text_With_Shielding_Tag")]
        [TestCase(@"сим\волы экранирования\ \должны остаться.\", ExpectedResult = @"сим\волы экранирования\ \должны остаться.\", TestName = "Escaping_Char_Not_Disappear_From_Result_Unless_It_Escapes")]
        [TestCase(@"\\_вот это будет выделено тегом\\_", ExpectedResult = "<em>вот это будет выделено тегом</em>", TestName = "Escape_Char_Can_Escaped")]
        [TestCase("_12_3", ExpectedResult = "_12_3" , TestName = "Underscores_In_Text_With_Numbers_Not_Tagged")]
        [TestCase("____", ExpectedResult = "____", TestName = "TagSymbols_On_EmptyText_Still_Symbols")]
        [TestCase("в _нач_але", ExpectedResult = "в <em>нач</em>але", TestName = "{m}_Tagged_Part_Word_At_Start")]
        [TestCase("сер_еди_не", ExpectedResult = "сер<em>еди</em>не", TestName = "{m}_Tagged_Part_Word_At_Middle")]
        [TestCase("кон_це_", ExpectedResult = "кон<em>це</em>", TestName = "{m}_Tagged_Part_Word_At_End")]
        [TestCase("ра_зных сл_овах", ExpectedResult = "ра_зных сл_овах", TestName = "Tagged_In_Different_Words_Not_Work.")]
        [TestCase("__пересечения _двойных__ и одинарных_", ExpectedResult = "__пересечения _двойных__ и одинарных_", TestName = "Intersections_DoubleUnderscores_And_SingleUnderscores_NotTagged")]
        [TestCase("__Непарные_ символы", ExpectedResult = "__Непарные_ символы", TestName = "Unpaired_Characters_Not_Considered_Tags")]
        [TestCase("#Заголовок __с _разными_ символами__", ExpectedResult = "<h1>Заголовок <strong>с <em>разными</em> символами</strong></h1>", TestName = "Header_Can_Contain_Other_Markup_Elements")]
        [TestCase("_aaa__b__ccc_", ExpectedResult = "<em>aaa</em><em>b</em><em>ccc</em>", TestName = "{m}_DoubleUnderscoreInUnderscore")]
        [TestCase("__a_b___", ExpectedResult = "<strong>a<em>b</em></strong>", TestName = "{m}_UnderscoreInDoubleUnderscore")]
        [TestCase("#__a_b___\n", ExpectedResult = "<h1><strong>a<em>b</em></strong></h1>\n", TestName = "{m}_TagsInHeader")]
        public string Render_Should(string line)
        {
            var result = md.Render(line);
            TestContext.WriteLine(result);
            return result;
        }
        [Test]
        public void Render_Should_Fail_OnNull()
        {
            Action action = () => md.Render(null);
            Assert.Throws<ArgumentNullException>(() => action());
        }
    }
}