using FluentAssertions;
using Markdown.SyntaxParser;
using Markdown.TokenFormatter;
using Markdown.Tokens;
using Markdown.Tokens.ConcreteTokens;
using NUnit.Framework;

namespace Markdown.Tests.TokensFormatter
{
    public class TokensFormatterTests
    {
        private HtmlTokensFormatter sut;

        [SetUp]
        public void SetUp()
        {
            sut = new HtmlTokensFormatter();
        }

        [TestCaseSource(typeof(TokensFormatterTestData), nameof(TokensFormatterTestData.TestTextData))]
        [TestCaseSource(typeof(TokensFormatterTestData), nameof(TokensFormatterTestData.TestBoldData))]
        [TestCaseSource(typeof(TokensFormatterTestData), nameof(TokensFormatterTestData.TestHeaderData))]
        [TestCaseSource(typeof(TokensFormatterTestData), nameof(TokensFormatterTestData.TestItalicsData))]
        public void Format_Should_FormatCorrectly_When(TokenTree[] parsedTokens, string expectedText)
        {
            var formattedText = sut.Format(parsedTokens);

            formattedText.Should().Be(expectedText);
        }

        [Test]
        public void Test()
        {
            var parsed = new TokenTree[]
            {
                new(new ImageToken("a", "abc"))
            };

            var expected = "<img src=\"a\" alt=\"abc\">";

            sut.Format(parsed).Should().Be(expected);
        }
    }
}