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
            var tagRoles = new[] { TagRole.Opening, TagRole.Closing };
            var tagStore = A.Fake<ITagStore>();
            A.CallTo(() => tagStore.GetTagType("_")).Returns(TagType.Emphasized);
            A.CallTo(() => tagStore.GetTagRole(A<string>.Ignored, A<char>.Ignored, A<char>.Ignored))
                .ReturnsNextFromSequence(tagRoles);
            A.CallTo(() => tagStore.GetTagsValues()).Returns(new []{"_"});
            
            var sut = new Tokenizer(tagStore);

            var tagTokens = sut.Tokenize(text).ToArray();

            tagTokens.Should().BeEquivalentTo(new[]
            {
                new Token(TagType.Emphasized, 0, 1, TagRole.Opening),
                new Token(TagType.Emphasized, 18, 1, TagRole.Closing)
            });
        }
    }
}