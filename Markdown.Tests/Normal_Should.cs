using FluentAssertions;
using Markdown.MDParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Tests
{
    [TestFixture]
    public class Normal_Should
    {
        private static IEnumerable<string> NormalTextStrings()
        {
            return Markers.Chars.Select(marker => "text" + marker);
        }

        [TestCaseSource(nameof(TestCases))]
        public void ReturnsRightToken((string text, Token expectedToken) td)
        {
            var returnedToken = new Normal().TryFindToken(td.text, 0);

            returnedToken.Should().BeEquivalentTo(td.expectedToken);
        }

        public static IEnumerable<(string, Token)> TestCases()
        {
            return NormalTextStrings().Select(str => (str, new Token(str[..^1], TokenProperty.Normal)));
        }
    }
}
