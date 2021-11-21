using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown
{
    [TestFixture]
    public class TokenConverterTests
    {
        private TokenConverter converter;

        [SetUp]
        public void SetUp()
        {
            converter = new TokenConverter();
        }

        [TestCase("_ab c_", 1)]
        [TestCase("_a_ _a_", 2)]
        [TestCase("_a_bcd", 1)]
        [TestCase("a_bc_d", 1)]
        [TestCase("abc_d_", 1)]
        public void FindTokens_ShouldFindAllItalicsTag_ItalicsMarkup(string source, int count)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => t.Tag)
                .Should().AllBeOfType<ItalicsTag>()
                .And.HaveCount(count);
        }

        [TestCase("_abc_", 0, 5)]
        [TestCase("ab _cd_", 3, 4)]
        [TestCase("_a_ cd", 0, 3)]
        public void FindTokens_ShouldSaveCorrectToken_ItalicsMarkup(string source, int start, int length)
        {
            var expectedToken = new Token(start, new ItalicsTag()) {Length = length};
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .First()
                .Should()
                .BeEquivalentTo(expectedToken);
        }

        [Test]
        public void FindTokens_ShouldSaveCorrectTokenParams_ItalicsMarkup()
        {
            var source = "_a_ _a_";
            var expected = new[]
            {
                (0, 3, true),
                (4, 3, true)
            };
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => (t.StartPosition, t.Length, t.Tag is ItalicsTag))
                .Should()
                .Contain(expected).And.HaveCount(2);
        }

        [TestCase("__")]
        [TestCase("abc_12_2")]
        [TestCase(" _abc_123 ")]
        [TestCase("a_b a_c")]
        [TestCase("_a b_c")]
        [TestCase("a_a b_")]
        [TestCase("_ a_")]
        [TestCase("_a _")]
        public void FindTokens_ShouldNotFindItalicsTag_IncorrectItalicsMarkup(string source)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Should()
                .BeEmpty();
        }

        [TestCase("_ab c_", "<em>ab c</em>")]
        [TestCase("_abc_ _abc_", "<em>abc</em> <em>abc</em>")]
        public void Build_ShouldReplaceMarkup_ItalicsMarkup(string source, string result)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .Build()
                .Should()
                .Be(result);
        }
    }
}