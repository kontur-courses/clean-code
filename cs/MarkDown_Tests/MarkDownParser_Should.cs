using System.Collections.Generic;
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
            availableTagTypes = new List<TagType>(){new EmTag(), new StrongTag()};
        }

        [Test]
        public void GetTokens_ParseTextToken()
        {
            var parser = new MarkDownParser(new TextStream("just some text"), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text")};
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseEmTagToken()
        {
            var parser = new MarkDownParser(new TextStream("_just some text_"), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text", new EmTag()) };
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseStrongTagToken()
        {
            var parser = new MarkDownParser(new TextStream("__just some text__"), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just some text", new StrongTag()) };
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseStrongTagWithInnerEmTagToken()
        {
            var parser = new MarkDownParser(new TextStream("__just _some_ text__"), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just _some_ text", new StrongTag()) };
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseEmTagWithInnerStrongTagToken()
        {
            var parser = new MarkDownParser(new TextStream("_just __some__ text_"), availableTagTypes);
            var expectedTokens = new List<Token>() { new Token(0, "just __some__ text", new EmTag()) };
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ParseMultipleDifferentTokensCorrectly()
        {
            var parser = new MarkDownParser(new TextStream("_just_ __some__ __different _tokens"), availableTagTypes);
            var expectedTokens = new List<Token>()
            {
                new Token(0, "just", new EmTag()),
                new Token(6, " "),
                new Token(7, "some", new StrongTag()),
                new Token(15, " __different _tokens")
            };
            parser.GetTokens().Should().BeEquivalentTo(expectedTokens);
        }
    }
}
