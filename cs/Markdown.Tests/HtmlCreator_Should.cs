using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HtmlCreator_Should
    {
        [Test]
        public void RenderNestedSequences()
        {
            new HtmlCreator().CreateFromTokens(new Token[]
                             {
                                 new PairedTagToken(0, 39,
                                                     "_this is __nested__ sequence of tokens_","_")
                                 {
                                     InnerTokens = new List<Token>
                                     {
                                         new PairedTagToken(9, 10, "__nested__","__")
                                     }
                                 }
                             })
                             .Should()
                             .BeEquivalentTo("<em>this is <strong>nested</strong> sequence of tokens</em>");
        }

        [Test]
        public void RenderSequences()
        {
            new HtmlCreator().CreateFromTokens(new Token[]
                             {
                                 new PairedTagToken(0, 5, "_hey_","_"),
                                 new StringToken(6, 5, " you!")
                             })
                             .Should()
                             .BeEquivalentTo("<em>hey</em> you!");
        }
    }
}
