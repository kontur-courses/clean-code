using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Extensions;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenParserTests
    {
        private TokenParser.TokenParser sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TokenParser.TokenParser();
        }

        [Test]
        public void Parse_ShouldThrowException_WhenArgumentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => sut.Parse(null));
        }

        [Test]
        public void Parse_ShouldNotApplyParsing_WhenNoToken()
        {
            var tokens = Array.Empty<Token>();
            var expected = Array.Empty<TokenNode>();
            AssertParse(tokens, expected);
        }


        [Test]
        public void Parse_ShouldNotApply_WhenOnlyTextToken()
        {
            var tokens = new[]
            {
                Token.Text("text")
            };
            var expected = new[]
            {
                Token.Text("text").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldConcatenateTextTokens()
        {
            var tokens = new[]
            {
                Token.Text("A"),
                Token.Text("B")
            };
            var expected = new[]
            {
                Token.Text("AB").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldTransformEscapeToText()
        {
            var tokens = new[]
            {
                Token.Escape
            };
            var expected = new[]
            {
                Token.Escape.ToText().ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldConcatenateEscapeAndText()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Text("Text")
            };
            var expected = new[]
            {
                Token.Text("\\Text").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldEscapeCursiveCharacterWhen_SingleCharacter()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Cursive.ToText().ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldEscapeCursiveCharacter_WhenSingleBoldCharacter()
        {
            var tokens = new[]
            {
                Token.Escape,
                Token.Bold
            };
            var expected = new[]
            {
                Token.Text("__").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyCursiveFormat_WhenSurroundsText()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Text"),
                Token.Cursive
            };
            var expected = new[]
            {
                new TokenNode(Token.Cursive, Token.Text("Text").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeCursiveAsText_WhenJustBeforeText()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Text")
            };
            var expected = new[]
            {
                Token.Text("_Text").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeCursiveAsText_WhenJustAfterText()
        {
            var tokens = new[]
            {
                Token.Text("Text"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Text("Text_").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyBoldFormat_WhenSurroundsText()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("Text"),
                Token.Bold
            };
            var expected = new[]
            {
                new TokenNode(Token.Bold, Token.Text("Text").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeBoldAsText_WhenJustBeforeText()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("Text")
            };
            var expected = new[]
            {
                Token.Text("__Text").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeBoldAsText_WhenSingleAfterText()
        {
            var tokens = new[]
            {
                Token.Text("Text"),
                Token.Bold
            };
            var expected = new[]
            {
                Token.Text("Text__").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyCursive_WhenSurroundsTextAfterText()
        {
            var tokens = new[]
            {
                Token.Text("A"),
                Token.Bold,
                Token.Text("B"),
                Token.Bold
            };
            var expected = new[]
            {
                Token.Text("A").ToNode(),
                new TokenNode(Token.Bold, Token.Text("B").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyCursive_WhenWithinBold()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("Start"),
                Token.Cursive,
                Token.Text("Cursive"),
                Token.Cursive,
                Token.Text("End"),
                Token.Bold
            };
            var expected = new[]
            {
                new TokenNode(Token.Bold,
                    new[]
                    {
                        Token.Text("Start").ToNode(),
                        new TokenNode(Token.Cursive, Token.Text("Cursive").ToNode()),
                        Token.Text("End").ToNode()
                    })
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyBold_WhenWithinCursiveWithText()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Start"),
                Token.Bold,
                Token.Text("Bold"),
                Token.Bold,
                Token.Text("End"),
                Token.Cursive
            };
            var expected = new[] { new TokenNode(Token.Cursive, Token.Text("Start__Bold__End").ToNode()) };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyCursive_WhenSurroundsNumber()
        {
            AssertParseNotApplyWhenSurroundsNumber(Token.Cursive);
        }

        [Test]
        public void Parse_ShouldNotApplyBold_WhenSurroundsNumber()
        {
            AssertParseNotApplyWhenSurroundsNumber(Token.Bold);
        }

        private void AssertParseNotApplyWhenSurroundsNumber(Token token)
        {
            var character = token.Value;
            var tokens = new[] { token, Token.Text("12"), token };
            var expected = new[] { Token.Text($"{character}12{character}").ToNode() };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyCursive_WhenSurroundsBeginOfWord()
        {
            AssertParseSurroundsStartOfWord(Token.Cursive);
        }

        [Test]
        public void Parse_ShouldApplyCursive_WhenSurroundsMiddleOfWord()
        {
            AssertParseSurroundsMiddleOfWord(Token.Cursive);
        }

        [Test]
        public void Parse_ShouldApplyCursive_WhenSurroundsEndOfWord()
        {
            AssertParseSurroundsEndOfWord(Token.Cursive);
        }

        [Test]
        public void Parse_ShouldApplyBold_WhenSurroundsBeginningOfWord()
        {
            AssertParseSurroundsStartOfWord(Token.Bold);
        }

        [Test]
        public void Parse_ShouldApplyBold_WhenSurroundsMiddleOfWord()
        {
            AssertParseSurroundsMiddleOfWord(Token.Bold);
        }

        [Test]
        public void Parse_ShouldApplyBold_WhenSurroundsEndOfWord()
        {
            AssertParseSurroundsEndOfWord(Token.Bold);
        }

        [Test]
        public void Parse_ShouldNotApplyCursive_WhenInMiddleOfDifferentWords(
            [ValueSource(typeof(TokenParserTests), nameof(GetWordSeparators))]
            string wordSeparator)
        {
            AssertParseShouldNotApplyWhenInMiddleOfDifferentWords(Token.Cursive, wordSeparator);
        }

        [Test]
        public void Parse_ShouldNotApplyBold_WhenInMiddleOfDifferentWords(
            [ValueSource(typeof(TokenParserTests), nameof(GetWordSeparators))]
            string wordSeparator)
        {
            AssertParseShouldNotApplyWhenInMiddleOfDifferentWords(Token.Bold, wordSeparator);
        }

        [Test]
        public void Parse_ShouldNotApplyBoldAndCursive_WhenBoldThenCursive()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("Text"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Text("__Text_").ToNode()
            };
            AssertParse(tokens, expected);
        }

        private void AssertParseShouldNotApplyWhenInMiddleOfDifferentWords(Token token, string wordSeparator)
        {
            var tokenCharacter = token.Value;
            var tokens = new[]
            {
                Token.Text("Sta"),
                token,
                Token.Text($"rt{wordSeparator}te"),
                token,
                Token.Text("xt")
            };
            var expected = new[]
            {
                Token.Text($"Sta{tokenCharacter}rt{wordSeparator}te{tokenCharacter}xt").ToNode()
            };
            AssertParse(tokens, expected);
        }

        public static IEnumerable<string> GetWordSeparators() => " !?.,;:\\|{}[]()*&^$".Select(x => x.ToString());

        private void AssertParseSurroundsStartOfWord(Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("Beg"),
                token,
                Token.Text("in of word")
            };
            var expected = new[]
            {
                new TokenNode(token, Token.Text("Beg").ToNode()),
                Token.Text("in of word").ToNode()
            };
            AssertParse(tokens, expected);
        }

        private void AssertParseSurroundsMiddleOfWord(Token token)
        {
            var tokens = new[]
            {
                Token.Text("Surround m"),
                token,
                Token.Text("iddl"),
                token,
                Token.Text("e part")
            };
            var expected = new[]
            {
                Token.Text("Surround m").ToNode(),
                new TokenNode(token, Token.Text("iddl").ToNode()),
                Token.Text("e part").ToNode()
            };
            AssertParse(tokens, expected);
        }

        private void AssertParseSurroundsEndOfWord(Token token)
        {
            var tokens = new[]
            {
                Token.Text("Surround m"),
                token,
                Token.Text("iddl"),
                token,
                Token.Text("e part")
            };
            var expected = new[]
            {
                Token.Text("Surround m").ToNode(),
                new TokenNode(token, Token.Text("iddl").ToNode()),
                Token.Text("e part").ToNode()
            };
            AssertParse(tokens, expected);
        }

        private void AssertParse(IEnumerable<Token> tokens, IEnumerable<TokenNode> expectedNodes)
        {
            var parsed = sut.Parse(tokens);
            parsed.Should().Equal(expectedNodes);
        }
    }
}