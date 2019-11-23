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
            var expectedResult = new FormattedToken(null, FormattedTokenType.Raw, 0, text.Length - 1);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnRawTokens_OnRawWithEscapes()
        {
            var text = @"abc \_def\_ghi";
            var expectedResult = new List<FormattedToken>()
            {
                new FormattedToken(null, FormattedTokenType.Raw, 0, "abc ".Length - 1),
                new FormattedToken(null, FormattedTokenType.Raw, @"abc \".Length, @"abc \_def".Length - 1),
                new FormattedToken(null, FormattedTokenType.Raw, @"abc \_def\".Length, text.Length - 1)
            };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneItalicToken_OnItalicWithEscapes()
        {
            var text = @"_abc \_def\_ghi_";
            var innerTokens = new List<FormattedToken>
            {
                new FormattedToken(null, FormattedTokenType.Raw, 1, "_abc ".Length - 1),
                new FormattedToken(null, FormattedTokenType.Raw, @"_abc \".Length, @"_abc \_def".Length - 1),
                new FormattedToken(null, FormattedTokenType.Raw, @"_abc \_def\".Length, text.Length - 2)
            };
            var expectedResult = new FormattedToken(innerTokens, FormattedTokenType.Italic, 1, text.Length - 2);

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneItalicToken_OnTextWithUnderscores()
        {
            var text = "_Just an italic text_";
            var innerRawToken = new FormattedToken(
                null, FormattedTokenType.Raw, 1, text.Length - 2);
            var expectedResult = new FormattedToken(
                new List<FormattedToken> { innerRawToken }, FormattedTokenType.Italic,
                innerRawToken.StartIndex, innerRawToken.EndIndex);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnThreeTokens_OnTextWithUnderscoresInside()
        {
            var text = "Just an _italic_ text";
            var firstToken = new FormattedToken(
                null, FormattedTokenType.Raw, 0, "Just an ".Length - 1);
            var secondInnerToken = new FormattedToken(
                null, FormattedTokenType.Raw, "Just an _".Length, "Just an _italic".Length - 1);
            var secondToken = new FormattedToken(
                new List<FormattedToken> { secondInnerToken }, FormattedTokenType.Italic,
                secondInnerToken.StartIndex, secondInnerToken.EndIndex);
            var thirdToken = new FormattedToken(
                null, FormattedTokenType.Raw, "Just an _italic_".Length, text.Length - 1);
            var expectedTokens = new List<FormattedToken>
                    {
                        firstToken, secondToken, thirdToken
                    };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ShouldReturnRawTokens_OnTextWithEscapedUnderscores()
        {
            var text = @"\_Just a text\_";
            var firstToken = new FormattedToken(
                null, FormattedTokenType.Raw, 1, @"\_Just a text".Length - 1);
            var secondToken = new FormattedToken(
                null, FormattedTokenType.Raw, @"\_Just a text\".Length, text.Length - 1);
            var expectedResult = new List<FormattedToken>()
            {
                firstToken,
                secondToken
            };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnRawTokens_OnTextWithEscapedAndEndingUnderscore()
        {
            var text = @"\_Just a text_";
            var expectedResult = new List<FormattedToken>
            {
                new FormattedToken(null, FormattedTokenType.Raw, 1, @"\_Just a text".Length - 1),
                new FormattedToken(null, FormattedTokenType.Raw, @"\_Just a text".Length, text.Length - 1)
            };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestCase("_Just a text", TestName = "One opening underscore in the beginning")]
        [TestCase("Just _a text", TestName = "One opening underscore inside")]
        [TestCase("Just a text_", TestName = "One ending underscore in the end")]
        [TestCase("Just a_ text", TestName = "One ending underscore inside")]
        [TestCase("Not_ an italic_", TestName = "Space after opening underscore")]
        [TestCase("__Just a text", TestName = "One opening double underscore in the beginning")]
        [TestCase("Just __a text", TestName = "One opening double underscore inside")]
        [TestCase("Just a text__", TestName = "One ending double underscore in the end")]
        [TestCase("Just a__ text", TestName = "One ending double underscore inside")]
        [TestCase("Not__ a bold__", TestName = "Space after opening double underscore")]
        [TestCase("Just a text_123_abc", TestName = "Underscores inside text")]
        public void GetTokens_ShouldReturnOnlyRawTokens_OnTextWithoutCorrectUnderscoring(string text)
        {
            var result = tokenizer.GetTokens(text).Select(t => t.Type).Any(t => t != FormattedTokenType.Raw);

            result.Should().BeFalse();
        }

        [TestCase("_Just a text", TestName = "One opening underscore in the beginning")]
        [TestCase("Just _a text", TestName = "One opening underscore inside")]
        [TestCase("Just a text_", TestName = "One ending underscore in the end")]
        [TestCase("Just a_ text", TestName = "One ending underscore inside")]
        [TestCase("Not_ an italic_", TestName = "Space after opening underscore")]
        [TestCase("__Just a text", TestName = "One opening double underscore in the beginning")]
        [TestCase("Just __a text", TestName = "One opening double underscore inside")]
        [TestCase("Just a text__", TestName = "One ending double underscore in the end")]
        [TestCase("Just a__ text", TestName = "One ending double underscore inside")]
        [TestCase("Not__ a bold__", TestName = "Space after opening double underscore")]
        [TestCase("Just a text_123_abc", TestName = "Underscores inside text")]
        public void GetToken_ShouldReturnTokenWithoutSpaces_OnTextWithoutCorrectUnderscoring(string text)
        {
            var result = tokenizer.GetTokens(text).ToList();
            result[0].StartIndex.Should().Be(0);
            result[result.Count - 1].EndIndex.Should().Be(text.Length - 1);
            for (var i = 0; i < result.Count - 1; i++)
            {
                var t1 = result[i];
                var t2 = result[i + 1];
                (t2.StartIndex - t1.EndIndex).Should().Be(1);
            }
        }

        [Test]
        public void GetTokens_ShouldReturnItalicTokenWithUnderscore_OnSpaceBeforeEndingUnderscore()
        {
            var text = "_Just an italic _ text_";
            var innerRawToken = new FormattedToken(null, FormattedTokenType.Raw, 
                1, text.Length - 2);
            var expectedResult = new FormattedToken(new List<FormattedToken> { innerRawToken }, FormattedTokenType.Italic,
                innerRawToken.StartIndex, innerRawToken.EndIndex);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnOneBoldToken_OnTextWithDoubleUnderscores()
        {
            var text = "__Just a bold text__";
            var innerRawToken = new FormattedToken(null, FormattedTokenType.Raw, 2, text.Length - 3);

            var expectedResult = new FormattedToken(new List<FormattedToken> { innerRawToken }, FormattedTokenType.Bold, 
                innerRawToken.StartIndex, innerRawToken.EndIndex);

            var result = tokenizer.GetTokens(text).ToList();

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnThreeTokens_OnTextWithDoubleUnderscoresInside()
        {
            var text = "Just a __bold__ text";
            var firstToken = new FormattedToken(null, FormattedTokenType.Raw, 
                0, "Just a ".Length - 1);
            var secondInnerToken = new FormattedToken(null, FormattedTokenType.Raw, 
                "Just a __".Length, "Just a __bold".Length - 1);
            var secondToken = new FormattedToken(new List<FormattedToken> { secondInnerToken }, FormattedTokenType.Bold, 
                secondInnerToken.StartIndex, secondInnerToken.EndIndex);
            var thirdToken = new FormattedToken(null, FormattedTokenType.Raw, 
                "Just a __bold__".Length, text.Length - 1);
            var expectedTokens = new List<FormattedToken>
            {
                firstToken, secondToken, thirdToken
            };

            var result = tokenizer.GetTokens(text);

            result.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ShouldReturnBoldTokenWithDoubleUnderscore_WhenSpaceBeforeEndingDoubleUnderscore()
        {
            var text = "__Just a bold __ text__";
            var innerRawToken = new FormattedToken(null, FormattedTokenType.Raw,
                2, text.Length - 3);

            var expectedResult = new FormattedToken(new List<FormattedToken> { innerRawToken },
                FormattedTokenType.Bold, innerRawToken.StartIndex, innerRawToken.EndIndex);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }

        [Test]
        public void GetTokens_ShouldReturnCorrectTokens_OnItalicWithEscapedEscape()
        {
            var text = @"_\\_";
            var innerRawToken = new FormattedToken(null, FormattedTokenType.Raw, 2, 2);
            var expectedResult = new FormattedToken(new List<FormattedToken> { innerRawToken }, FormattedTokenType.Italic,
                innerRawToken.StartIndex, innerRawToken.EndIndex);

            var result = tokenizer.GetTokens(text);

            result.Should().HaveCount(1).And.AllBeEquivalentTo(expectedResult);
        }
    }
}