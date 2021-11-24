using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkdownConvertor;
using MarkdownConvertor.ITokens;
using NUnit.Framework;

namespace MarkdownConvertorTests
{
    [TestFixture]
    public class TokenizerTests
    {
        private static IEnumerable<TestCaseData> CorrectInputsTests()
        {
            yield return new TestCaseData("abc", new[] { 
                new TextToken("abc"),
                new TextToken("\n") 
            });
            yield return new TestCaseData("_abc", new IToken[]
            {
                new TagToken("_"),
                new TextToken("abc"),
                new TextToken("\n")
            }).SetName("Should found double tag");
            yield return new TestCaseData("__abc_", new IToken[]
            {
                new TagToken("__"),
                new TextToken("abc"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should found double tag");
            yield return new TestCaseData("# abc\n_dgv_", new IToken[]
            {
                new TagToken("# "),
                new TextToken("abc"),
                new TextToken("\n"),
                new TagToken("_"),
                new TextToken("dgv"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should found single tag");
            yield return new TestCaseData("as# sd", new IToken[]
            {
                new TextToken("as"),
                new TagToken("# "),
                new TextToken("sd"),
                new TextToken("\n")
            }).SetName("Should found double tag");
            yield return new TestCaseData(@"\__avd_", new IToken[]
            {
                new ScreenerToken(@"\"),
                new TagToken("_"),
                new TagToken("_"),
                new TextToken("avd"),
                new TagToken("_"),
                new TextToken("\n")
            }).SetName("Should found screeners");
            yield return new TestCaseData(@"\\\__ad", new IToken[]
            {
                new ScreenerToken(@"\"),
                new ScreenerToken(@"\"),
                new ScreenerToken(@"\"),
                new TagToken("_"),
                new TagToken("_"),
                new TextToken("ad"),
                new TextToken("\n")
            }).SetName("Should found screeners");
            yield return new TestCaseData(@"as\cd", new IToken[]
            {
                new TextToken("as"),
                new ScreenerToken(@"\"),
                new TextToken("cd"),
                new TextToken("\n")
            }).SetName("Should found screeners");
            yield return new TestCaseData("- a\n- b\n- c", new IToken[]
            {
                new TagToken("- "),
                new TextToken("a"),
                new TextToken("\n"),
                new TagToken("- "),
                new TextToken("b"),
                new TextToken("\n"),
                new TagToken("- "),
                new TextToken("c"),
                new TextToken("\n")
            });
        }

        [TestCaseSource(nameof(CorrectInputsTests))]
        public void GetTokens_ShouldReturnCorrectTokens_WhenInputIsCorrect(string text, IToken[] expectedTokens)
        {
            var tokenizer = new Tokenizer();

            var tokens = tokenizer.GetTokens(text).ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}