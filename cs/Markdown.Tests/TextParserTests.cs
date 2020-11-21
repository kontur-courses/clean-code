using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TextParserTests
    {
        private TextParser Parser { get; set; }

        [SetUp]
        public void SetUp()
        {
            Parser = new TextParser();
        }

        [TestCase("_e __s__ e_", 1, TestName = "Strong tag in emphasized")]
        [TestCase(@"te\xt", 1, TestName = "Not escaped backslash")]
        [TestCase(@"_e\\_", 1, TestName = "Escaped backslash before ending tag")]
        [TestCase(@"\_", 1, TestName = "Tag after backslash")]
        [TestCase(@"_E __s e_ S__", 5, TestName = "Emphasized tag intersect strong tag")]
        [TestCase("__S _e s__ E_", 5, TestName = "Strong tag intersect emphasized tag")]
        [TestCase("text __in tag__", 2, TestName = "Plain text at start")]
        [TestCase("__s s _e_ s__", 1, TestName = "Emphasized tag in strong")]
        [TestCase("__let__ __me__ __in__", 5, TestName = "More than one token")]
        [TestCase("_in tag_ text", 2, TestName = "Plain text at the end")]
        public void GetTokens_ReturnExpectedTokenCount_When(string text, int expectedTokensCount)
        {
            var result = Parser.GetTokens(text, text);

            result.Count().Should().Be(expectedTokensCount);
        }
    }
}