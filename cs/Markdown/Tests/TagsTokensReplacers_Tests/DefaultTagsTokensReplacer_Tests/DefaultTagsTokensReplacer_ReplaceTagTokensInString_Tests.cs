using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var result = ReplaceTagTokensInString(text, new TagToken[0], t => t);

            result.Should().Be(text);
        }

        [Test]
        public void ShouldThrow_WhenTextIsNull() =>
            ShouldThrowArgumentNullException(() => ReplaceTagTokensInString(null, new TagToken[0], t => t));

        [Test]
        public void ShouldThrow_WhenReplacableTagsIsNull() =>
            ShouldThrowArgumentNullException(() => ReplaceTagTokensInString("123", null, t => t));

        [Test]
        public void ShouldThrow_WhenReplaceToFunctionIsNull() =>
            ShouldThrowArgumentNullException(() => ReplaceTagTokensInString("asd", new TagToken[0], null));

        [Test]
        public void ShouldCorrectReplaceTags()
        {
            var text = "q-z(asd)z-w";
            var tagsTokens = new TagToken[]
            {
                new TagToken()
                {
                    Tag = new Tag() { Id = "0", Value = "(" },
                    Token = new Token() { StartIndex = 3, Count = 1, Str = text }
                },
                new TagToken()
                {
                    Tag = new Tag() { Id = "1", Value = ")" },
                    Token = new Token() { StartIndex = 7, Count = 1, Str = text }
                },
                new TagToken()
                {
                    Tag = new Tag() { Id = "2", Value = "-"},
                    Token = new Token() { StartIndex = 1, Count = 1, Str = text }
                },
                new TagToken()
                {
                    Tag = new Tag() { Id = "3", Value = "-"},
                    Token = new Token() { StartIndex = 9, Count = 1, Str = text }
                }
            };
            Tag replaceTo(Tag t)
            {
                switch (t.Id)
                {
                    case "0": return new Tag() { Id = "4", Value = "<strong>" };
                    case "1": return new Tag() { Id = "5", Value = "</strong>" };
                    case "2": return new Tag() { Id = "6", Value = "<em>" };
                    case "3": return new Tag() { Id = "7", Value = "</em>" };
                    default: throw new NotSupportedException();
                }
            }

            var result = ReplaceTagTokensInString(text, tagsTokens, replaceTo);

            result.Should().Be("q<em>z<strong>asd</strong>z</em>w");
        }

        private void ShouldThrowArgumentNullException(Action act) =>
            act.Should().Throw<ArgumentNullException>();

        private string ReplaceTagTokensInString(
            string text,
            IEnumerable<TagToken> replacableTags,
            Func<Tag, Tag> replaceTo) =>
            DefaultTagsTokensReplacer.ReplaceTagTokensInString(text, replacableTags, replaceTo);
    }
}