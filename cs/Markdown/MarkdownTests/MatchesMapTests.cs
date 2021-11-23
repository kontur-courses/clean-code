using System;
using FluentAssertions;
using Markdown;
using Markdown.Models;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class MatchesMapTests
    {
        private MatchesMap map;
        private TokenMatch match;

        [SetUp]
        public void SetUp()
        {
            match = new TokenMatch {Start = 0, Length = 3, Token = new ItalicToken()};
            map = new MatchesMap(new[] {match});
        }

        [Test]
        public void Constructor_ThrowsException_IfMatchesNull() =>
            Assert.That(() => new MatchesMap(null), Throws.InstanceOf<ArgumentException>());

        [Test]
        public void TryGetTagReplacerAtPosition_ReturnsTrue_IfPositionOnMatchStart() =>
            map.TryGetTagReplacerAtPosition(match.Start, out var _).Should().BeTrue();

        [Test]
        public void TryGetTagReplacerAtPosition_ReturnsTrue_IfPositionOnMatchEnd() =>
            map.TryGetTagReplacerAtPosition(match.Length - 1, out var _).Should().BeTrue();

        [Test]
        public void TryGetTagReplacerAtPosition_ReturnsFalse_IfPositionNotOnMatch() =>
            map.TryGetTagReplacerAtPosition(1, out var _).Should().BeFalse();


        [Test]
        public void TryGetTagReplacerAtPosition_ReturnsOpenTag_IfPositionOnMatchStart()
        {
            map.TryGetTagReplacerAtPosition(match.Start, out var replacer);
            replacer.Should().Be(match.Token.TokenHtmlRepresentation.OpenTag);
        }

        [Test]
        public void TryGetTagReplacerAtPosition_ReturnsCloseTag_IfPositionOnMatchEnd()
        {
            map.TryGetTagReplacerAtPosition(match.Length - 1, out var replacer);
            replacer.Should().Be(match.Token.TokenHtmlRepresentation.CloseTag);
        }
    }
}