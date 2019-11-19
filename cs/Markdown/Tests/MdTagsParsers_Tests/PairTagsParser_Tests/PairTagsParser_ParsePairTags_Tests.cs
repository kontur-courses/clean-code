using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.MdTagsParsers;

namespace Markdown.Tests.MdTagsParsers_Tests.PairTagsParser_Tests
{
    class PairTagsParser_ParsePairTags_Tests
    {
        private PairTagsParser sut;

        [SetUp]
        public void SetUp()
        {
            sut = new PairTagsParser();
        }

        [Test]
        public void ShouldThrow_WhenTextArgumentIsNull() =>
            ShouldThrowArgumentNullException(() => sut.ParsePairTags(null, new[] { 1, 2 }));

        [Test]
        public void ShouldThrow_WhenIgnorableIndexesIsNull() =>
            ShouldThrowArgumentNullException(() => sut.ParsePairTags("asdczcxz", null));

        [TestCase("")]
        [TestCase("asdqwe")]
        [TestCase("\\asd")]
        public void ShouldReturnEmptyEnumeration_WhenTextNotContainsTags(string text)
        {
            var result = sut.ParsePairTags(text, new int[0]);

            result.Should().BeEmpty();
        }

        [Test]
        public void ShouldReturnCorrectTagPairs()
        {
            var text = ">__zxc\\__asd_qwe____asd__";

            var result = sut.ParsePairTags(text, new[] { 6, 7 });

            result.Should().BeEquivalentTo(
            (
                CreateTagToken("open__", "__", 18, 2, text),
                CreateTagToken("close__", "__", 23, 2, text)
            ),
            (
                CreateTagToken("open_", "_", 8, 1, text),
                CreateTagToken("close_", "_", 12, 1, text)
            ),
            (
                CreateTagToken("open__", "__", 1, 2, text),
                CreateTagToken("close__", "__", 16, 2, text)
            ),
            (
                CreateTagToken("open>", ">", 0, 1, text),
                CreateTagToken("close>", "\n", 25, 1, text + "\n")
            )
            );
        }

        [Test]
        public void ShouldIgnoreIgnorableIndexesFromArgument()
        {
            var text = "zxc__qwe_s";

            var result = sut.ParsePairTags(text, new[] { 4 });

            result.Should().BeEquivalentTo(
            (
                CreateTagToken("open_", "_", 3, 1, text),
                CreateTagToken("close_", "_", 8, 1, text)
            )
            );
        }

        private TagToken CreateTagToken(
            string tagId, string tagValue,
            int tokenStartIndex, int tokenLength, string tokenStr) =>
            new TagToken
            {
                Tag = new Tag { Id = tagId, Value = tagValue },
                Token = new Token { StartIndex = tokenStartIndex, Length = tokenLength, Str = tokenStr }
            };

        private void ShouldThrowArgumentNullException(Action act) =>
            act.Should().Throw<ArgumentNullException>();
    }
}