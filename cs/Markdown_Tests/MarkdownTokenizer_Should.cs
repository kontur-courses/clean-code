using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownTokenizer_Should
    {
        private MarkdownTokenizer markdown;

        [SetUp]
        public void MakeMarkdown() => 
            markdown = new MarkdownTokenizer();

        [TestCase("_a", 2, TestName = "Underscore on left")]
        [TestCase("a_", 2, TestName = "Underscore on right")]
        [TestCase("__a", 2, TestName = "Two underscores on left")]
        [TestCase("a__", 2,  TestName = "Two underscores on right")]
        [TestCase("123456789__", 2, TestName = "Two underscores on right after digits")]
        [TestCase("__a_", 3, TestName = "Two underscores on left and one on right")]
        public void SplitTextToTokens_InputStringWithOneTag_ReturnCorrectTokensCountAndTypes(string input, int tagCount)
        {
            var tokens = markdown.SplitTextToTokens(input).Select(x => x.Type).ToArray();

            tokens.Length.Should().Be(tagCount);
            tokens.Should().BeEquivalentTo(new TagType[tagCount].Select(x => TagType.None));
        }

        [TestCase("__a___s_", TagType.Bold, TagType.Italics, TestName = "Bold and Italic")]
        [TestCase("_s_", TagType.Italics, TestName = "Italic")]
        [TestCase("__s__", TagType.Bold, TestName = "Bold")]
        public void SplitTextToTokens_InputString_ReturnCorrectTypes(string input, params TagType[] types)
        {
            var tokens = markdown.SplitTextToTokens(input).Select(x => x.Type).ToArray();

            tokens.Should().BeEquivalentTo(types);
        }
    }
}