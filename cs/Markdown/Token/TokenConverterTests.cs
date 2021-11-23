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
        public void FindTokens_ShouldFindAll_ItalicsMarkup(string source, int count)
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
        public void FindTokens_ShouldSaveCorrectTokens_ItalicsMarkup(string source, int start, int length)
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
        public void FindTokens_ShouldNotFind_IncorrectItalicsMarkup(string source)
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
        
        [TestCase("__ab c__", 1)]
        [TestCase("__a__ __a__", 2)]
        [TestCase("__a__bcd", 1)]
        [TestCase("a__bc__d", 1)]
        [TestCase("abc__d__", 1)]
        public void FindTokens_ShouldFindAll_BoldMarkup(string source, int count)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => t.Tag)
                .Should().AllBeOfType<BoldTag>()
                .And.HaveCount(count);
        }
        
        [TestCase("__abc__", 0, 7)]
        [TestCase("ab __cd__", 3, 6)]
        [TestCase("__a__ cd", 0, 5)]
        public void FindTokens_ShouldSaveCorrectTokens_BoldMarkup(string source, int start, int length)
        {
            var expectedToken = new Token(start, new BoldTag()) {Length = length};
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .First()
                .Should()
                .BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void FindTokens_ShouldSaveCorrectTokenParams_BoldMarkup()
        {
            var source = "__a__ __a__";
            var expected = new[]
            {
                (0, 5, true),
                (6, 5, true)
            };
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => (t.StartPosition, t.Length, t.Tag is BoldTag))
                .Should()
                .Contain(expected).And.HaveCount(2);
        }
        
        [TestCase("____")]
        [TestCase("abc__12__2")]
        [TestCase(" __abc__123 ")]
        [TestCase("a__b a__c")]
        [TestCase("__a b__c")]
        [TestCase("a__a b__")]
        [TestCase("__ a__")]
        [TestCase("__a __")]
        public void FindTokens_ShouldNotFindBoldMarkup_IncorrectBoldMarkup(string source)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Should()
                .BeEmpty();
        }
        
        [TestCase("__ab c__", "<strong>ab c</strong>")]
        [TestCase("__abc__ __abc__", "<strong>abc</strong> <strong>abc</strong>")]
        public void Build_ShouldReplaceMarkup_BoldMarkup(string source, string result)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .Build()
                .Should()
                .Be(result);
        }
        
        [TestCase("__a b_")]
        [TestCase("_a b__")]
        [TestCase("__a")]
        [TestCase("a__")]
        [TestCase("_a")]
        [TestCase("a_")]
        public void FindTokens_ShouldNotFindMarkup_IncorrectMarkup(string source)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Should()
                .BeEmpty();
        }
        
        [TestCase("__ab_c_de__", 1)]
        [TestCase("___c_ a__", 1)]
        [TestCase("__c _a___", 1)]
        [TestCase("__ _c_ _a_ __", 2)]
        public void FindTokens_ShouldFind_InnerItalicMarkup(string source, int count)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is ItalicsTag)
                .Should()
                .HaveCount(count);
        }
        
        [TestCase("___c_ a__", 2, 3)]
        [TestCase("__c _a___", 4, 3)]
        public void FindTokens_ShouldSaveCorrectTokens_InnerItalicMarkup(string source, int start, int length)
        {
            var expectedToken = new Token(start, new ItalicsTag()) {Length = length};
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .First(t => t.Tag is ItalicsTag)
                .Should()
                .BeEquivalentTo(expectedToken);
        }
        
        [TestCase("__ab_c_de__")]
        [TestCase("___c_ a__")]
        [TestCase("__c _a___")]
        [TestCase("___c_ _a___")]
        public void FindTokens_ShouldFind_ExternalBoldMarkup(string source)
        {
             converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is BoldTag)
                .Should()
                .HaveCount(1);
        }
        
        [TestCase("___c_ a__")]
        [TestCase("__c _a___")]
        public void FindTokens_ShouldSaveCorrectTokens_ExternalBoldMarkup(string source)
        {
            var expectedToken = new Token(0, new BoldTag()) {Length = 9};
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .First(t => t.Tag is BoldTag)
                .Should()
                .BeEquivalentTo(expectedToken);
        }
    }
}