using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_Should
    {
        [Test]
        public void RenderOriginalText_WhenHasNoTags()
        {
            Md.Render("asd").Should().Be("asd");
        }

        [TestCase("# asd", ExpectedResult = "<h1>asd</h1>", TestName = "when tag in first line")]
        [TestCase("# asd ", ExpectedResult = "<h1>asd </h1>", TestName = "when space in string ending")]
        [TestCase("asd\r\n# asd", ExpectedResult = "asd\r\n<h1>asd</h1>", TestName = @"when tag after '\r\n'")]
        [TestCase("# asd    ", ExpectedResult = "<h1>asd    </h1>",
            TestName = "when not single space in string ending")]
        public string RenderHeaderTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase(" # asd", ExpectedResult = " # asd", TestName = "when tag not in line beginning")]
        [TestCase(@"\# asd", ExpectedResult = @"# asd", TestName = "when tag escaped")]
        [TestCase("#asd", ExpectedResult = "#asd", TestName = "when # has no space after")]
        public string NotRenderHeaderTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("# _asd_", ExpectedResult = "<h1><em>asd</em></h1>", TestName = "when paragraph is header")]
        [TestCase("* _asd_", ExpectedResult = "<ul>\r\n<li><em>asd</em></li>\r\n</ul>",
            TestName = "when paragraph is list item")]
        [TestCase("_asd_ ", ExpectedResult = "<em>asd</em> ", TestName = "when space in string ending")]
        [TestCase("_asd_    ", ExpectedResult = "<em>asd</em>    ",
            TestName = "when not single space in string ending")]
        [TestCase("_as_d", ExpectedResult = "<em>as</em>d", TestName = "in word beginning")]
        [TestCase("_asd a_sd asd_", ExpectedResult = "<em>asd a_sd asd</em>",
            TestName = "when first tag can't pair with second tag, but can with third tag")]
        [TestCase("a_sd_", ExpectedResult = "a<em>sd</em>", TestName = "in word ending")]
        [TestCase("asd_asd_asd", ExpectedResult = "asd<em>asd</em>asd", TestName = "in word middle")]
        [TestCase("_asd_", ExpectedResult = "<em>asd</em>", TestName = "1 time")]
        [TestCase("_asd_ a1s", ExpectedResult = "<em>asd</em> a1s", TestName = "when other word with digits")]
        [TestCase("_as1d_", ExpectedResult = "<em>as1d</em>", TestName = "when tags on edges of word with digits")]
        [TestCase("_asd_ _asd_", ExpectedResult = "<em>asd</em> <em>asd</em>", TestName = "2 times")]
        [TestCase("_asd__asd_", ExpectedResult = "<em>asd__asd</em>", TestName = "when nested non-pair bold tag")]
        [TestCase("_asd\r\n_asd_", ExpectedResult = "_asd\r\n<em>asd</em>",
            TestName = "when tag in first paragraph has no pair")]
        [TestCase("as_d _asd_", ExpectedResult = "as_d <em>asd</em>",
            TestName = "when tag in first word has no pair")]
        [TestCase("__a_asd_d__", ExpectedResult = "<strong>a<em>asd</em>d</strong>",
            TestName = "when nesting in bold tag")]
        [TestCase(@"___asd_\___", ExpectedResult = "<strong><em>asd</em>_</strong>",
            TestName = "when nesting in bold tag and close tag in ending of word but before escape tag and bold tag")]
        [TestCase(@"_1asd_\_", ExpectedResult = @"<em>1asd</em>_",
            TestName = "when close tag in ending of word but before escape tag")]
        [TestCase(@"\__1asd_", ExpectedResult = @"_<em>1asd</em>",
            TestName = "when open tag in beginning of word but after escape tag")]
        [TestCase(@"\_\__asd_\_\_", ExpectedResult = @"__<em>asd</em>__",
            TestName = "when tags on edges in word but nested in escape tags")]
        public string RenderItalicTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("a1sd_s_as2d", ExpectedResult = "a1sd_s_as2d", TestName = "when word with digits")]
        [TestCase("__asd a__sd asd__", ExpectedResult = "<strong>asd a__sd asd</strong>",
            TestName = "when first tag can't pair with second tag, but can with third tag")]
        [TestCase("_asd", ExpectedResult = "_asd", TestName = "when tag has no pair")]
        [TestCase("as_d a_sd", ExpectedResult = "as_d a_sd", TestName = "when tags in middle of different words")]
        [TestCase("_asd\r\nasd_", ExpectedResult = "_asd\r\nasd_", TestName = "when tags in different paragraphs")]
        [TestCase("_ asd_", ExpectedResult = "_ asd_", TestName = "when first tag can't be open")]
        [TestCase("_asd _", ExpectedResult = "_asd _", TestName = "when second tag can't be close")]
        [TestCase(@"_1asd_\a", ExpectedResult = @"_1asd_\a",
            TestName = "when possible close tag in ending of word but before escape tag which is not escaping tag")]
        [TestCase(@"\a_1asd_", ExpectedResult = @"\a_1asd_",
            TestName = "when possible open tag in beginning of word but after escape tag which is not escaping tag")]
        public string NotRenderItalicTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("# __asd__", ExpectedResult = "<h1><strong>asd</strong></h1>",
            TestName = "when paragraph is header")]
        [TestCase("* __asd__", ExpectedResult = "<ul>\r\n<li><strong>asd</strong></li>\r\n</ul>",
            TestName = "when paragraph is list item")]
        [TestCase("__asd__ ", ExpectedResult = "<strong>asd</strong> ", TestName = "when space in string ending")]
        [TestCase("__asd__    ", ExpectedResult = "<strong>asd</strong>    ",
            TestName = "when not single space in string ending")]
        [TestCase("__as__d", ExpectedResult = "<strong>as</strong>d", TestName = "in word beginning")]
        [TestCase("a__sd__", ExpectedResult = "a<strong>sd</strong>", TestName = "in word ending")]
        [TestCase("asd__asd__asd", ExpectedResult = "asd<strong>asd</strong>asd", TestName = "in word middle")]
        [TestCase("__asd__", ExpectedResult = "<strong>asd</strong>", TestName = "1 time")]
        [TestCase("__as1d__", ExpectedResult = "<strong>as1d</strong>",
            TestName = "when tags on edges of word with digits")]
        [TestCase("__asd__ a1s", ExpectedResult = "<strong>asd</strong> a1s", TestName = "when other word with digits")]
        [TestCase("__asd__ __asd__", ExpectedResult = "<strong>asd</strong> <strong>asd</strong>",
            TestName = "2 times with space between pair")]
        [TestCase("__asd____asd__", ExpectedResult = "<strong>asd</strong><strong>asd</strong>",
            TestName = "2 times without space between pair")]
        [TestCase("__asd\r\n__asd__", ExpectedResult = "__asd\r\n<strong>asd</strong>",
            TestName = "when tag in first paragraph has no pair")]
        [TestCase("as__d __asd__", ExpectedResult = "as__d <strong>asd</strong>",
            TestName = "when tag in first word has no pair")]
        [TestCase(@"__1asd__\_", ExpectedResult = @"<strong>1asd</strong>_",
            TestName = "when close tag in ending of word but before escape tag")]
        [TestCase(@"\___1asd__", ExpectedResult = @"_<strong>1asd</strong>",
            TestName = "when open tag in beginning of word but after escape tag")]
        [TestCase(@"\_\___asd__\_\_", ExpectedResult = @"__<strong>asd</strong>__",
            TestName = "when tags on edges in word but nested in escape tags")]
        public string RenderBoldTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("a1sd__s__as2d", ExpectedResult = "a1sd__s__as2d", TestName = "when word with digits")]
        [TestCase("__asd", ExpectedResult = "__asd", TestName = "when tag has no pair")]
        [TestCase("as__d a__sd", ExpectedResult = "as__d a__sd", TestName = "when tags in middle of different words")]
        [TestCase("__asd\r\nasd__", ExpectedResult = "__asd\r\nasd__", TestName = "when tags in different paragraphs")]
        [TestCase("asd____asd", ExpectedResult = "asd____asd", TestName = "when word between tags is empty")]
        [TestCase("_a__asd__d_", ExpectedResult = "<em>a__asd__d</em>", TestName = "when nesting in italic tag")]
        [TestCase("__ asd__", ExpectedResult = "__ asd__", TestName = "when first tag can't be open")]
        [TestCase("__asd __", ExpectedResult = "__asd __", TestName = "when second tag can't be close")]
        [TestCase(@"__1asd__\a", ExpectedResult = @"__1asd__\a",
            TestName = "when possible close tag in ending of word but before escape tag which is not escaping tag")]
        [TestCase(@"\a__1asd__", ExpectedResult = @"\a__1asd__",
            TestName = "when possible open tag in beginning of word but after escape tag which is not escaping tag")]
        public string NotRenderBoldTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("__as_d __a_sd", ExpectedResult = "__as_d __a_sd", TestName = "when first tag is bold")]
        [TestCase("_as__d _a__sd", ExpectedResult = "_as__d _a__sd", TestName = "when first tag is italic")]
        public string NotRenderTags_WhenTagsIntersecting(string input)
        {
            return Md.Render(input);
        }

        [TestCase(@"\ asd", ExpectedResult = @"\ asd", TestName = "when escape symbol before space")]
        [TestCase(@"\asd", ExpectedResult = @"\asd", TestName = "when escape symbol before letter")]
        [TestCase(@"a5s\_d", ExpectedResult = @"a5s\_d",
            TestName = "when escape symbol before tag in word with digits")]
        [TestCase(@"asd \# asd", ExpectedResult = @"asd \# asd",
            TestName = "when escape symbol before header tag which not in line beginning")]
        [TestCase(@"asd\", ExpectedResult = @"asd\", TestName = "when escape symbol in end of string")]
        public string RenderEscapeSymbol(string input)
        {
            return Md.Render(input);
        }

        [TestCase(@"\# asd", ExpectedResult = @"# asd", TestName = "when escape symbol before header tag")]
        [TestCase(@"\* asd", ExpectedResult = @"* asd", TestName = "when escape symbol before list item tag")]
        [TestCase(@"as\_d_", ExpectedResult = @"as_d_", TestName = "when escape symbol before italic tag")]
        [TestCase(@"as\__d__", ExpectedResult = @"as__d__", TestName = "when escape symbol before bold tag")]
        [TestCase(@"\_a5sd", ExpectedResult = @"_a5sd",
            TestName = "when escape symbol before tag in beginning of word with digits")]
        [TestCase(@"_\_a5sd_", ExpectedResult = @"<em>_a5sd</em>",
            TestName = "when escape symbol before tag in beginning of word with digits, but after tag")]
        [TestCase(@"_a5sd\__", ExpectedResult = @"<em>a5sd_</em>",
            TestName = "when escape symbol before tag in ending of word with digits, but before tag")]
        [TestCase(@"a5sd\_", ExpectedResult = @"a5sd_",
            TestName = "when escape symbol before italic tag in ending of word with digits")]
        [TestCase(@"as\\d", ExpectedResult = @"as\d", TestName = "when escape symbol before escape symbol")]
        [TestCase(@"as\\\\d", ExpectedResult = @"as\\d",
            TestName = "when escape symbol before escape symbol 2 times in a row")]
        public string NotRenderEscapeSymbol(string input)
        {
            return Md.Render(input);
        }

        [TestCase("* asd", ExpectedResult = "<ul>\r\n<li>asd</li>\r\n</ul>", TestName = "when has only one list item")]
        [TestCase("* asd\r\n* asd", ExpectedResult = "<ul>\r\n<li>asd</li>\r\n<li>asd</li>\r\n</ul>",
            TestName = "when has more than one list item")]
        [TestCase("* asd\r\nasd\r\n* asd",
            ExpectedResult = "<ul>\r\n<li>asd</li>\r\n</ul>\r\nasd\r\n<ul>\r\n<li>asd</li>\r\n</ul>",
            TestName = "when several unordered lists")]
        [TestCase("* asd\r\n\\* asd\r\n* asd",
            ExpectedResult = "<ul>\r\n<li>asd</li>\r\n</ul>\r\n* asd\r\n<ul>\r\n<li>asd</li>\r\n</ul>",
            TestName = "when 3 list item tags in a row, but tag in middle escaped")]
        public string RenderUnorderedList(string input)
        {
            return Md.Render(input);
        }

        [TestCase(" * asd", ExpectedResult = " * asd", TestName = "when tag not in line beginning")]
        [TestCase("*asd", ExpectedResult = "*asd", TestName = "when * has no space after")]
        [TestCase(@"\* asd", ExpectedResult = @"* asd", TestName = "when tag escaped")]
        public string NotRenderUnorderedList(string input)
        {
            return Md.Render(input);
        }
    }
}