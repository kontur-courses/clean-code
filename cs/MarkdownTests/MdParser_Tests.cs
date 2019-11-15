using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MdParser_Tests
    {
        [TestCase("first", 1)]
        [TestCase("f _s_", 2)]
        [TestCase("f _s", 2)]
        [TestCase("w _t _v_", 3)]
        [TestCase("f __s__ _a", 3)]
        [TestCase("_asd __d__ dfdf_", 1)]
        public void MdParser_GetTokens_ReturnRightTokensCount(string text, int tokensCount)
        {
            new MdParser(text).GetTokens().Count().Should().Be(tokensCount);
        }
        
        [Test]
        public void MdParser_GetTokens_RightTokensPositions()
        {
            var tokens = new MdParser("f __s__").GetTokens();
            var positions = tokens.Select(t => t.Position).ToList();
            var expectedPositions = new List<int>{0, 2};
            positions.Should().BeEquivalentTo(expectedPositions);
        }
    }
}