using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using HtmlConvertor;
using HtmlConvertor.ITokens;
using NUnit.Framework;

namespace HtmlConvertorTests
{
    [TestFixture]
    public class MarkdownProcessorTests
    {
        private static IEnumerable<TestCaseData> Tests()
        {
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("abc"),
                    new TextToken("\n")
                }, "abc\n")
                .SetName("Should be TextTag when no tags in text");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("_"),
                    new TextToken("abc"),
                    new TextToken("\n")
                }, "_abc\n")
                .SetName("Should be TextTag when non-paired tags");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("__"),
                    new TextToken("abc"),
                    new TextToken("_"),
                    new TextToken("\n")
                }, "__abc_\n")
                .SetName("Should be TextTag when non-paired tags");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("# "),
                    new TextToken("abc"),
                    new TextToken("\n")
                }, "<h1>abc</h1>\n")
                .SetName("Should be valid header tags when tags are correct");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("_", true),
                    new TextToken("dgv"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "<em>dgv</em>\n")
                .SetName("Should be valid italic tags when tags are correct");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("__", true),
                    new TextToken("dgv"),
                    new TagToken("__"),
                    new TextToken("\n")
                }, "<strong>dgv</strong>\n")
                .SetName("Should be valid strong tags when tags are correct");
            yield return
                new TestCaseData(new IToken[]
                    {
                        new TextToken("as"),
                        new TextToken("# "),
                        new TextToken("sd"),
                        new TextToken("\n")
                    }, "as# sd\n")
                    .SetName("Should be TextTag when header tag inside word");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("_"),
                    new TextToken("ab "),
                    new TextToken("_"),
                    new TextToken("\n")
                }, "_ab _\n")
                .SetName("Should be TextTag when whitespaces before closing tag");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("_"),
                    new TextToken(" ab"),
                    new TextToken("_"),
                    new TextToken("\n")
                }, "_ ab_\n")
                .SetName("Should be TextTag when whitespaces after opening tag");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("ab"),
                    new TextToken("_"),
                    new TextToken("cd fg"),
                    new TextToken("_"),
                    new TextToken("oi"),
                    new TextToken("\n")
                }, "ab_cd fg_oi\n")
                .SetName("Should be TextTag when tags inside different words");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("_", true),
                    new TextToken("abc "),
                    new TextToken("_"),
                    new TextToken("ghd idk"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "<em>abc _ghd idk</em>\n")
                .SetName("Invalid tag should be text");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("aa "),
                    new TextToken("__"),
                    new TextToken("__"),
                    new TextToken(" aa"),
                    new TextToken("\n")
                }, "aa ____ aa\n")
                .SetName("Should be TextTag when empty string between tags");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("aa "),
                    new TextToken("_"),
                    new TextToken(" "),
                    new TextToken("_"),
                    new TextToken(" aa"),
                    new TextToken("\n")
                }, "aa _ _ aa\n")
                .SetName("Should be TextTag when whitespaces between tags");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("acd"),
                    new TextToken("_"),
                    new TextToken("12"),
                    new TextToken("_"),
                    new TextToken("3 rf"),
                    new TextToken("\n")
                }, "acd_12_3 rf\n")
                .SetName("Should be TextTag when tags inside digits");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("_", true),
                    new TextToken("ad"),
                    new TextToken("__"),
                    new TextToken("dc"),
                    new TextToken("__"),
                    new TextToken("jn"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "<em>ad__dc__jn</em>\n")
                .SetName("Strong inside italic should not work");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("__", true),
                    new TextToken("ad"),
                    new TagToken("_", true),
                    new TextToken("dc"),
                    new TagToken("_"),
                    new TextToken("jn"),
                    new TagToken("__"),
                    new TextToken("\n")
                }, "<strong>ad<em>dc</em>jn</strong>\n")
                .SetName("Italic inside strong should work");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("__"),
                    new TextToken("ad"),
                    new TextToken("_"),
                    new TextToken("dc"),
                    new TextToken("__"),
                    new TextToken("jn"),
                    new TextToken("_"),
                    new TextToken("\n")
                }, "__ad_dc__jn_\n")
                .SetName("Should be TextTag when tags intersects");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("_"),
                    new TextToken("ad"),
                    new TextToken("__"),
                    new TextToken("dc"),
                    new TextToken("_"),
                    new TextToken("jn"),
                    new TextToken("__"),
                    new TextToken("\n")
                }, "_ad__dc_jn__\n")
                .SetName("Should be TextTag when tags intersects");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("_"),
                    new TagToken("_", true),
                    new TextToken("avd"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "_<em>avd</em>\n")
                .SetName("Screened tag should be text");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken(@"\"),
                    new TextToken("_"),
                    new TextToken("_"),
                    new TextToken("ad"),
                    new TextToken("\n")
                }, @"\__ad" + '\n')
                .SetName("Screened screener should be text");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("as"),
                    new TextToken(@"\"),
                    new TextToken("c"),
                    new TextToken(@"\"),
                    new TextToken(" d"),
                    new TextToken(@"\"),
                    new TextToken("\n")
                }, @"as\c\ d\" + '\n')
                .SetName("Screeners should be text when do not screen");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("# "),
                    new TagToken("__", true),
                    new TextToken("abc"),
                    new TagToken("__"),
                    new TextToken(" "),
                    new TagToken("_", true),
                    new TextToken("dc"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "<h1><strong>abc</strong> <em>dc</em></h1>\n")
                .SetName("Tags should work inside header");
            yield return new TestCaseData(new IToken[]
                {
                    new TagToken("- "),
                    new TextToken("ab"),
                    new TextToken("\n"),
                    new TagToken("- "),
                    new TextToken("cd"),
                    new TextToken("\n"),
                    new TagToken("- "),
                    new TextToken("de"),
                    new TextToken("\n")
                }, "<ul>\n<li>ab</li>\n<li>cd</li>\n<li>de</li>\n</ul>\n")
                .SetName("Simple unordered list should be marked by tags");
            yield return new TestCaseData(new IToken[]
                {
                    new TextToken("abc"),
                    new TextToken("\n"),
                    new TagToken("- "),
                    new TextToken("a"),
                    new TextToken("\n"),
                    new TagToken("- "),
                    new TextToken("b"),
                    new TextToken("\n"),
                    new TagToken("_", true),
                    new TextToken("ii"),
                    new TagToken("_"),
                    new TextToken("\n")
                }, "abc\n<ul>\n<li>a</li>\n<li>b</li>\n</ul>\n<em>ii</em>\n")
                .SetName("Unordered list inside text should be marked by tags");
        }

        [TestCaseSource(nameof(Tests))]
        public void Render_ShouldReturnCorrectTokens(IToken[] inputTokens, string result)
        {
            var markdownProcessor = new MarkdownProcessor();

            markdownProcessor.Render(inputTokens.ToList()).Should().BeEquivalentTo(result);
        }
    }
}