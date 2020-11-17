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
        [TestCase("asd\r\n# asd", ExpectedResult = "asd\r\n<h1>asd</h1>", TestName = @"when tag after '\r\n'")]
        public string RenderHeaderTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase(" # asd", ExpectedResult = " # asd", TestName = "when tag not in line beginning")]
        [TestCase("#asd", ExpectedResult = "#asd", TestName = "when # has no space after")]
        public string NotRenderHeaderTag(string input)
        {
            return Md.Render(input);
        }


        [TestCase("_as_d", ExpectedResult = "<em>as</em>d", TestName = "in word beginning")]
        [TestCase("_asd a_sd asd_", ExpectedResult = "<em>asd a_sd asd</em>",
            TestName = "when first tag can't pair with second tag, but can with third tag")]
        [TestCase("a_sd_", ExpectedResult = "a<em>sd</em>", TestName = "in word ending")]
        [TestCase("asd_asd_asd", ExpectedResult = "asd<em>asd</em>asd", TestName = "in word middle")]
        [TestCase("_asd_", ExpectedResult = "<em>asd</em>", TestName = "1 time")]
        [TestCase("_asd_ a1s", ExpectedResult = "<em>asd</em> a1s", TestName = "when other word with digits")]
        [TestCase("_asd_ _asd_", ExpectedResult = "<em>asd</em> <em>asd</em>", TestName = "2 times")]
        [TestCase("_asd__asd_", ExpectedResult = "<em>asd__asd</em>", TestName = "when nested non-pair bold tag")]
        [TestCase("_asd\r\n_asd_", ExpectedResult = "_asd\r\n<em>asd</em>",
            TestName = "when tag in first paragraph has no pair")]
        [TestCase("as_d _asd_", ExpectedResult = "as_d <em>asd</em>",
            TestName = "when tag in first word has no pair")]
        [TestCase("__a_asd_d__", ExpectedResult = "<strong>a<em>asd</em>d</strong>",
            TestName = "when nesting in bold tag")]
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
        public string NotRenderItalicTag(string input)
        {
            return Md.Render(input);
        }

        [TestCase("__as__d", ExpectedResult = "<strong>as</strong>d", TestName = "in word beginning")]
        [TestCase("a__sd__", ExpectedResult = "a<strong>sd</strong>", TestName = "in word ending")]
        [TestCase("asd__asd__asd", ExpectedResult = "asd<strong>asd</strong>asd", TestName = "in word middle")]
        [TestCase("__asd__", ExpectedResult = "<strong>asd</strong>", TestName = "1 time")]
        [TestCase("__asd__ a1s", ExpectedResult = "<strong>asd</strong> a1s", TestName = "when other word with digits")]
        [TestCase("__asd__ __asd__", ExpectedResult = "<strong>asd</strong> <strong>asd</strong>",
            TestName = "2 times with space between pair")]
        [TestCase("__asd____asd__", ExpectedResult = "<strong>asd</strong><strong>asd</strong>",
            TestName = "2 times without space between pair")]
        [TestCase("__asd\r\n__asd__", ExpectedResult = "__asd\r\n<strong>asd</strong>",
            TestName = "when tag in first paragraph has no pair")]
        [TestCase("as__d __asd__", ExpectedResult = "as__d <strong>asd</strong>",
            TestName = "when tag in first word has no pair")]
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
        [TestCase(@"asd \# asd", ExpectedResult = @"asd \# asd",
            TestName = "when escape symbol before header tag which not in line beginning")]
        [TestCase(@"asd\", ExpectedResult = @"asd\", TestName = "when escape symbol in end of string")]
        public string RenderEscapeSymbol(string input)
        {
            return Md.Render(input);
        }

        [TestCase(@"\# asd", ExpectedResult = @"# asd", TestName = "when escape symbol before header tag")]
        [TestCase(@"as\_d_", ExpectedResult = @"as_d_", TestName = "when escape symbol before italic tag")]
        [TestCase(@"as\__d__", ExpectedResult = @"as__d__", TestName = "when escape symbol before bold tag")]
        [TestCase(@"as\\d", ExpectedResult = @"as\d", TestName = "when escape symbol before escape symbol")]
        [TestCase(@"as\\\\d", ExpectedResult = @"as\\d",
            TestName = "when escape symbol before escape symbol 2 times in a row")]
        public string NotRenderEscapeSymbol(string input)
        {
            return Md.Render(input);
        }
    }
}