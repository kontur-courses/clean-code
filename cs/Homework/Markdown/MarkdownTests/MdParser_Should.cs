using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class MdParser_Should
    {
        [TestCase("", TestName = "Empty string")]
        [TestCase(@"\_a_", TestName = "With escaping")]
        [TestCase("_a", TestName = "No close tag")]
        [TestCase("a_a", TestName = "Close tag with text part after")]
        [TestCase("a _a ", TestName = "No closure")]
        public void GetAllTokens_ReturnEmptyList(string rule)
        {
            var actual = MdParser.GetAllTokens(rule);

            actual.Should().BeEmpty();
        }

        [TestCase("_a_", TestName = "Valid tag pair")]
        [TestCase("_a_.", TestName = "Valid tag pair with punctuation after")]
        [TestCase("_a_ ", TestName = "Valid tag pair with space after")]
        [TestCase(" _a_ ", TestName = "Valid tag pair with space before")]
        [TestCase("._a_", TestName = "Valid tag pair with punctuation before")]
        public void GetAllTokens_ReturnToken(string rule)
        {
            var actual = MdParser.GetAllTokens(rule);

            actual.Should().NotBeEmpty();
        }

        [Test]
        public void GetNotIntersectingEndsTokens_ReturnLeftToken_EndsIntersecting()
        {
            var tokens = new List<Token>
            {
                new Token(MdParser.Tags[0], startPosition: 0, endPosition: 15),
                new Token(MdParser.Tags[0], startPosition: 5, endPosition: 15)
            };

            var actual = MdParser.GetNotIntersectingEndsTokens(tokens);

            actual.Should().HaveCount(1);
            actual.First().Should().BeEquivalentTo(tokens.First());
        }

        [Test]
        public void GetNotIntersectingEndsTokens_ReturnBoth_OneInAnother()
        {
            var tokens = new List<Token>
            {
                new Token(MdParser.Tags[0], startPosition: 0, endPosition: 15),
                new Token(MdParser.Tags[0], startPosition: 5, endPosition: 10)
            };

            var actual = MdParser.GetNotIntersectingEndsTokens(tokens);

            actual.Should().HaveCount(2);
        }

        [Test]
        public void GetNotIntersectingEndsTokens_ReturnBoth_IntersectingButNotEnds()
        {
            var tokens = new List<Token>
            {
                new Token(MdParser.Tags[0], startPosition: 0, endPosition: 15),
                new Token(MdParser.Tags[0], startPosition: 5, endPosition: 20)
            };

            var actual = MdParser.GetNotIntersectingEndsTokens(tokens);

            actual.Should().HaveCount(2);
        }

        [Test]
        public void RemoveNotWorkingNestedTokens_ReturnMainOnly_NestedNotWorking()
        {
            var tokens = new List<Token>
            {
                new Token(new Tag("_", "em"), startPosition: 0, endPosition: 20),
                new Token(new Tag("__", "strong"), startPosition: 5, endPosition: 15)
            };

            var actual = MdParser.RemoveNotWorkingNestedTokens(tokens);

            actual.Should().HaveCount(1);
            actual.First().Should().BeEquivalentTo(tokens.First());
        }
    }
}
