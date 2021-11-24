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
        public void GetTokens_ShouldReturnCorrectTokens_ItalicsMarkup(string source, int start, int length)
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
        public void GetTokens_ShouldReturnCorrectTokenParams_ItalicsMarkup()
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
        public void GetTokens_ShouldNotReturn_IncorrectItalicsMarkup(string source)
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
        public void GetTokens_ShouldReturnCorrectToken_BoldMarkup(string source, int start, int length)
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
        public void GetTokens_ShouldReturnCorrectTokenParams_BoldMarkup()
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
        [TestCase("___c_ _a___", 2)]
        public void FindTokens_ShouldFind_InnerItalicsMarkup(string source, int count)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is ItalicsTag)
                .Should()
                .HaveCount(count);
        }
        
        [Test]
        public void GetTokens_ShouldReturnCorrectItalicsToken_InnerItalicsMarkup()
        {
            var source = "___c_ a__";
            var expectedToken = new Token(2, new ItalicsTag()) {Length = 3};
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

        [Test]
        public void GetTokens_ShouldReturnCorrectToken_ExternalBoldMarkup()
        {
            var source = "___c_ a__";
            var expectedToken = new Token(0, new BoldTag()) {Length = 9};
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .First(t => t.Tag is BoldTag)
                .Should()
                .BeEquivalentTo(expectedToken);
        }
        
        [TestCase("_a __b__ c_")]
        [TestCase("_a__b__ __d__e_")]
        [TestCase("_a__b__ __c__ __d__e_")]
        public void FindTokens_ShouldNotFindBoldMarkup_InnerBoldMarkup(string source)
        {
            converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is BoldTag)
                .Should()
                .BeEmpty();
        }
        
        [Test]
        public void GetTokens_ShouldReturnCorrectToken_InnerBoldMarkup()
        {
            var source = "_ab __b__ c_";
            var expectedToken = new Token(0, new ItalicsTag()) {Length = source.Length};
            converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is ItalicsTag)
                .Should()
                .HaveCount(1)
                .And.OnlyContain(
                    t => t.StartPosition == expectedToken.StartPosition
                         && t.Length == expectedToken.Length
                );
        }
        
        [TestCase("_a __b_ c__")]
        [TestCase("__a_")]
        [TestCase("_a__")]
        public void FindTokens_ShouldNotFindMarkup_IntersectedBoldAndItalicsMarkup(string source)
        {
            converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Should()
                .BeEmpty();
        }

        [Test]
        public void FindTokens_ShouldFindItalicsAndBoldTokens()
        {
            var source = "_ab c_ __a__";
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Should()
                .Contain(t => t.StartPosition == 0 && t.Length == 6 && t.Tag is ItalicsTag)
                .And.Contain(t => t.StartPosition == 7 && t.Length == 5 && t.Tag is BoldTag);
        }

        [TestCase("_ab c_ __a__", "<em>ab c</em> <strong>a</strong>")]
        [TestCase("__a_b_c__", "<strong>a<em>b</em>c</strong>")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>")]
        public void Build_ShouldReplaceMarkup_ItalicsAndBoldMarkup(string source, string result)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .Build()
                .Should()
                .Be(result);
        }

        [TestCase("\\_a_")]
        [TestCase("\\__a__")]
        [TestCase("_a\\_")]
        [TestCase("__a\\__")]
        public void FindTokens_ShouldNotFindBoldAndItalicsMarkup_ShieldingMarkup(string source)
        {
            converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => t.Tag)
                .Should()
                .AllBeAssignableTo<ShieldingTag>();
        }
        
        [TestCase("\\\\_a b_")]
        [TestCase("\\\\__a b__")]
        [TestCase("_\\__")]
        [TestCase("__\\___")]
        public void FindTokens_ShouldFindMarkup_ShieldingMarkup(string source)
        {
            converter.SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Where(t => t.Tag is BoldTag || t.Tag is ItalicsTag)
                .Should()
                .HaveCount(1);
        }
        
        [TestCase("\\_", "_")]
        [TestCase("\\__", "__")]
        public void Build_ShouldReplaceShielding_ShieldingMarkup(string source, string result)
        {
            converter.SetMarkupString(source)
                .FindTokens()
                .Build()
                .Should()
                .Be(result);
        }

        [Test]
        public void Build_ShouldNotReplaceShielding_ShieldingMarkup()
        {
            var source = "a\\b\\c\\ \\";
            converter.SetMarkupString(source)
                .FindTokens()
                .Build()
                .Should()
                .Be(source);
        }

        [TestCase("# abc\n", 1)]
        [TestCase("# abc\n# \n", 2)]
        public void FindTokens_ShouldFindAll_HeadingMarkup(string source, int count)
        {
            converter
                .SetMarkupString(source)
                .FindTokens()
                .GetTokens()
                .Select(t => t.Tag)
                .Should().AllBeOfType<HeadingTag>()
                .And.HaveCount(count);
        }
    }
}