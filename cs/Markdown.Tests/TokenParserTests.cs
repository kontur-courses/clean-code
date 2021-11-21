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
        public void Parse_ShouldApplyBoldFormat_WhenSurroundsWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                token
            };
            var expected = new[]
            {
                new TokenNode(token, Token.Text("A").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyBoldFormat_WhenSurroundsText(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A B C"),
                token
            };
            var expected = new[]
            {
                new TokenNode(token, Token.Text("A B C").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeFormattingAsText_WhenSingleBeforeText(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("Text")
            };
            var expected = new[]
            {
                Token.Text($"{token.Value}Text").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldMakeFormattingAsText_WhenSingleAfterText(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                Token.Text("Text"),
                token
            };
            var expected = new[]
            {
                Token.Text($"Text{token.Value}").ToNode()
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
        public void Parse_ShouldNotApplyFormatting_WhenSurroundsNumber(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var character = token.Value;
            var tokens = new[] { token, Token.Text("12"), token };
            var expected = new[] { Token.Text($"{character}12{character}").ToNode() };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldApplyFormatting_WhenSurroundsBeginningOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
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

        [Test]
        public void Parse_ShouldApplyFormatting_WhenSurroundsMiddleOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
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

        [Test]
        public void Parse_ShouldApplyFormatting_WhenSurroundsEndOfWord(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
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

        [Test]
        public void Parse_ShouldNotApplyFormatting_WhenInMiddleOfDifferentWords(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token,
            [ValueSource(nameof(GetWordSeparators))]
            string wordSeparator)
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

        [Test]
        public void Parse_ShouldNotApplyBoldAndCursive_WhenCursiveBetweenBold()
        {
            var tokens = new[]
            {
                Token.Bold,
                Token.Text("Text "),
                Token.Cursive,
                Token.Text("Text"),
                Token.Bold
            };
            var expected = new[]
            {
                Token.Text("__Text _Text__").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyBoldAndCursive_WhenBoldBetweenCursive()
        {
            var tokens = new[]
            {
                Token.Cursive,
                Token.Text("Text "),
                Token.Bold,
                Token.Text("Text"),
                Token.Cursive
            };
            var expected = new[]
            {
                Token.Text("_Text __Text_").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyFormatting_WhenBeforeWhitespace(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text(" Text"),
                token
            };
            var expected = new[]
            {
                Token.Text($"{token.Value} Text{token.Value}").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyFormatting_WhenOpening_AndBeforeWhitespace_AndThenTheThirdToken(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text(" Text"),
                token,
                Token.Text("Text"),
                token
            };
            var expected = new[]
            {
                Token.Text($"{token.Value} Text{token.Value}Text{token.Value}").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyUnderscore_WhenClosing_AndAfterWhitespace(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("Text "),
                token
            };
            var expected = new[]
            {
                Token.Text($"{token.Value}Text {token.Value}").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyFormatting_WhenFormattingTokensIntersected(
            [ValueSource(nameof(GetOppositeTokens))]
            (Token, Token) pair)
        {
            var (token, opposite) = pair;
            var tokens = new[]
            {
                token,
                Token.Text("Text"),
                opposite,
                Token.Text("Text"),
                token,
                Token.Text("Text"),
                opposite
            };
            var expected = new[]
            {
                Token.Text($"{token.Value}Text{opposite.Value}Text{token.Value}Text{opposite.Value}").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldNotApplyFormatting_WhenEmptyText([ValueSource(nameof(GetFormattingTokens))] Token token)
        {
            var tokens = new[]
            {
                token,
                token
            };
            var expected = new[]
            {
                Token.Text($"{token.Value}{token.Value}").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldFormatHeader_WhenJustText()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("Text")
            };
            var expected = new[]
            {
                new TokenNode(Token.Header1, Token.Text("Text").ToNode())
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldFormatHeader_WhenEmptyText()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("")
            };
            var expected = new[]
            {
                Token.Header1.ToNode()
            };
            AssertParse(tokens, expected);
        }


        [Test]
        public void Parse_ShouldFormatHeader_WhenFormattingText()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("A "),
                Token.Cursive,
                Token.Text("B"),
                Token.Cursive
            };
            var expected = new[]
            {
                new TokenNode(Token.Header1, new[]
                {
                    Token.Text("A ").ToNode(),
                    new TokenNode(Token.Cursive, Token.Text("B").ToNode())
                })
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldFormatHeader_WhenTextAfterNewLine()
        {
            var tokens = new[]
            {
                Token.Header1,
                Token.Text("A"),
                Token.NewLine,
                Token.Text("B")
            };
            var expected = new[]
            {
                new TokenNode(Token.Header1, new[]
                {
                    Token.Text("A\n").ToNode()
                }),
                Token.Text("B").ToNode()
            };
            AssertParse(tokens, expected);
        }

        [Test]
        public void Parse_ShouldFlushFormatting_WhenNewLine(
            [ValueSource(nameof(GetFormattingTokens))]
            Token token)
        {
            var tokens = new[]
            {
                token,
                Token.Text("A"),
                Token.NewLine,
                Token.Text("B")
            };
            var expected = new[]
            {
                Token.Text($"{token.Value}A{Token.NewLine.Value}B").ToNode()
            };
            AssertParse(tokens, expected);
        }

        public static IEnumerable<(Token token, Token opposite)> GetOppositeTokens()
        {
            yield return (Token.Cursive, Token.Bold);
            yield return (Token.Bold, Token.Cursive);
        }

        public static IEnumerable<string> GetWordSeparators() => " !?.,;:\\|{}[]()*&^$".Select(x => x.ToString());


        public static IEnumerable<Token> GetFormattingTokens()
        {
            yield return Token.Cursive;
            yield return Token.Bold;
        }

        private void AssertParse(IEnumerable<Token> tokens, IEnumerable<TokenNode> expectedNodes)
        {
            var parsed = sut.Parse(tokens);
            parsed.Should().Equal(expectedNodes);
        }
    }
}