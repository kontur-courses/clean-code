using System.Linq;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Token
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
        
        
        [TestCase("_abc_", 1)]
        [TestCase("_abc_ _abc_", 2)]
        [TestCase(" _a_ _bc_ ", 2)]
        public void FindTokens_ShouldFindAllTokensWithItalicsTag_ItalicsMarkup(string source, int count)
        {
            converter.FindTokens(source)
                .GetTokens()
                .Select(t => t.Tag)
                .Should().AllBeOfType<ItalicsTag>()
                .And.HaveCount(count);
        }
    }
}