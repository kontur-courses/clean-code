using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MarkDown;
using MarkDown.TokenParsers;
using NUnit.Framework;

namespace MarkDown_Tests
{
    class TokenParser_Tests
    {
        protected static EMParser emParser { get; private set; }
        protected static StrongParser strongParser { get; private set; }

        [SetUp]
        public void SetUp()
        {
            emParser = new EMParser();
            strongParser = new StrongParser();
        }

        [TestCase("_a_", @"<em>a</em>",TokenType.EM)]
        [TestCase("__a__", @"<strong>a</strong>", TokenType.Strong)]
        public void GetToken_ReturnParsedToken_CommonToken(string line, string expectedResult, TokenType tokenType)
        {
            var result= tokenType == TokenType.EM? emParser.GetToken(line, 0).Value
            : strongParser.GetToken(line, 0).Value;

            result.Should().Be(expectedResult);

        }
        [TestCase("/__a__", TokenType.EM)]
        [TestCase("/_a_", TokenType.Strong)]
        public void GetToken_ReturnNull_ShieldedToken(string line, TokenType tokenType)
        {
            var result = tokenType == TokenType.EM ? emParser.GetToken(line, 0)
                : strongParser.GetToken(line, 0);

            result.Should().Be(null);

        }
        [TestCase("__a_", TokenType.EM)]
        [TestCase("_a__", TokenType.Strong)]
        public void GetToken_ReturnNull_OnNotPairedTokenToken(string line,TokenType tokenType)
        {
            var result = tokenType == TokenType.EM ? emParser.GetToken(line, 0)
                : strongParser.GetToken(line, 0);

            result.Should().Be(null);

        }
    }
}
