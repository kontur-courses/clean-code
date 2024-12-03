using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Markdown.MDParser;

namespace Markdown.Tests
{
    [TestFixture]
    public class Hidden_Should
    {
        [TestCaseSource(nameof(TestCases))]
        public void ReturnsRightToken((string text, Token expectedToken) td)
        {
            var returnedToken =  new Hidden().TryFindToken(td.text, 0);

            returnedToken.Should().BeEquivalentTo(td.expectedToken);
        }

        public static IEnumerable<(string, Token)> TestCases()
        {
            yield return (@"\", new Token( @"\", TokenProperty.Normal));
            yield return (@"\\", new Token( @"\", TokenProperty.Normal));
            yield return (@"\_", new Token( @"_", TokenProperty.Normal));
            yield return (@"\abra", new Token("", TokenProperty.Normal));
        }
    }
}
