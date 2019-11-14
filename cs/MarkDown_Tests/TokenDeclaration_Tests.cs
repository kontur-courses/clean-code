using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MarkDown.TokenDeclarations;
using NUnit.Framework;

namespace MarkDown_Tests
{
    class TokenDeclaration_Tests
    {
        private static EMDeclaration declaration;
        [SetUp]
        public void SetUp()
        {
            declaration = new EMDeclaration();
        }

        [Test]
        public void CommonToken()
        {
            var line = "_a_";

            var result = declaration.GetToken(line, 0).Value;

            result.Should().Be(@"<em>a</em>");

        }
        [Test]
        public void ShildedToken()
        {
            var line = "/_a_";

            var result = declaration.GetToken(line, 0);

            result.Should().Be(null);

        }
        [Test]
        public void NotPairedTokenToken()
        {
            var line = "__a_";

            var result = declaration.GetToken(line, 0);

            result.Should().Be(null);

        }
    }
}
