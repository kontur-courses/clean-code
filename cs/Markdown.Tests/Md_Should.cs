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
        [TestCase("_abacaba_ __caba _dab_ a__", "<em>abacaba</em> <strong>caba <em>dab</em> a</strong>", TestName = "text has tags and nested tags")]
        [TestCase("_a_ _b __cd _b dc__", "<em>a</em> _b <strong>cd _b dc</strong>", TestName = "text has non paired tags")]
        public void ParseTextCorrectly_When(string text, string renderedText)
        {
            parser.Render(text)
                  .Should()
                  .BeEquivalentTo(renderedText);
        }
    }
}