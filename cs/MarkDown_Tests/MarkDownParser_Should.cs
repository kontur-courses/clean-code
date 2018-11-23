using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MarkDown;
using MarkDown.TagTypes;
using NUnit.Framework;

namespace MarkDown_Tests
{
    [TestFixture]
    public class MarkDownParser_Should
    {
        private List<TagType> availableTagTypes;
        [SetUp]
        public void SetUp()
        {
            availableTagTypes = new List<TagType>(){new EmTag(), new StrongTag(), new ATag()};
        }

        [Test]
        public void GetTokens_ParseTextToken()
        {
            var parser = new MarkDownParser("just some text".GetCharStates(), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text".GetCharStates())};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseEmTagToken()
        {
            var parser = new MarkDownParser("_just some text_".GetCharStates(), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text".GetCharStates(), new EmTag()) };
            expectedTokens[0].InnerTokens = new[] {new Token(0, "just some text".GetCharStates())};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseStrongTagToken()
        {
            var parser = new MarkDownParser("__just some text__".GetCharStates(), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text".GetCharStates(), new StrongTag()) };
            expectedTokens[0].InnerTokens = new[] { new Token(0, "just some text".GetCharStates())};
            var tokens = parser.GetTokens().ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseStrongTagWithInnerEmTagToken()
        {
            var parser = new MarkDownParser("__just _some_ text__", availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just _some_ text", new StrongTag()) };
            var innerTokens = new[] { new Token(0, "just "), new Token(5, "some", new EmTag()), new Token(11, " text")};
            innerTokens[1].InnerTokens = new[] {new Token(0, "some")};
            expectedTokens[0].InnerTokens = innerTokens;
            var tokens = parser.GetTokens().ToList();
            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseEmTagWithInnerStrongTagToken()
        {
            var parser = new MarkDownParser("_just __some__ text_", availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just __some__ text", new EmTag()) };
            expectedTokens[0].InnerTokens = new[] {new Token(0, "just __some__ text")};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseMultipleDifferentTokensCorrectly()
        {
            var parser = new MarkDownParser("_just_ __some__ __different _tokens", availableTagTypes);
            var expectedTokens = new List<Token>()
            {
                new Token(0, "just", new EmTag()),
                new Token(6, " "),
                new Token(7, "some", new StrongTag()),
                new Token(15, " __different _tokens")
            };

            expectedTokens[0].InnerTokens = new[] {new Token(0, "just")};
            expectedTokens[2].InnerTokens = new[] {new Token(0, "some")};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseATag()
        {
            var parser = new MarkDownParser("[foo](bar)", availableTagTypes);
            var expectedTokens = new[] { new Token(0, "bar", new ATag(), "foo")};
            expectedTokens[0].InnerTokens = new[] { new Token(0, "bar")};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }
    }
}
