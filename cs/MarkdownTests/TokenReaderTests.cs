using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Models;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenReaderTests
    {
        [Test]
        public void FindAll_ReturnEmptyCollection_IfTokensNotGiven()
        {
            new TokenReader("_text_", Enumerable.Empty<IToken>())
                .FindAll()
                .Should().BeEmpty();
        }

        [Test]
        public void FindAll_ReturnEmptyCollection_IfTextHaveNoTokens()
        {
            new TokenReader("qwerty", MarkdownTokensFactory.GetTokens())
                .FindAll()
                .Should().BeEmpty();
        }

        [TestCase("", TestName = "Empty")]
        [TestCase(null, TestName = "null")]
        public void Constructor_ThrowsException_IfTextIs(string text)
        {
            Assert.Throws<ArgumentException>(() =>
                new TokenReader(text, Enumerable.Empty<IToken>()));
        }

        [TestCaseSource(nameof(FindAllReturnSingleMatchCases))]
        public void FindAll_ReturnSingleMatch_IfTextHas(TokenMatch expectedMatch, string text)
        {
            var matches = new[] {expectedMatch};
            var actual = new TokenReader(text, new[] {expectedMatch.Token}).FindAll();
            actual.Should().BeEquivalentTo(matches);
        }

        public static IEnumerable<TestCaseData> FindAllReturnSingleMatchCases()
        {
            var token = MarkdownTokensFactory.Italic();

            yield return new TestCaseData(new TokenMatch {Start = 0, Length = 3, Token = token}, "_a_")
                {TestName = "Single token"};

            yield return new TestCaseData(new TokenMatch {Start = 3, Length = 3, Token = token}, "qwe_a_ewq")
                {TestName = "Single token in context"};

            yield return new TestCaseData(new TokenMatch {Start = 1, Length = 5, Token = token}, "q_abc_q")
                {TestName = "Token with many symbols inside"};
        }

        [Test]
        public void FindAll_ReturnsAllMatches_IfTextHasOneTypeTokens()
        {
            var token = MarkdownTokensFactory.Italic();
            var matches = new[]
            {
                new TokenMatch {Start = 1, Length = 5, Token = token},
                new TokenMatch {Start = 7, Length = 5, Token = token}
            };

            var actual = new TokenReader("U_one_U_two_U", new[] {token}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }
    }
}