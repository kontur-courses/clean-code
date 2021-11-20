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
        private Dictionary<int, TokenInfo> dictionary = new();

        [SetUp]
        public void SetUp()
        {
            dictionary = new Dictionary<int, TokenInfo>
            {
                [1] = new(null, true, false, false, true),
                [4] = new(null, true, true, true, true),
                [6] = new(null, false, false, false, false),
                [8] = new(null, true, false, false, true),
                [10] = new(null, true, true, true, true),
                [13] = new(null, false, true, false, true),
                [16] = new(null, false, true, false, true),
                [18] = new(null, true, false, false, true),
            };
        }

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