using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class TokenTypeExtensionsTests
    {
        private readonly TokenType singleUnderscore =
            new TokenType(TokenTypeEnum.SingleUnderscore, "_", "em", TokenLocationType.InlineToken);
        private readonly TokenType doubleUnderscore =
            new TokenType(TokenTypeEnum.DoubleUnderscore, "__", "strong", TokenLocationType.InlineToken);

        [TestCase("f _d", 2, ExpectedResult = true, TestName = "After whitespace")]
        [TestCase("_d", 0, ExpectedResult = true, TestName = "In paragraph beginning")]
        [TestCase("f_d", 1, ExpectedResult = true, TestName = "Without whitespace before")]
        [TestCase("d \\_d", 4, ExpectedResult = false, TestName = "Is not valid after backslash")]
        public bool ValidOpeningPositionTest(string text, int openingPosition)
        {
            return singleUnderscore.ValidOpeningPosition(text, openingPosition);
        }

        [TestCase("d_ d", 1, ExpectedResult = true, TestName = "Before whitespace")]
        [TestCase("d_", 1, ExpectedResult = true, TestName = "In paragraph ending")]
        [TestCase("d_f", 1, ExpectedResult = true, TestName = "Without whitespace after")]
        [TestCase("dd\\_ d", 4, ExpectedResult = false, TestName = "Is not valid after backslash")]
        public bool ValidClosingPositionTest(string text, int closingPosition)
        {
            return singleUnderscore.ValidClosingPosition(text, closingPosition);
        }

        [TestCase("__f", 0, ExpectedResult = "DoubleUnderscore", TestName = "Find double underscore")]
        [TestCase("_f", 0, ExpectedResult = "SingleUnderscore", TestName = "Find single underscore")]
        public string GetOpeningTokenTest(string text, int startIndex)
        {
            var tokens = new List<TokenType>
            {
                doubleUnderscore,
                singleUnderscore
            };

            return tokens.GetOpeningToken(text, startIndex).Name.ToString();
        }

        [TestCase("f__", 1, ExpectedResult = "DoubleUnderscore", TestName = "Find double underscore")]
        [TestCase("f_", 1, ExpectedResult = "SingleUnderscore", TestName = "Find single underscore")]
        public string GetClosingTokenTest(string text, int startIndex)
        {
            var tokens = new List<TokenType>
            {
                doubleUnderscore,
                singleUnderscore
            };

            return tokens.GetClosingToken(text, startIndex).Name.ToString();
        }
    }
}
