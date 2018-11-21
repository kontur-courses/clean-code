using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            var neutralizingSymbols = "0123456789".ToCharArray();
            var emTag = new Tag("_", "em", neutralizingSymbols, new List<string> { "__" });
            var strongTag = new Tag("__", "strong", neutralizingSymbols);
            var tagList = new List<Tag>
            {
                emTag,
                strongTag
            };
            md = new Md(tagList);
        }

        [Test]
        public void ReturnEmptyString_WhenInputIsEmpty()
        {
            var input = "";

            var result = md.Render(input);

            result.Should().BeEmpty();
        }

        [Test]
        public void ReplaceOneGroundSymbolsToEmTags()
        {
            var input = "_word_";
            var expected = "<em>word</em>";

            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [Test]
        public void ReplaceTwoGroundSymbolsToStrongTags()
        {
            var input = "__word__";
            var expected = "<strong>word</strong>";

            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [TestCase("_ word_", "_ word_", TestName = "when whitespace after opener tag")]
        [TestCase("_word _", "_word _", TestName = "when whitespace before closer tag")]
        [TestCase("_word1_ 3_", "_word1_ 3_", TestName = "when tag inside digits")]
        [TestCase(@"\_word\_", "_word_", TestName = "when symbols are escaped")]
        [TestCase("_some __sentence__ here_", "<em>some __sentence__ here</em>", TestName = "when double grounding tags inside once grounding tags")]
        public void DoesntReplace(string input, string expected)
        {
            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [Test]
        public void WorkWithLinearTimeHardness()
        {
            var baseLine = @"_em tag_ \_quoted\_ __nested _tag___ __double_";
            var input = new StringBuilder(baseLine);
            var measuring = new List<int>();
            DetectExecutionTime(md.Render, input.ToString());
            for (var i = 1; i < 500; i++)
            {
                input.Append(baseLine);
                var measure = DetectExecutionTime(md.Render, input.ToString());
                measuring.Add(measure);
            }
            var result = IsLinear(measuring);
            result.Should().BeTrue();
        }

        private bool IsLinear(List<int> values)
        {
            for (var i = 1; i < values.Count; i++)
            {
                if (Math.Abs(values[i] - values[i - 1]) > 50)
                    return false;
            }

            return true;
        }

        private int DetectExecutionTime(Func<string, string> func, string input)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            func.Invoke(input);
            stopwatch.Stop();
            return (int)stopwatch.ElapsedMilliseconds;
        }
    }
}
