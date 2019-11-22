using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Markdown.Core;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class MdTests
    {
        [TestCase("_a b_", ExpectedResult = "<em>a b</em>", TestName = "When_AtBeginningAndEndText")]
        [TestCase("a _b c_ d", ExpectedResult = "a <em>b c</em> d", TestName = "When_InMiddleText")]
        [TestCase("_a _b c_ d_", ExpectedResult = "<em>a <em>b c</em> d</em>",
            TestName = "When_SeveralTimesInText")]
        public string Render_ReplacedEm(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase("__a b__", ExpectedResult = "<strong>a b</strong>", TestName = "When_AtBeginningAndEndText")]
        [TestCase("a __b c__ d", ExpectedResult = "a <strong>b c</strong> d", TestName = "When_InMiddleText")]
        [TestCase("__a __b c__ d__", ExpectedResult = "<strong>a <strong>b c</strong> d</strong>",
            TestName = "When_SeveralTimesInText")]
        public string Render_ReplacedStrong(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase("_a __b__ c_", ExpectedResult = "<em>a <strong>b</strong> c</em>",
            TestName = "WhenStrongEmbeddedInEm")]
        [TestCase("__a _b_ c__", ExpectedResult = "<strong>a <em>b</em> c</strong>",
            TestName = "WhenEmEmbeddedInStrong")]
        [TestCase("# ab\n_c_", ExpectedResult = "<h1> ab</h1>\n<em>c</em>",
            TestName = "WhenFirstLineHeaderAndEmInSecond")]
        public string Render_ReplacedSeveralTags(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase(@"\_a\_", ExpectedResult = "_a_", TestName = "EscapedTagAtStartLine")]
        [TestCase(@"a \__b\__ c", ExpectedResult = "a __b__ c", TestName = "EscapedTagAtMiddleLine")]
        [TestCase(@"\# blabla", ExpectedResult = "# blabla", TestName = "EscapedHeader")]
        public string Render_DontReplaceEscapedTags(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase("`foo bar`", ExpectedResult = "<code>foo bar</code>", TestName = "WhenCodeTag")]
        [TestCase("*Просто пример курсива*", ExpectedResult = "<em>Просто пример курсива</em>",
            TestName = "WhenStarTag")]
        [TestCase("**Пример strong**", ExpectedResult = "<strong>Пример strong</strong>",
            TestName = "WhenDoubleStarTag")]
        [TestCase("# Заголовок", ExpectedResult = "<h1> Заголовок</h1>", TestName = "WhenSharpAtStartLine")]
        [TestCase("не # Заголовок", ExpectedResult = "не # Заголовок", TestName = "WhenSharpAtMiddleLine")]
        public string Render_ReplacedTagsFromSpec(string rawText)
        {
            return Md.Render(rawText);
        }

        [TestCase("_foo bar_ __bla_bla_bla__")]
        public void Render_WorkLinear(string rawText)
        {
            var millisecondsSpent = new List<long>();
            var rawTextStingBuilder = new StringBuilder();
            var watch = new Stopwatch();
            for (var k = 0; k < 1000; k++)
            {
                rawTextStingBuilder.Append(rawText);
                watch.Start();
                Md.Render(rawTextStingBuilder.ToString());
                watch.Stop();
                millisecondsSpent.Add(watch.ElapsedMilliseconds);
                watch.Reset();
            }

            for (var j = 1; j < millisecondsSpent.Count; j += 2)
            {
                Assert.AreEqual(millisecondsSpent[j - 1], millisecondsSpent[j], 50);
            }
        }
    }
}