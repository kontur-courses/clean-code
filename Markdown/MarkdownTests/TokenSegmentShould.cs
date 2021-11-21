using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    public class TokenSegmentShould
    {
        private List<TokenInfo> dictionary = new();

        [SetUp]
        public void SetUp()
        {
            dictionary = new List<TokenInfo>
            {  
                new(1, new Token("-"), true, false, false, true),
                new(4, new Token("-"), true, true, true, true),
                new(6, new Token("-"), false, false, false, false),
                new(8, new Token("-"), true, false, false, true),
                new(10, new Token("-"), true, true, true, true),
                new(13, new Token("-"), false, true, false, true),
                new(16, new Token("-"), false, true, false, true),
                new(18, new Token("-"), true, false, false, true)
            };
        }

        [Test]
        public void GetTokensSegments_startIndexesTest()
        {
            var segments = TokenSegment.GetTokensSegments(dictionary);

            segments.First().Length.Should().Be(4);
            segments.Last().Length.Should().Be(8);
        }
        
        [Test]
        public void GetTokensSegments_CountTest()
        {
            TokenSegment.GetTokensSegments(dictionary).Count().Should().Be(2);
        }
    }
}