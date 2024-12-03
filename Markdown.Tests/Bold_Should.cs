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
    public class Bold_Should
    {
        private readonly Bold _bold = new();

        [TestCaseSource(nameof(TestCases))]
        public void ReturnsRightToken((string text, Token expectedToken) td)
        {
            var returnedToken = _bold.TryFindToken(td.text, 0);

            returnedToken.Should().BeEquivalentTo(td.expectedToken);
        }

        public static IEnumerable<(string, Token)> TestCases()
        {
            yield return ("__", new Token("__", TokenProperty.Normal));
            yield return ("__a", new Token("__a", TokenProperty.Normal));
            yield return ("__aa", new Token("__aa", TokenProperty.Normal));
            yield return ("____", new Token("____", TokenProperty.Normal));
            yield return (@"__a\__b\__c", new Token("__a__b__c", TokenProperty.Normal));
            yield return ("__ a__", new Token("__", TokenProperty.Normal));
            yield return ("__a a__", new Token("a a", TokenProperty.Bold));
            yield return ("__a __a", new Token("__a __a", TokenProperty.Normal));
            yield return (@"__a\__a\__ __", new Token("__a__a__ __", TokenProperty.Normal));
        }
    }
}
