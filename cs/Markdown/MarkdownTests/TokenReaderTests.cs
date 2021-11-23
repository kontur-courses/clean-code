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
        private List<IToken> boldAndItalic;

        [SetUp]
        public void SetUp()
        {
            italic = new ItalicToken();
            bold = new BoldToken();
            boldAndItalic = new List<IToken> {bold, italic};
        }

        [Test]
        public void Constructor_ThrowsException_IfTokensNull() =>
            Assert.That(() => new TokenReader(null), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void FindAll_ThrowsException_IfTextNull()
        {
            var reader = new TokenReader(new List<IToken> {italic});
            Assert.That(() => reader.FindAll(null), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void FindAll_ThrowsException_IfTokenStartIntersect()
        {
            var reader = new TokenReader(new List<IToken> {italic, italic});
            Assert.That(() => reader.FindAll("_a_"), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        public void FindAll_ReturnEmptyCollection_IfTokensNotGiven()
        {
            new TokenReader(Enumerable.Empty<IToken>().ToList())
                .FindAll("_text_")
                .Should().BeEmpty();
        }

        [Test]
        public void FindAll_ReturnEmptyCollection_IfTextHaveNoTokens()
        {
            new TokenReader(MarkdownTokensFactory.GetTokens().ToList())
                .FindAll("qwerty")
                .Should().BeEmpty();
        }

        [TestCaseSource(nameof(FindAllReturnSingleMatchCases))]
        public void FindAll_ReturnSingleMatch_IfTextHas(TokenMatch expectedMatch, string text)
        {
            var matches = new[] {expectedMatch};
            var actual = new TokenReader(new List<IToken> {expectedMatch.Token}).FindAll(text);
            actual.Should().BeEquivalentTo(matches);
        }

        private static IEnumerable<TestCaseData> FindAllReturnSingleMatchCases()
        {
            var italic = new ItalicToken();

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

            var actual = new TokenReader(new List<IToken> {italic}).FindAll("_one_ _two_");

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

            var actual = new TokenReader(boldAndItalic).FindAll("_one_ __two__");

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

            var actual = new TokenReader(boldAndItalic).FindAll("_one_ __W _W__ W_ __two__");

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnsEmptyCollection_TextContainsOnlyUnderlines()
        {
            Enumerable.Range(1, 10).ToList().ForEach(i =>
            {
                new TokenReader(boldAndItalic)
                    .FindAll(new string('_', i))
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

            var actual = new TokenReader(boldAndItalic).FindAll("__q_w_q__");

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_IgnoreMatches_WithForbiddenNesting()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 9, Token = italic}
            };

            var actual = new TokenReader(boldAndItalic).FindAll("_q__w__q_");

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnMatches_ThatNotEscaped()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 4, Length = 3, Token = italic}
            };

            var actual = new TokenReader(new List<IToken> {italic}).FindAll(@"\_\__a_");

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnMatches_WithEscapedEndSymbolInside()
        {
            var matches = new[]
            {
                new TokenMatch {Start = 0, Length = 6, Token = italic}
            };

            var actual = new TokenReader(new List<IToken> {italic}).FindAll(@"_a\_b_");

            actual.Should().BeEquivalentTo(matches);
        }

        [Test]
        public void FindAll_ReturnSameMatches_EveryTime()
        {
            var reader = new TokenReader(MarkdownTokensFactory.GetTokens().ToList());
            var firstMatches = reader.FindAll("# _a_ __bc__");
            var secondMatches = reader.FindAll("# _a_ __bc__");
            secondMatches.Should().BeEquivalentTo(firstMatches);
        }

        [TestCaseSource(nameof(FindAllIgnoreMatchesCases))]
        public void FindAll_IgnoreMatches_IfTags(string text, List<IToken> tokens)
        {
            new TokenReader(tokens)
                .FindAll(text)
                .Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> FindAllIgnoreMatchesCases()
        {
            var italic = new ItalicToken();
            var bold = new BoldToken();
            yield return new TestCaseData("one_ two_", new List<IToken> {italic}) {TestName = "Whitespace after tags"};
            yield return new TestCaseData("__one_ qwe", new List<IToken> {italic, bold}) {TestName = "Unpaired tags"};
            yield return new TestCaseData("o_ne tw_o", new List<IToken> {italic}) {TestName = "In different words"};
            yield return new TestCaseData("qwe_12_3", new List<IToken> {italic}) {TestName = "Tags inside digits"};
            yield return new TestCaseData(@"\_ab\_", new List<IToken> {italic}) {TestName = "Escaped tags"};
            yield return new TestCaseData("_one__ _two__", new List<IToken> {italic, bold})
                {TestName = "Tags intersects"};
        }
    }
}