using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTest
{
    public class MdTests
    {
        [TestCase("_a_", "<em>a</em>", TestName = "Word is highlighted by tag when word is key in terms")]
        [TestCase("__", "__", TestName = "Term does not become tag when there is nothing in it")]
        [TestCase("_aa", "_aa", TestName = "Word is not tagged when term is not closed")]
        [TestCase("a_sds_d", "a<em>sds</em>d", TestName = "Part of word is highlighted with tag when term is closed inside word")]
        [TestCase("_a b_", "<em>a b</em>", TestName = "Multiple words are highlighted with tag when term is behind word")]
        [TestCase("a_s d_d", "a_s d_d", TestName = "Part of word is not highlighted with tag when opening and closing terms are inside different words")]
        [TestCase("_s d_d", "_s d_d", TestName = "Input is not highlighted with tag when only closing term is inside word")]
        [TestCase("a_s d_", "a_s d_", TestName = "Input is not highlighted with tag when only opening term is inside word")]
        [TestCase("_asd _", "_asd _", TestName = "Input is not tagged when space is before closing tag")]
        [TestCase("_ sdf_", "_ sdf_", TestName = "Input is not tagged when space is after opening tag")]
        [TestCase("dfdf _sdf_ dfsdf _dddd_ df", "dfdf <em>sdf</em> dfsdf <em>dddd</em> df", TestName = "Terms in sentence are tagged when they are closed")]
        [TestCase("dfdf _sdf_ dfsdf _dddd df", "dfdf <em>sdf</em> dfsdf _dddd df", TestName = "Closed terms in sentence are highlighted with tag when there is open after closed one")]
        [TestCase("dfdf _sdf dfsdf _dddd_ df", "dfdf _sdf dfsdf <em>dddd</em> df", TestName = "Closed terms in sentence are highlighted with tag when there is closed one after open one")]
        [TestCase("df_123_2", "df_123_2", TestName = "Numbers enclosed in term are not tagged when term is inside word")]
        [TestCase("_123_", "<em>123</em>", TestName = "Numbers enclosed in term are highlighted with tag when word is inside term")]
        //[TestCase("_a_ _b_", "<em>a</em> <em>b</em>", TestName = "два тега друг за другом")]
        //[TestCase("__ _b_", "__ <em>b</em>", TestName = "два тега друг за другом")]
        //[TestCase("_", "_", TestName = "только подчеркивание")]
        //[TestCase("a_", "a_", TestName = "подчеркивание после слова")]



        [TestCase("__asd _aa_ dfdf__", "<strong>asd <em>aa</em> dfdf</strong>", TestName = "Terms become tags when singles inside doubles")]
        [TestCase("_a __a__ d_", "<em>a __a__ d</em>", TestName = "Internal terms do not turn into tags when doubles inside singles")]
        //[TestCase("_asd __aa__ dfdf", "_asd <strong>aa</strong> dfdf", TestName = "двойные после незакрытой одинарной")]
        [TestCase("_asd __aa_ dfdf__", "_asd __aa_ dfdf__", TestName = "Terms do not become tags when doubles and singles intersect")]
        [TestCase("_asd__", "_asd__", TestName = "Word is not highlighted in tag when terms are unpaired")]

        [TestCase(@"\asd \n \t", @"\asd \n \t", TestName = "Escaping character is displayed when nothing is escaping")]
        [TestCase(@"\_a_", @"_a_", TestName = "Escaping character is not displayed when escaping underscore")]
        [TestCase(@"\__a__", @"__a__", TestName = "Escaping character is not displayed when escaping double underscore")]
        [TestCase(@"\\", @"\", TestName = "Escaping character is not displayed when escaping itself")]
        [TestCase(@"\#", @"#", TestName = "Escaping character is not displayed when escaping header term")]

        [TestCase("#asfdf asd", "<h1>asfdf asd</h1>", TestName = "Sentence is highlighted in title tag when title term is at beginning")]
        [TestCase("aa #asfdf asd", "aa #asfdf asd", TestName = "Sentence is not highlighted in title tag when title term is not at beginning")]
        //[TestCase("#_asfdf_ asd", "<h1><em>asfdf</em> asd</h1>", TestName = "заголовок содержащий курсив")]
        //[TestCase("#__as _asfdf_ as__", "<h1><strong>as <em>asfdf</em> as</strong></h1>", TestName = "максимальный уровень вложенности")]
      
        public void CheckHeaderOnly(string input, string asertResult)
        {
            var html = Md.Render(input);

            html.Should().Be(asertResult);
        }
    }
}  