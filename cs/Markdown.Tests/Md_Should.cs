using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Specialized;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new Md(new HtmlCreator());
        }

        private Md parser;

        [TestCase("_hey_ you!", "<em>hey</em> you!", TestName = "text has one paired underscore tag")]
        [TestCase("hey you!", "hey you!", TestName = "text has no tags")]
        [TestCase("_abacaba_ __caba _dab_ a__", "<em>abacaba</em> <strong>caba <em>dab</em> a</strong>", TestName =
            "text has tags and nested tags")]
        [TestCase("_a_ _b __cd _b dc__", "<em>a</em> _b <strong>cd _b dc</strong>", TestName =
            "text has non paired tags")]
        public void ParseTextCorrectly_When(string text, string renderedText)
        {
            parser.Render(text)
                  .Should()
                  .BeEquivalentTo(renderedText);
        }

        [TestCase(100, 0, 50)]
        [TestCase(1000, 0, 50)]
        [TestCase(10000, 0, 200)]
        [TestCase(100000, 2, 0)]
        public void BeFast(int amount, int seconds, int ms)
        {
            var textBuilder = new StringBuilder();
            var renderBuilder = new StringBuilder();
            for (var i = 0; i < amount; i++)
            {
                var tag = i % 2 == 0 ? "_" : "__";
                textBuilder.Append($" {tag}ab{i}cd{tag} ");
                var htmlTag = i % 2 == 0 ? "em" : "strong";
                renderBuilder.Append($" <{htmlTag}>ab{i}cd</{htmlTag}> ");
            }

            var text = textBuilder.ToString();
            var render = renderBuilder.ToString();

            void Rendering() => parser.Render(text);

            new ExecutionTimeAssertions(Rendering).ShouldNotExceed(new TimeSpan(0,0,seconds,ms));
            parser.Render(text).ShouldBeEquivalentTo(render);

        }
    }
}
