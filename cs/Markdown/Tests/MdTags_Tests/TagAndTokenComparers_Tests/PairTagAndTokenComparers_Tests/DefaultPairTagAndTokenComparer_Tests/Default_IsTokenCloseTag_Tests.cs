using NUnit.Framework;
using FluentAssertions;
using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;

namespace Markdown.Tests.MdTags_Tests.TagAndTokenComparers_Tests.PairTagAndTokenComparers_Tests.DefaultPairTagAndTokenComparer_Tests
{
    class Default_IsTokenCloseTag_Tests
    {
        private DefaultPairTagAndTokenComparer sut;

        [SetUp]
        public void SetUp()
        {
            sut = new DefaultPairTagAndTokenComparer();
        }

        [TestCase("z asdq", 2, 3, "asd")]
        [TestCase(" qw", 1, 2, "qw")]
        public void TokenShouldNotBeCloseTag_IfIgnorableSymbolIsBeforeTag(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenCloseTag(token, tag).Should().BeFalse();
        }

        [TestCase("zxcasdqwe", 3, 3, "asf")]
        [TestCase("(*zxc*)", 0, 2, "(_")]
        public void TokenShouldNotBeCloseTag_IfTokenIsNotEqualCloseTagValue(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenCloseTag(token, tag).Should().BeFalse();
        }

        [TestCase("asd3_1zxc", 4, 1, "_")]
        [TestCase("zxc21**12", 5, 2, "**")]
        public void TokenShouldNotBeCloseTag_IfTokenIsBetweenDigits(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenCloseTag(token, tag).Should().BeFalse();
        }

        [TestCase("_zxc", 0, 1, "_")]
        [TestCase("__qwe__", 2, 3, "qwe")]
        [TestCase("(z) zxc", 2, 1, ")")]
        [TestCase("z2_a", 2, 1, "_")]
        [TestCase("q_3x", 1, 1, "_")]
        public void TokenShouldBeCloseTag(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenCloseTag(token, tag).Should().BeTrue();
        }

        private (Tag tag, Token token) CreateTagAndToken(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var token = new Token { StartIndex = tokenStartIndex, Length = tokenLength, Str = text };
            var tag = new Tag { Id = "1", Value = tagValue };
            return (tag, token);
        }
    }
}