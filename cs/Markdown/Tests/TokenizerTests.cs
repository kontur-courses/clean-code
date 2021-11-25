using System;
using System.Collections.Generic;
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
            const string text = "Lorem _ipsum_ _dolor_ sit amet";
            var tagStore = A.Fake<ITagStore>();
            A.CallTo(() => tagStore.GetTagType(text, A<int>.Ignored, A<int>.Ignored)).Returns(TagType.Emphasized);
            A.CallTo(() => tagStore.GetTagRole(A<string>.Ignored, A<int>.Ignored, A<int>.Ignored))
                .ReturnsNextFromSequence(TagRole.Opening, TagRole.Closing, TagRole.Opening, TagRole.Closing);
            A.CallTo(() => tagStore.GetTagValues()).Returns(new HashSet<string> { "_" });
            var sut = new Tokenizer(tagStore);

            var tagTokens = sut.Tokenize(text).OrderBy(t => t.Start).ToArray();

            tagTokens.Should().BeEquivalentTo(new[]
            {
                new Token(TokenType.Text, 0, 6),
                new Token(TokenType.Tag, TagType.Emphasized, TagRole.Opening, 6, 1),
                new Token(TokenType.Text, 7, 5),
                new Token(TokenType.Tag, TagType.Emphasized, TagRole.Closing, 12, 1),
                new Token(TokenType.Text, 13, 1),
                new Token(TokenType.Tag, TagType.Emphasized, TagRole.Opening, 14, 1),
                new Token(TokenType.Text, 15, 5),
                new Token(TokenType.Tag, TagType.Emphasized, TagRole.Closing, 20, 1),
                new Token(TokenType.Text, 21, 9)
            });
        }
    }
}