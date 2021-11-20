using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        private TokenParser sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TokenParser();
        }

        [Test]
        public void Parse_ShouldThrowException_WhenArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null));
        }

        [Test]
        public void Parse_ShouldNotBeApplied_WhenNoToken()
        {
            var tokens = Enumerable.Empty<Token>();

            var parsed = sut.Parse(tokens);

            parsed.Should().BeEmpty();
        }

        [Test]
        public void Parse_ShouldNotBeApplied_WhenOnlyTextTokens()
        {
            var tokens = new[] { Token.Text("text") };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(tokens[0]));
        }

        [Test]
        public void Parse_ShouldConcatenateTextTokens()
        {
            var tokens = new[]
            {
                Token.Text("A"),
                Token.Text("B")
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(Token.Text("AB")));
        }
        
        [Test]
        public void Parse_ShouldTransformEscapeSymbolToText()
        {
            var tokens = new[]
            {
                Token.Escape
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(Token.Text("\\")));
        }

        [Test]
        public void Parse_ShouldConcatenateEscapeSymbolAndText()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Text("Text")
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(Token.Text("\\Text")));
        }

        [Test]
        public void Parse_ShouldEscapeCursive_WhenSingleCursive()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Cursive
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(Token.Text("_")));
        }

        [Test]
        public void Parse_ShouldEscapeCursive_WhenBold()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Bold
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(Token.Text("_").ToNode(), Token.Text("_").ToNode());
        }

        [Test]
        public void Parse_ShouldPlaceTextToken_WhenSurroundedByCursiveIntoOneTokenNode()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Text"),
                Token.Cursive
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(new TokenNode(Token.Cursive, Token.Text("Text").ToNode()));
        }
        
        [Test]
        public void Parse_ShouldMakeToText_WhenStartsWithCursiveButNotEnds()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Text"),
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(Token.Text(Token.Cursive.Value).ToNode(), Token.Text("Text").ToNode());
        }
        
        [Test]
        public void Parse_ShouldMakeToTextCursive_WhenEndsWithCursive()
        {
            var tokens = new[]
            {
                Token.Text("Text"),
                Token.Cursive,
            };

            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(Token.Text("Text").ToNode(), Token.Text(Token.Cursive.Value).ToNode());
        }
        
        [TestCaseSource(typeof(TokenParserTests), nameof(GenerateCorrectnessCases))]
        public void Parse_ShouldParseCorrectly(IEnumerable<Token> tokens, IEnumerable<TokenNode> nodes)
        {
            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(nodes);
        }

        public static IEnumerable<TestCaseData> GenerateCorrectnessCases()
        {
            yield return new TestCaseData(
                new[] { Token.Text("A"), Token.Cursive, Token.Text("B"), Token.Cursive },
                new[] { Token.Text("A").ToNode(), new TokenNode(Token.Cursive, Token.Text("B").ToNode())}
            ).SetName("Should apply cursive when formatting text after some text");
        }
    }
}