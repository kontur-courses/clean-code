using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class MarkdownTextTokenizerTests
    {
        private MarkdownTextTokenizer tokenizer;

        [SetUp]
        public void SetUp()
        {
            tokenizer = new MarkdownTextTokenizer();
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnTextWithoutSymbols()
        {
            var text = "Just a text";
            var expectedResult = new Token(text, TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneItalicToken_OnTextWithUnderscores()
        {
            var text = "_Just an italic text_";
            var content = "Just an italic text";
            var expectedResult = new Token(content, TokenType.Italic);

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnThreeTokens_OnTextWithUnderscoresInside()
        {
            var text = "Just an _italic_ text";
            var expectedTokens = new List<Token>
            {
                new Token("Just an ", TokenType.Raw),
                new Token("italic", TokenType.Italic),
                new Token(" text", TokenType.Raw)
            };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnTextWithEscapedUnderscores()
        {
            var text = @"\_Just a text\_";
            var expectedResult = new Token("_Just a text_", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnTextWithEscapedAndEndingUnderscore()
        {
            var text = @"\_Just a text_";
            var expectedResult = new Token("_Just a text_", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnOneOpeningUnderscore()
        {
            var text = "_Just a text";
            var expectedResult = new Token("_Just a text", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawTokens_OnOneOpeningUnderscoreInside()
        {
            var text = "Just _a text";
            var expectedResult = new Token("Just _a text", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnOneEndingUnderscore()
        {
            var text = "Just a text_";
            var expectedResult = new Token("Just a text_", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnOneEndingUnderscoreInside()
        {
            var text = "Just a_ text";
            var expectedResult = new Token("Just a_ text", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }


        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnSpaceAfterOpeningUnderscore()
        {
            var text = "Not_ an italic_";
            var expectedResult = new Token("Not_ an italic_", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnItalicTokenWithUnderscore_OnSpaceBeforeEndingUnderscore()
        {
            var text = "_Just an italic _ text_";
            var expectedResult = new Token("Just an italic _ text", TokenType.Italic);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneRawToken_OnUnderscoreInsideText()
        {
            var text = "Just a text_123_abc";
            var expectedResult = new Token("Just a text_123_abc", TokenType.Raw);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }
    }
}