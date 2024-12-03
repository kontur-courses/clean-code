using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.MDParser;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Italic_Should
    {
        private readonly Italic _italic = new();

        [TestCaseSource(nameof(TestCases))]
        public void ReturnsRightToken((string text, Token expectedToken) td)
        {
            var returnedToken = _italic.TryFindToken(td.text, 0);

            returnedToken.Should().BeEquivalentTo(td.expectedToken);
        }

        public static IEnumerable<(string, Token)> TestCases()
        {
            yield return ("_", new Token("_", TokenProperty.Normal));
            yield return ("__a__", new Token("a", TokenProperty.Bold));
            yield return ("_a a_", new Token("_a", TokenProperty.Normal));
            yield return ("_a1a_", new Token("_a1a", TokenProperty.Normal));
            yield return ("_abc", new Token("_abc", TokenProperty.Normal));
            yield return ("_a_", new Token("a", TokenProperty.Italic));
            yield return ("_a\\_", new Token("_a", TokenProperty.Normal));
        }
    }
}
