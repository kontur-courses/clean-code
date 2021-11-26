using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTest
{
    public class MdTests
    {
        [TestCase("", "", TestName = "Empty input")]
        [TestCase(null, "", TestName = "Null in input")]

        [TestCase("aa", "aa", TestName = "Only one word")]
        [TestCase("aaa sdsd", "aaa sdsd", TestName = "Sentance without service symbols")]

        [TestCase("_a_", "<em>a</em>", TestName = "One word in italic tag")]
        [TestCase("__", "__", TestName = "Only bold term")]
        [TestCase("_aa", "_aa", TestName = "Opened italic term and word")]
        [TestCase("_a_sds_d_", "<em>a</em>sds<em>d</em>",
            TestName = "Part of word is highlighted with tag when term is closed inside word")]
        [TestCase("_a b_", "<em>a b</em>", TestName = "Multiple words in italic tag")]
        [TestCase("a_s d_d", "a_s d_d",
            TestName = "Opening and closing terms are inside different words")]
        [TestCase("_s d_d", "_s d_d",
            TestName = "Input is not highlighted with tag when only closing term is inside word")]
        [TestCase("a_s d_", "a_s d_",
            TestName = "Input is not highlighted with tag when only opening term is inside word")]
        [TestCase("_asd _", "_asd _",
            TestName = "Space before closed term")]
        [TestCase("_ sdf_", "_ sdf_",
            TestName = "Space after opened term")]
        [TestCase("dfdf _sdf_ dfsdf _dddd_ df", "dfdf <em>sdf</em> dfsdf <em>dddd</em> df",
            TestName = "Italic tags inside sentence")]
        [TestCase("dfdf _sdf_ dfsdf _dddd df", "dfdf <em>sdf</em> dfsdf _dddd df",
            TestName = "Italic tag before unclosed italic term")]
        [TestCase("dfdf _sdf dfsdf _dddd_ df", "dfdf _sdf dfsdf <em>dddd</em> df",
            TestName = "Italic tag after unclosed italic term")]
        [TestCase("df_123_2", "df_123_2", TestName = "Italic term in word with digits")]
        [TestCase("_123_", "<em>123</em>", TestName = "Number in italic tag")]
        [TestCase("_a_ _b_", "<em>a</em> <em>b</em>", TestName = "Only words in tags in sentence")]
        [TestCase("__ _b_", "__ <em>b</em>", TestName = "Open bold term and italic tag")]
        [TestCase("_", "_", TestName = "Only italic term")]
        [TestCase("a_", "a_", TestName = "Word and italic term")]

        [TestCase("__ aa__", "__ aa__", TestName = "Space after open bold tag")]
        [TestCase("__aa __", "__aa __", TestName = "Space before closing bold tag")]
        [TestCase("__a__aaa__a__", "<strong>a</strong>aaa<strong>a</strong>",
            TestName = "Bold tags inside word")]
        [TestCase("a__d a__d", "a__d a__d", TestName = "Bold terms in different words")]

        [TestCase("__asd _aa_ dfdf__", "<strong>asd <em>aa</em> dfdf</strong>",
            TestName = "Italic tag inside bold tag")]
        [TestCase("_a __a__ d_", "<em>a __a__ d</em>", TestName = "Bold tag inside italic tag")]
        [TestCase("_asd __aa__ dfdf", "_asd <strong>aa</strong> dfdf",
            TestName = "Unclosed italic term and bold tag")]
        [TestCase("_asd __aa_ dfdf__", "_asd __aa_ dfdf__", TestName = "Crossing italic and bold terms")]
        [TestCase("_asd__", "_asd__", TestName = "Unpaired terms")]

        [TestCase(@"\asd\n\t", @"\asd\n\t", TestName = "Escaping with regular characters")]
        [TestCase(@"\_a_", @"_a_", TestName = "Escaping italic term")]
        [TestCase(@"\_ai _sjj_ dfdf_", @"_ai <em>sjj</em> dfdf_",
            TestName = "Escaping italic term and italic tag")]
        [TestCase(@"\__a__", @"__a__", TestName = "Escaping bold term")]
        [TestCase(@"\\", @"\", TestName = "Escaping itself")]
        [TestCase(@"\#", @"#", TestName = "Escaping header term")]

        [TestCase("# ", "# ", TestName = "Only header tag")]
        [TestCase("#asfdf", "<h1>asfdf</h1>", TestName = "One word in header tag")]
        [TestCase("#asfdf asd", "<h1>asfdf asd</h1>", TestName = "Sentence in header tag")]
        [TestCase("aa #asfdf asd", "aa #asfdf asd", TestName = "Header term inside sentence")]
        [TestCase("#_asfdf_ asd", "<h1><em>asfdf</em> asd</h1>", TestName = "Header with italic tag")]
        [TestCase("#__aaaa__", "<h1><strong>aaaa</strong></h1>", TestName = "Header with bold tag")]
        [TestCase("#__as _asfdf_ as__", "<h1><strong>as <em>asfdf</em> as</strong></h1>",
            TestName = "Header with italic tag inside bold tag")]
        public void CheckHeaderOnly(string input, string asertResult)
        {
            var html = Md.Render(input);

            html.Should().Be(asertResult);
        }
    }
}  