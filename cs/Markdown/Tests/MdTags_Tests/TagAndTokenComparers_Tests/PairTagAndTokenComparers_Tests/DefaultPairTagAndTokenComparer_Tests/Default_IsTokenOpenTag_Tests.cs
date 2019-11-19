using NUnit.Framework;
using FluentAssertions;
using Markdown.MdTags.TagAndTokenComparers.PairTagAndTokenComparers;

namespace Markdown.Tests.MdTags_Tests.TagAndTokenComparers_Tests.PairTagAndTokenComparers_Tests.DefaultPairTagAndTokenComparer_Tests
{
    class Default_IsTokenOpenTag_Tests
    {
        private DefaultPairTagAndTokenComparer sut;

        [SetUp]
        public void SetUp()
        {
            sut = new DefaultPairTagAndTokenComparer();
        }

        [TestCase("asd qwe", 0, 3, "asd")]
        [TestCase("_ _asd_", 0, 1, "_")]
        public void TokenShouldNotBeOpenTag_IfIgnorableSymbolIsAfterTag(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenOpenTag(token, tag).Should().BeFalse();
        }

        [TestCase("asdqweasd", 0, 3, "asf")]
        [TestCase("*_asd_", 0, 2, "_*")]
        public void TokenShouldNotBeOpenTag_IfTokenIsNotEqualsOpenTagValue(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenOpenTag(token, tag).Should().BeFalse();
        }

        [TestCase("asd3_3zxc", 3, 1, "_")]
        [TestCase("qwe33zxc23asd", 5, 3, "zxc")]
        [TestCase("zx3asdqwe", 3, 3, "asd")]
        [TestCase("qasd4", 1, 3, "asd")]
        public void TokenShouldNotBeOpenTag_IfTokenIsNearWithDigit(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenOpenTag(token, tag).Should().BeFalse();
        }

        [TestCase("asd**zxc", 3, 2, "**")]
        [TestCase("asd*", 3, 1, "*")]
        [TestCase("a *qwe", 2, 1, "*")]
        public void TokenShouldBeOpenTag(
            string text, int tokenStartIndex, int tokenLength,
            string tagValue)
        {
            var (tag, token) = CreateTagAndToken(text, tokenStartIndex, tokenLength, tagValue);

            sut.IsTokenOpenTag(token, tag).Should().BeTrue();
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