using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md parser;

        [SetUp]
        public void SetUp()
        {
            parser = new Md(new TextParser(new []{new UnderscoreRule()}),new HtmlCreator());   
        }

        [TestCase("_hey_ you!", "<em>hey</em> you!", TestName = "text has one paired underscore tag")]
        [TestCase("hey you!", "hey you!", TestName = "text has no tags")]
        public void ParseTextCorrectly_When(string text, string renderedText)
        {
            parser.Render(text)
                  .Should()
                  .BeEquivalentTo(renderedText);
        }
    }
}