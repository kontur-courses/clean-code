using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    internal interface IHtmlCreator
    {
        string CreateFromTokens(IEnumerable<Token> tokens);
    }

    class HtmlCreator : IHtmlCreator
    {
        public string CreateFromTokens(IEnumerable<Token> tokens)
        {
            return string.Join( "", tokens.Select(token => token.ToHtml()));
        }
    }

    [TestFixture]
    public class HtmlCreator_Should
    {
        [Test]
        public void Render_When()
        {
            new HtmlCreator().CreateFromTokens(new Token[]
            {
                new UnderscoreToken(0, 5, "_die_"),
                new StringToken(6, 5, " you!")
            }).Should().BeEquivalentTo("<em>die</em> you!");
        }
    }
}