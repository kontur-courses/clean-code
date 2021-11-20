using System;
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

        [TestCaseSource(nameof(GetCorrectnessTestCases))]
        public void Parse_ShouldReturnCorrectNodes(IEnumerable<Token> tokens, IEnumerable<TokenNode> expected)
        {
            var parsed = sut.Parse(tokens);

            parsed.Should().Equal(expected);
        }

        public static IEnumerable<TestCaseData> GetCorrectnessTestCases()
        {
            yield return new TestCaseData(
                Array.Empty<Token>(),
                Array.Empty<TokenNode>()
            ).SetName("Parse_ShouldNotBeApplied_WhenNoToken");
            
            yield return new TestCaseData(
                new[] { Token.Text("text") },
                new[] { Token.Text("text").ToNode() }
            ).SetName("Only text token");

            yield return new TestCaseData(
                new[] { Token.Text("A"), Token.Text("B") },
                new[] { Token.Text("AB").ToNode() }
            ).SetName("Concatenate text tokens");

            yield return new TestCaseData(
                new[] { Token.Escape },
                new[] { Token.Escape.ToText().ToNode() }
            ).SetName("Should transform escape to text");

            yield return new TestCaseData(
                new[] { Token.Escape, Token.Text("Text") },
                new[] { Token.Text("\\Text").ToNode() }
            ).SetName("Should concatenate escape and text");

            yield return new TestCaseData(
                new[] { Token.Escape, Token.Cursive },
                new[] { Token.Cursive.ToText().ToNode() }
            ).SetName("Should escape cursive character when single character");

            yield return new TestCaseData(
                new[] { Token.Escape, Token.Bold },
                new[] { Token.Cursive.ToText().ToNode(), Token.Cursive.ToText().ToNode() }
            ).SetName("Should escape cursive character when single bold character");

            yield return new TestCaseData(
                new[] { Token.Cursive, Token.Text("Text"), Token.Cursive },
                new[] { new TokenNode(Token.Cursive, Token.Text("Text").ToNode()) }
            ).SetName("Should apply cursive format when surrounds text");

            yield return new TestCaseData(
                new[] { Token.Cursive, Token.Text("Text") },
                new[] { Token.Cursive.ToText().ToNode(), Token.Text("Text").ToNode() }
            ).SetName("Should make cursive as text when just before text");

            yield return new TestCaseData(
                new[] { Token.Text("Text"), Token.Cursive },
                new[] { Token.Text("Text").ToNode(), Token.Cursive.ToText().ToNode() }
            ).SetName("Should make cursive as text when just before text");

            yield return new TestCaseData(
                new[] { Token.Text("A"), Token.Cursive, Token.Text("B"), Token.Cursive },
                new[] { Token.Text("A").ToNode(), new TokenNode(Token.Cursive, Token.Text("B").ToNode()) }
            ).SetName("Should apply cursive when formatting text after some text");
        }
    }
}