using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TokenReaderTests
    {
        private IToken italic;
        private IToken bold;

        [SetUp]
        public void SetUp()
        {
            italic = MarkdownTokensFactory.Italic();
            bold = MarkdownTokensFactory.Bold();
        }

        [Test]
        public void FindAll_ReturnEmptyCollection_TokensNotGiven()
        {
            new TokenReader("_text_", Enumerable.Empty<IToken>())
                .FindAll()
                .Should().BeEmpty();
        }

        [Test]
        public void FindAll_ReturnEmptyCollection_TextHaveNoTokens()
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

        [Test]
        public void FindAll_ThrowsException_WhenTokenStartIntersect()
        {
            var reader = new TokenReader("_a_", new[] {italic, italic});
            Assert.Throws<ArgumentException>(() =>
                reader.FindAll());
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
            var italic = MarkdownTokensFactory.Italic();

            yield return new TestCaseData(new TokenMatch {Start = 0, Length = 5, Token = italic}, "_abc_")
                {TestName = "Single token"};

            yield return new TestCaseData(new TokenMatch {Start = 3, Length = 5, Token = italic}, "qwe_abc_ewq")
                {TestName = "Single token inside word"};
        }

        [Test]
        public void FindAll_ReturnsAllMatches_TextHasOneTypeTokens()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 5, Token = italic},
                new TokenMatch {Start = 6, Length = 5, Token = italic}
            };

            var actual = new TokenReader("_one_ _two_", new[] {italic}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnsAllMatches_WithTwoTokenTypes()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 5, Token = italic},
                new TokenMatch {Start = 6, Length = 7, Token = bold}
            };

            var actual = new TokenReader("_one_ __two__", new[] {italic, bold}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnsAllMatches_ThatNotIntersects()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 5, Token = italic},
                new TokenMatch {Start = 18, Length = 7, Token = bold}
            };

            var actual = new TokenReader("_one_ __W _W__ W_ __two__", new[] {italic, bold}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnsEmptyCollection_TextContainsOnlyUnderlines()
        {
            Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                new TokenReader(new string('_', i), new[] {italic, bold})
                    .FindAll()
                    .Should().BeEmpty($"underlines count = {i}");
            });
        }

        [Test]
        public void FindAll_ReturnNestedMatches_WhenTheyAllowed()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 9, Token = bold},
                new TokenMatch {Start = 3, Length = 3, Token = italic}
            };

            var actual = new TokenReader("__q_w_q__", new[] {bold, italic}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_IgnoreMatches_WithForbiddenNesting()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 9, Token = italic}
            };

            var actual = new TokenReader("_q__w__q_", new[] {bold, italic}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnMatches_ThatNotEscaped()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 4, Length = 3, Token = italic}
            };

            var actual = new TokenReader(@"\_\__a_", new[] {italic}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnMatches_WithEscapedEndSymbolInside()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 6, Token = italic}
            };

            var actual = new TokenReader(@"_a\_b_", new[] {italic}).FindAll();

            actual.Should().BeEquivalentTo(matches);
        }

        [TestCaseSource(nameof(FindAllIgnoreMatchesCases))]
        public void FindAll_IgnoreMatches_IfTags(string text, IEnumerable<IToken> tokens)
        {
            new TokenReader(text, tokens)
                .FindAll()
                .Should().BeEmpty();
        }

        public static IEnumerable<TestCaseData> FindAllIgnoreMatchesCases()
        {
            var italic = MarkdownTokensFactory.Italic();
            var bold = MarkdownTokensFactory.Bold();
            yield return new TestCaseData("one_ two_", new[] {italic}) {TestName = "Whitespace after tags"};
            yield return new TestCaseData("_one__ _two__", new[] {italic, bold}) {TestName = "Tags intersects"};
            yield return new TestCaseData("__one_ qwe", new[] {italic, bold}) {TestName = "Unpaired tags"};
            yield return new TestCaseData("o_ne tw_o", new[] {italic}) {TestName = "In different words"};
            yield return new TestCaseData("qwe_12_3", new[] {italic}) {TestName = "Tags inside digits"};
            yield return new TestCaseData(@"\_ab\_", new[] {italic}) {TestName = "Escaped tags"};
        }
    }
}