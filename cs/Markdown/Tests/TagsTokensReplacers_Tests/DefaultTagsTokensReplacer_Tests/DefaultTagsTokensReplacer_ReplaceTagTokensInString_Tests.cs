using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using Markdown.TagsTokensReplacers;

namespace Markdown.Tests.TagsTokensReplacers_Tests.DefaultTagsTokensReplacer_Tests
{
    class DefaultTagsTokensReplacer_ReplaceTagTokensInString_Tests
    {
        [TestCase("123zxc")]
        [TestCase("123_zx_c")]
        public void ShouldReturnSameText_IfReplacableTagsEnumerableIsEmpty(string text)
        {
            var result = DefaultTagsTokensReplacer.ReplaceTagTokensInString(text, new TagToken[0], t => null);

            result.Should().Be(text);
        }

        [Test]
        public void ShouldThrow_WhenTextIsNull() =>
            ShouldThrowArgumentNullException(null, new TagToken[0], t => t);

        [Test]
        public void ShouldThrow_WhenReplacableTagsIsNull() =>
            ShouldThrowArgumentNullException("123", null, t => t);

        [Test]
        public void ShouldThrow_WhenReplaceToFunctionIsNull() =>
            ShouldThrowArgumentNullException("asd", new TagToken[0], null);

        [Test]
        public void ShouldCorrectReplaceTags()
        {
            var text = "q-z(asd)z-w";
            var tagsTokens = new TagToken[]
            {
                CreateTagToken(text, 3, 1, "0", "("),
                CreateTagToken(text, 7, 1, "1", ")"),
                CreateTagToken(text, 1, 1, "2", "-"),
                CreateTagToken(text, 9, 1, "3", "-")
            };
            Tag replaceTo(Tag t)
            {
                switch (t.Id)
                {
                    case "0": return new Tag { Id = "4", Value = "<strong>" };
                    case "1": return new Tag { Id = "5", Value = "</strong>" };
                    case "2": return new Tag { Id = "6", Value = "<em>" };
                    case "3": return new Tag { Id = "7", Value = "</em>" };
                    default: throw new NotSupportedException();
                }
            }

            var result = DefaultTagsTokensReplacer.ReplaceTagTokensInString(text, tagsTokens, replaceTo);

            result.Should().Be("q<em>z<strong>asd</strong>z</em>w");
        }

        private void ShouldThrowArgumentNullException(string text, IEnumerable<TagToken> replacableTags, Func<Tag, Tag> replaceTo)
        {
            Action act = () => DefaultTagsTokensReplacer.ReplaceTagTokensInString(text, replacableTags, replaceTo);
            act.Should().Throw<ArgumentNullException>();
        }

        private TagToken CreateTagToken(
            string text, int tokenStartIndex, int tokenLength,
            string tagId, string tagValue)
        {
            var token = new Token { StartIndex = tokenStartIndex, Length = tokenLength, Str = text };
            var tag = new Tag { Id = tagId, Value = tagValue };
            return new TagToken { Tag = tag, Token = token };
        }
    }
}