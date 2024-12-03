using Markdown.MDParser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    public class Link_Should
    {
        private readonly Link _link = new();

        [TestCaseSource(nameof(TestCases))]
        public void ReturnsRightToken((string text, Token expectedToken) td)
        {
            var returnedToken = _link.TryFindToken(td.text, 0);

            returnedToken.Should().BeEquivalentTo(td.expectedToken);
        }

        public static IEnumerable<(string, Token)> TestCases()
        {
            yield return ("[name](address)]", new Token("name"+"+++"+"address", TokenProperty.Link));
            yield return (@"[name\](address)]", new Token("[", TokenProperty.Normal));
            yield return (@"[name](address\)]", new Token("[", TokenProperty.Normal));
            yield return (@"[name", new Token("[", TokenProperty.Normal));
            yield return (@"[name]", new Token("[", TokenProperty.Normal));
            yield return (@"[name]add", new Token("[", TokenProperty.Normal));
            yield return (@"[name](", new Token("[", TokenProperty.Normal));
            yield return (@"[name](address", new Token("[", TokenProperty.Normal));
        }
    }
}
