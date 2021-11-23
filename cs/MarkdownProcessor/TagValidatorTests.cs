using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor
{
    [TestFixture]
    public class TagValidatorTests
    {
        private static IEnumerable<TestCaseData> Tests()
        {
            yield return new TestCaseData("abc", new IToken[]
            {
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when no tags");
            yield return new TestCaseData("_abc", new IToken[]
            {
                new TextToken("_"),
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when non-paired tags");
            yield return new TestCaseData("__abc_", new IToken[]
            {
                new TextToken("__"),
                new TextToken("abc"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextTokens when non-paired tags");
            yield return new TestCaseData("# abc\n_dgv_", new IToken[]
            {
                new SingleTagToken("# "),
                new TextToken("abc"),
                new TextToken("\n"),
                new DoubleTagToken("_", true, 2),
                new TextToken("dgv"),
                new DoubleTagToken("_", false, 2),
                new TextToken("\n")
            }).SetName("Should be valid header and italic tags when tags are correct");
            yield return new TestCaseData("as# sd", new IToken[]
            {
                new TextToken("as"),
                new TextToken("# "),
                new TextToken("sd"),
                new TextToken("\n")
            }).SetName("Header should be TextToken when inside word");
            yield return new TestCaseData("_ab _", new IToken[]
            {
                new TextToken("_"),
                new TextToken("ab "),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespace before closing tag");
            yield return new TestCaseData("_ ab_", new IToken[]
            {
                new TextToken("_"),
                new TextToken(" ab"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespace after opening tag");
            yield return new TestCaseData("ab_cd fg_oi", new IToken[]
            {
                new TextToken("ab"),
                new TextToken("_"),
                new TextToken("cd fg"),
                new TextToken("_"),
                new TextToken("oi"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags inside different words");
            yield return new TestCaseData("_abc _ghd idk_", new IToken[]
            {
                new DoubleTagToken("_", true, 2),
                new TextToken("abc "),
                new TextToken("_"),
                new TextToken("ghd idk"),
                new DoubleTagToken("_", false, 2),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag is invalid");
            yield return new TestCaseData("aa ____ aa", new IToken[]
            {
                new TextToken("aa "),
                new TextToken("__"),
                new TextToken("__"),
                new TextToken(" aa"),
                new TextToken("\n")
            }).SetName("Should be TextToken when empty string between tags");
            yield return new TestCaseData("aa _ _ aa", new IToken[]
            {
                new TextToken("aa "),
                new TextToken("_"),
                new TextToken(" "),
                new TextToken("_"),
                new TextToken(" aa"),
                new TextToken("\n")
            }).SetName("Should be TextToken when whitespaces between tags");
            yield return new TestCaseData("acd_12_3 rf", new IToken[]
            {
                new TextToken("acd"),
                new TextToken("_"),
                new TextToken("12"),
                new TextToken("_"),
                new TextToken("3 rf"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags inside digits");
            yield return new TestCaseData("_ad__dc__jn_", new IToken[]
            {
                new DoubleTagToken("_", true, 2),
                new TextToken("ad"),
                new TextToken("__"),
                new TextToken("dc"),
                new TextToken("__"),
                new TextToken("jn"),
                new DoubleTagToken("_", false, 2),
                new TextToken("\n")
            }).SetName("Should be TextToken when strong tags inside italic");
            yield return new TestCaseData("__ad_dc_jn__", new IToken[]
            {
                new DoubleTagToken("__", true, 1),
                new TextToken("ad"),
                new DoubleTagToken("_", true, 2),
                new TextToken("dc"),
                new DoubleTagToken("_", false, 2),
                new TextToken("jn"),
                new DoubleTagToken("__", false, 1),
                new TextToken("\n")
            }).SetName("Should be TagToken when italic tags inside strong");
            yield return new TestCaseData("__ad_dc__jn_", new IToken[]
            {
                new TextToken("__"),
                new TextToken("ad"),
                new TextToken("_"),
                new TextToken("dc"),
                new TextToken("__"),
                new TextToken("jn"),
                new TextToken("_"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags intersects");
            yield return new TestCaseData("_ad__dc_jn__", new IToken[]
            {
                new TextToken("_"),
                new TextToken("ad"),
                new TextToken("__"),
                new TextToken("dc"),
                new TextToken("_"),
                new TextToken("jn"),
                new TextToken("__"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tags intersects");
            yield return new TestCaseData(@"\__avd_", new IToken[]
            {
                new TextToken("_"),
                new DoubleTagToken("_", true, 2),
                new TextToken("avd"),
                new DoubleTagToken("_", false, 2),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag is screened");
            yield return new TestCaseData(@"\\\__ad", new IToken[]
            {
                new TextToken(@"\"),
                new TextToken("_"),
                new TextToken("_"),
                new TextToken("ad"),
                new TextToken("\n")
            }).SetName("Should be TextToken when tag and screener are screened");
            yield return new TestCaseData(@"as\c\ d\", new IToken[]
            {
                new TextToken("as"),
                new TextToken(@"\"),
                new TextToken("c"),
                new TextToken(@"\"),
                new TextToken(" d"),
                new TextToken(@"\"),
                new TextToken("\n")
            }).SetName("Should be TextToken when screeners don't screen");
            yield return new TestCaseData("# __abc__ _dc_", new IToken[]
            {
                new SingleTagToken("# "),
                new DoubleTagToken("__", true, 1),
                new TextToken("abc"),
                new DoubleTagToken("__", false, 1),
                new TextToken(" "),
                new DoubleTagToken("_", true, 2),
                new TextToken("dc"),
                new DoubleTagToken("_", false, 2),
                new TextToken("\n")
            }).SetName("Should be TagToken when italic and strong inside header");
        }

        [TestCaseSource(nameof(Tests))]
        public void GetValidTokens_ShouldReturnCorrectTokens(string text, IToken[] expectedTokens)
        {
            var validator = new TagValidator(new Dictionary<string, int> { { "_", 2 }, { "__", 1 } },
                new HashSet<string> { "# ", "- " },
                new HashSet<string> { @"\" });

            var tokens = validator.GetValidTokens(text).ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}