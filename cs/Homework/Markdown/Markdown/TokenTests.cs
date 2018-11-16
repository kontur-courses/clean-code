using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class Token_Should
    {
        [TestCase("", TestName = "Empty string")]
        [TestCase(@"\_", TestName = "With escaping")]
        [TestCase("_", TestName = "Open tag without text part after")]
        [TestCase("a_a", TestName = "Close tag with text part after")]
        public void GetPossibleTokens_ReturnEmptyList(string rule)
        {
            var actual = TokenLogic.GetPossibleTokens(rule);

            actual.Should().BeEmpty();
        }

        [TestCase("a_", TestName = "Close tag in the end of the line")]
        [TestCase("a_.", TestName = "Close tag with punctuation after")]
        [TestCase("a_ ", TestName = "Close tag with space after")]
        [TestCase("_a", TestName = "Open tag in the start of the line")]
        [TestCase(" _a", TestName = "Open tag with space before")]
        [TestCase("._a", TestName = "Open tag with punctuation before")]
        public void GetPossibleTokens_ReturnToken(string rule)
        {
            var actual = TokenLogic.GetPossibleTokens(rule);

            actual.Should().NotBeEmpty();
        }

        [Test]
        public void GetValidTokens_ReturnEmptyList_NoClosePair()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 0, isOpen: true),
                new Token(TokenLogic.Tags[0], position: 2, isOpen: true)
            };

            var actually = rule.GetValidTokens();

            actually.Should().BeEmpty();
        }

        [Test]
        public void GetValidTokens_ReturnEmptyList_NoOpenPair()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 0, isOpen: false),
                new Token(TokenLogic.Tags[0], position: 2, isOpen: false)
            };

            var actually = rule.GetValidTokens();

            actually.Should().BeEmpty();
        }

        [Test]
        public void GetValidTokens_ReturnEmptyList_PairOfDifferentTags()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 0, isOpen: false),
                new Token(TokenLogic.Tags[1], position: 2, isOpen: false)
            };

            var actually = rule.GetValidTokens();

            actually.Should().BeEmpty();
        }

        [Test]
        public void GetValidTokens_ReturnEmptyList_NotInRightOrder()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 2, isOpen: false),
                new Token(TokenLogic.Tags[0], position: 0, isOpen: false)
            };

            var actually = rule.GetValidTokens();

            actually.Should().BeEmpty();
        }

        [Test]
        public void GetValidTokens_ReturnTokens_RightPair()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 0, isOpen: true),
                new Token(TokenLogic.Tags[0], position: 2, isOpen: false)
            };

            var actually = rule.GetValidTokens();

            actually.Should().HaveCount(2);
        }

        [Test]
        public void GetValidTokens_ReturnTokens_NestedRightPair()
        {
            var rule = new List<Token>
            {
                new Token(TokenLogic.Tags[0], position: 0, isOpen: true),
                new Token(TokenLogic.Tags[1], position: 2, isOpen: true),
                new Token(TokenLogic.Tags[1], position: 4, isOpen: false),
                new Token(TokenLogic.Tags[0], position: 6, isOpen: false)
            };

            var actually = rule.GetValidTokens();

            actually.Should().HaveCount(4);
        }
    }
}
