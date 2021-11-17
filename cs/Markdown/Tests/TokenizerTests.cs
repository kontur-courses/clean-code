using System.Linq;
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
        public void ReturnTagToken_WhenStringHasTag()
        {
            const string text = "_Lorem ipsum dolor_ sit amet";
            var sut = new Tokenizer(new MdTagStore());

            var tagTokens = sut.Tokenize(text).ToArray();

            tagTokens.Should().BeEquivalentTo(new[]
            {
                new Token(TagType.Emphasized, 0, 1, TagRole.Opening),
                new Token(TagType.Emphasized, 18, 1, TagRole.Closing)
            });
        }
    }
}