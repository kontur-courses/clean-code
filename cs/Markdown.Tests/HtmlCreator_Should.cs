namespace Markdown.Tests
{
    using System.Collections.Generic;

    using FluentAssertions;

    using NUnit.Framework;

    [TestFixture]
    public class HtmlCreator_Should
    {
        private HtmlCreator creator;

        [Test]
        public void RenderNestedSequences()
        {
            var innerTokens = new List<Token>
                                  {
                                      new PairedTagToken(9, 10, "__nested__", "__")
                                  };
            creator.CreateFromTokens(
                                     new Token[]
                                         {
                                             new PairedTagToken(0, 39, "_this is __nested__ sequence of tokens_", "_")
                                                 {
                                                     InnerTokens = innerTokens
                                                 }
                                         })
                   .Should()
                   .BeEquivalentTo("<em>this is <strong>nested</strong> sequence of tokens</em>");
        }

        [Test]
        public void RenderSequences()
        {
            creator.CreateFromTokens(
                                     new Token[]
                                         {
                                             new PairedTagToken(0, 5, "_hey_", "_"), new StringToken(6, 5, " you!")
                                         })
                   .Should()
                   .BeEquivalentTo("<em>hey</em> you!");
        }

        [SetUp]
        public void SetUp()
        {
            creator = new HtmlCreator(
                                      new Dictionary<string, (string opening, string closing)>
                                          {
                                              ["_"] = ("<em>", "</em>"),
                                              ["*"] = ("<em>", "</em>"),
                                              ["__"] = ("<strong>", "</strong>"),
                                              ["**"] = ("<strong>", "</strong>")
                                          });
        }
    }
}
