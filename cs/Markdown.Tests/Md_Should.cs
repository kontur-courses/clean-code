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

        [Test]
        public void FAST()
        {
            var s = new StringBuilder();
            var s2 = new StringBuilder();
            for (int i = 0; i < 100000; i++)
            {
                var t = i % 2 == 0 ? "_" : "__";
                s.Append($" {t}ab{i}cd{t} ");
                var t2 = i % 2 == 0 ? "em" : "strong";
                s2.Append($" <{t2}>ab{i}cd</{t2}> ");
            }

            var text = s.ToString();
            var render = s2.ToString();

            void Rendering() => parser.Render(text);

            new ExecutionTimeAssertions(Rendering).ShouldNotExceed(new TimeSpan(0,0,0,2));
            parser.Render(text).ShouldBeEquivalentTo(render);

        }
    }
}
