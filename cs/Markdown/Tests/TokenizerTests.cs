using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using Markdown.Tags;
using Markdown.TagStore;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void Tokenize_IfStringHasTag_ShouldReturnTagToken()
        {
            const string text = "_Lorem ipsum dolor_ sit amet";
            var tagStore = A.Fake<ITagStore>();
            A.CallTo(() => tagStore.GetTagType(text, A<int>.Ignored, A<int>.Ignored)).Returns(TagType.Emphasized);
            A.CallTo(() => tagStore.GetTagRole(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored))
                .ReturnsNextFromSequence(TagRole.Opening, TagRole.Closing);
            A.CallTo(() => tagStore.GetTagsValues()).Returns(new[] { "_" });
            var sut = new Tokenizer(tagStore);

            var tagTokens = sut.Tokenize(text).ToArray();

            tagTokens.Should().BeEquivalentTo(new[]
            {
                new Token(TagType.Emphasized, 0, 1, TagRole.Opening, TokenType.Tag),
                new Token(TagType.Emphasized, 18, 1, TagRole.Closing, TokenType.Tag)
            });
        }
    }
}