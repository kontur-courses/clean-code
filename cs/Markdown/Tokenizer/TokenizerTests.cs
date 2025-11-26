using NUnit.Framework;
using FluentAssertions;
using System;
using System.Linq;
using Markdown;
using System.Collections.Generic;

namespace Markdown.Tests
{
    [TestFixture]
    public class TokenizerTests
    {
        private Tokenizer tokenizer;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tokenizer = new Tokenizer();
        }

        [Test]
        public void TextToTokens_WithEmptyInput_ReturnsEmptyList()
        {
            var tokens = tokenizer.TextToTokens("");
            tokens.Should().BeEmpty();
        }

        [Test]
        public void TextToTokens_WithPlainText_ReturnsSingleTextToken()
        {
            var tokens = tokenizer.TextToTokens("простойтекст");
            tokens.Should().HaveCount(1);
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("простойтекст");
        }

        [Test]
        public void TextToTokens_WithOnlySpaces_ReturnsSingleSpaceToken()
        {
            var tokens = tokenizer.TextToTokens("   ");
            tokens.Should().HaveCount(1);
            tokens[0].Type.Should().Be(TokenType.Space);
            tokens[0].Value.Should().Be("   ");
        }

        [Test]
        public void TextToTokens_WithOnlyNewLines_ReturnsMultipleNewLineTokens()
        {
            var tokens = tokenizer.TextToTokens("\n\n\n");
            tokens.Should().HaveCount(3);
            tokens.All(t => t.Type == TokenType.NextLine).Should().BeTrue();
        }

        [Test]
        public void TextToTokens_WithMultipleSpaces_ReturnsSingleSpaceTokenWithMultipleSpaces()
        {
            var tokens = tokenizer.TextToTokens("много   пробелов");
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[1].Type.Should().Be(TokenType.Space);
            tokens[1].Value.Should().Be("   ");
            tokens[2].Type.Should().Be(TokenType.Text);
            tokens[2].Value.Should().Be("пробелов");
        }

        [Test]
        public void TextToTokens_WithNewLine_ReturnsCorrectTokenSequence()
        {
            var tokens = tokenizer.TextToTokens("строка1\nстрока2");
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[1].Type.Should().Be(TokenType.NextLine);
            tokens[2].Type.Should().Be(TokenType.Text);
        }

        [Test]
        public void TextToTokens_WithSingleEmphasisAtStart_ReturnsEmphasisThenText()
        {
            var tokens = tokenizer.TextToTokens("_курсив");
            tokens[0].Type.Should().Be(TokenType.Emphasis);
            tokens[1].Type.Should().Be(TokenType.Text);
            tokens[1].Value.Should().Be("курсив");
        }

        [Test]
        public void TextToTokens_WithSingleEmphasisAtEnd_ReturnsTextThenEmphasis()
        {
            var tokens = tokenizer.TextToTokens("курсив_");
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("курсив");
            tokens[1].Type.Should().Be(TokenType.Emphasis);
        }

        [Test]
        public void TextToTokens_WithSingleEmphasisBetweenText_ReturnsCorrectTokenSequence()
        {
            var tokens = tokenizer.TextToTokens("текст _курсив_ текст");

            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("текст");
            tokens[1].Type.Should().Be(TokenType.Space);
            tokens[2].Type.Should().Be(TokenType.Emphasis);
            tokens[3].Type.Should().Be(TokenType.Text);
            tokens[3].Value.Should().Be("курсив");
            tokens[4].Type.Should().Be(TokenType.Emphasis);
            tokens[5].Type.Should().Be(TokenType.Space);
            tokens[6].Type.Should().Be(TokenType.Text);
            tokens[6].Value.Should().Be("текст");
        }

        [Test]
        public void TextToTokens_WithSingleStrongAtStart_ReturnsStrongThenText()
        {
            var tokens = tokenizer.TextToTokens("__жирный");
            tokens[0].Type.Should().Be(TokenType.Strong);
            tokens[1].Type.Should().Be(TokenType.Text);
            tokens[1].Value.Should().Be("жирный");
        }

        [Test]
        public void TextToTokens_WithSingleStrongAtEnd_ReturnsTextThenStrong()
        {
            var tokens = tokenizer.TextToTokens("жирный__");
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("жирный");
            tokens[1].Type.Should().Be(TokenType.Strong);
        }

        [Test]
        public void TextToTokens_WithSingleStrongBetweenText_ReturnsCorrectTokenSequence()
        {
            var tokens = tokenizer.TextToTokens("текст __жирный__ текст");

            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[1].Type.Should().Be(TokenType.Space);
            tokens[2].Type.Should().Be(TokenType.Strong);
            tokens[3].Type.Should().Be(TokenType.Text);
            tokens[3].Value.Should().Be("жирный");
            tokens[4].Type.Should().Be(TokenType.Strong);
            tokens[5].Type.Should().Be(TokenType.Space);
            tokens[6].Type.Should().Be(TokenType.Text);
            tokens[6].Value.Should().Be("текст");
        }

        [Test]
        public void TextToTokens_WithHeaderAtStartOfLine_ReturnsHeaderToken()
        {
            var tokens = tokenizer.TextToTokens("# заголовок");
            tokens[0].Type.Should().Be(TokenType.Header);
            tokens[1].Type.Should().Be(TokenType.Space);
            tokens[2].Type.Should().Be(TokenType.Text);
            tokens[2].Value.Should().Be("заголовок");
        }

        [Test]
        public void TextToTokens_WithHeaderWithoutSpace_ReturnsTextToken()
        {
            var tokens = tokenizer.TextToTokens("#Заголовок");
            tokens.Should().NotContain(t => t.Type == TokenType.Header && t.Position == 0);
        }

        [Test]
        public void TextToTokens_WithEscapedUnderscore_ReturnsTextTokenWithUnderscores()
        {
            var tokens = tokenizer.TextToTokens(@"\_текст\_");
            tokens.Should().HaveCount(1);
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("_текст_");
        }

        [Test]
        public void TextToTokens_WithFourUnderscores_ReturnsSingleTextToken()
        {
            var tokens = tokenizer.TextToTokens("____");
            tokens.Should().HaveCount(1);
            tokens[0].Type.Should().Be(TokenType.Text);
            tokens[0].Value.Should().Be("____");
        }

        [Test]
        public void TextToTokens_WithMixedFormatting_ReturnsCorrectTokenSequence()
        {
            var tokens = tokenizer.TextToTokens("Обычный _курсив_ и __жирный__ текст");

            tokens.Should().Equal(
                new Token(TokenType.Text, "Обычный", 0),
                new Token(TokenType.Space, " ", 7),
                new Token(TokenType.Emphasis, "_", 8),
                new Token(TokenType.Text, "курсив", 9),
                new Token(TokenType.Emphasis, "_", 15),
                new Token(TokenType.Space, " ", 16),
                new Token(TokenType.Text, "и", 17),
                new Token(TokenType.Space, " ", 18),
                new Token(TokenType.Strong, "__", 19),
                new Token(TokenType.Text, "жирный", 21),
                new Token(TokenType.Strong, "__", 27),
                new Token(TokenType.Space, " ", 29),
                new Token(TokenType.Text, "текст", 30)
            );
        }
    }
}