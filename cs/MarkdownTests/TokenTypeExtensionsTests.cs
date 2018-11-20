using System.Collections.Generic;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    class TokenTypeExtensionsTests
    {
        [TestCase("f _d", 2, ExpectedResult = true, TestName = "After whitespace")]
        [TestCase("_d", 0, ExpectedResult = true, TestName = "In paragraph beginning")]
        [TestCase("f_d", 1, ExpectedResult = true, TestName = "Without whitespace before")]
        [TestCase("d \\_d", 4, ExpectedResult = false, TestName = "Is not valid after backslash")]
        public bool ValidOpeningPositionTest(string text, int openingPosition)
        {
            var token = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);

            return token.ValidOpeningPosition(text, openingPosition);
        }

        [TestCase("d_ d", 1, ExpectedResult = true, TestName = "Before whitespace")]
        [TestCase("d_", 1, ExpectedResult = true, TestName = "In paragraph ending")]
        [TestCase("d_f", 1, ExpectedResult = true, TestName = "Without whitespace after")]
        [TestCase("dd\\_ d", 4, ExpectedResult = false, TestName = "Is not valid after backslash")]
        public bool ValidClosingPositionTest(string text, int closingPosition)
        {
            var token = new TokenType("singleUnderscore", "_", "em", TokenLocationType.InlineToken);

            return token.ValidClosingPosition(text, closingPosition);
        }

        [TestCase("__f", 0, ExpectedResult = "doubleUnderscore", TestName = "Find double underscore")]
        [TestCase("_f", 0, ExpectedResult = "simpleUnderscore", TestName = "Find simple underscore")]
        public string GetOpeningTokenTest(string text, int startIndex)
        {
            var tokens = new List<TokenType>
            {
                new TokenType("doubleUnderscore", "__", "strong",TokenLocationType.InlineToken),
                new TokenType("simpleUnderscore", "_", "em",TokenLocationType.InlineToken)
            };

            return tokens.GetOpeningToken(text, startIndex).Name;
        }

        [TestCase("f__", 1, ExpectedResult = "doubleUnderscore", TestName = "Find double underscore")]
        [TestCase("f_", 1, ExpectedResult = "simpleUnderscore", TestName = "Find simple underscore")]
        public string GetClosingTokenTest(string text, int startIndex)
        {
            var tokens = new List<TokenType>
            {
                new TokenType("doubleUnderscore", "__", "strong", TokenLocationType.InlineToken),
                new TokenType("simpleUnderscore", "_", "em", TokenLocationType.InlineToken)
            };

            return tokens.GetClosingToken(text, startIndex).Name;
        }
    }
}
