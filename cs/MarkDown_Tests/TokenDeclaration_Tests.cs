using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MarkDown.TokenParsers;
using NUnit.Framework;

namespace MarkDown_Tests
{
    class TokenParser_Tests
    {
        private static EMParser Parser;
        [SetUp]
        public void SetUp()
        {
            Parser = new EMParser();
        }

        [Test]
        public void CommonToken()
        {
            var line = "_a_";

            var result = Parser.GetToken(line, 0).Value;

            result.Should().Be(@"<em>a</em>");

        }
        [Test]
        public void ShildedToken()
        {
            var line = "/_a_";

            var result = Parser.GetToken(line, 0);

            result.Should().Be(null);

        }
        [Test]
        public void NotPairedTokenToken()
        {
            var line = "__a_";

            var result = Parser.GetToken(line, 0);

            result.Should().Be(null);

        }
    }
}
