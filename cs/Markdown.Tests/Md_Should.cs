using System;
using System.Text;
using FluentAssertions;
using FluentAssertions.Specialized;
using NUnit.Framework;

namespace Markdown.Tests
{
    using System.Collections.Generic;

    [TestFixture]
    public class Md_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new Md(
                            new HtmlCreator(
                                            new Dictionary<string, (string opening, string closing)>
                                                {
                                                    ["_"] = ("<em>", "</em>"),
                                                        ["*"] = ("<em>", "</em>"),
                                                        ["__"] = ("<strong>", "</strong>"),
                                                        ["**"] = ("<strong>", "</strong>")
                                                    }));
        }

        private Md parser;

        [TestCase("_hey_ you!", "<em>hey</em> you!", TestName = "text has one paired underscore tag")]
        [TestCase("hey you!", "hey you!", TestName = "text has no tags")]
        [TestCase("_abacaba_ __caba _dab_ a__", "<em>abacaba</em> <strong>caba <em>dab</em> a</strong>", TestName =
            "text has underscore tags and nested  underscore tags")]
        [TestCase("_a_ _b __cd _b dc__", "<em>a</em> _b <strong>cd _b dc</strong>", TestName =
            "text has non paired tags")]
        [TestCase("*hey* you!", "<em>hey</em> you!", TestName = "text has one paired asterisk tag")]
        [TestCase("*abacaba* **caba *dab* a**", "<em>abacaba</em> <strong>caba <em>dab</em> a</strong>", TestName =
            "text has asterisk tags and nested asterisk tags")]
        [TestCase("*a* *b **cd *b dc**", "<em>a</em> *b <strong>cd *b dc</strong>", TestName =
            "text has non paired asterisk tags")]
        [TestCase("_a_ *b **cd _b dc**", "<em>a</em> *b <strong>cd _b dc</strong>", TestName =
            "text has mixed tags")]
        public void ParseTextCorrectly_When(string text, string renderedText)
        {
            parser.Render(text)
                  .Should()
                  .BeEquivalentTo(renderedText);
        }

        [TestCase("asdwarhyeuit  truehgfoneoghe432534___3*&&&&3254&&&882143",TestName = "random stuff #1")]
        [TestCase("____________________",TestName = "many underscores")]
        [TestCase("_*_*_*_*_*_",TestName = "underscores followed by asterisks")]
        [TestCase("_aa_www_wre____ewe____wewtr_rr",TestName = "random stuff #2")]
        [TestCase("",TestName = "")]
        public void NotThrow_WhenTrashIsGiven(string text)
        {
            Action parsing = () => parser.Render(text);
            parsing.ShouldNotThrow();
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
                var isEvenStep = i % 2 == 0;
                var tag = isEvenStep ? "_" : "__";
                textBuilder.Append($" {tag}ab{i}cd{tag} ");
                var htmlTag = isEvenStep ? "em" : "strong";
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
