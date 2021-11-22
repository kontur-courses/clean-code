using FluentAssertions;
using Markdown.SyntaxParser;
using Markdown.TokenFormatter;
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
    }
}