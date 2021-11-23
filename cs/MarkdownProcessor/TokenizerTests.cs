using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessor
{
    [TestFixture]
    public class TokenizerTests
    {
        private static IEnumerable<TestCaseData> CorrectInputsTests()
        {
            yield return new TestCaseData("abc", new[] { new TextToken("abc") });
            yield return new TestCaseData("_abc", new IToken[]
            {
                new DoubleTagToken("_", false, 2),
                new TextToken("abc")
            }).SetName("Should found double tag");
            yield return new TestCaseData("__abc_", new IToken[]
            {
                new DoubleTagToken("__", false, 1),
                new TextToken("abc"),
                new DoubleTagToken("_", false, 2)
            }).SetName("Should found double tag");
            yield return new TestCaseData("# abc\n_dgv_", new IToken[]
            {
                new SingleTagToken("# "),
                new TextToken("abc\n"),
                new DoubleTagToken("_", false, 2),
                new TextToken("dgv"),
                new DoubleTagToken("_", false, 2)
            }).SetName("Should found single tag");
            yield return new TestCaseData("as# sd", new IToken[]
            {
                new TextToken("as"),
                new SingleTagToken("# "),
                new TextToken("sd")
            }).SetName("Should found double tag");
            yield return new TestCaseData(@"\__avd_", new IToken[]
            {
                new ScreenerToken(@"\"),
                new DoubleTagToken("_", false, 2),
                new DoubleTagToken("_", false, 2),
                new TextToken("avd"),
                new DoubleTagToken("_", false, 2)
            }).SetName("Should found screeners");
            yield return new TestCaseData(@"\\\__ad", new IToken[]
            {
                new ScreenerToken(@"\"),
                new ScreenerToken(@"\"),
                new ScreenerToken(@"\"),
                new DoubleTagToken("_", false, 2),
                new DoubleTagToken("_", false, 2),
                new TextToken("ad")
            }).SetName("Should found screeners");
            yield return new TestCaseData(@"as\cd", new IToken[]
            {
                new TextToken("as"),
                new ScreenerToken(@"\"),
                new TextToken("cd")
            }).SetName("Should found screeners");
        }

        [TestCaseSource(nameof(CorrectInputsTests))]
        public void GetTokens_ShouldReturnCorrectTokens_WhenInputIsCorrect(string text, IToken[] expectedTokens)
        {
            var tokenizer = new Tokenizer(new HashSet<string> { "# ", "- " },
                new Dictionary<string, int> { { "_", 2 }, { "__", 1 } },
                new HashSet<string> { @"\" });

            var tokens = tokenizer.GetTokens(text).ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}