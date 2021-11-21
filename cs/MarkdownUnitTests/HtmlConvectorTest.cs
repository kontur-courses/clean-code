using FluentAssertions;
using Markdown;
using NUnit.Framework;
using System.Linq;

namespace MarkdownUnitTests
{
    public class HtmlConvectorTest
    {
        private readonly Tokenizer tokenizer = new Tokenizer();

        [Test]
        public void Convert_ShouldBe_TextTag()
        {
            var tokenList = tokenizer.Parse("test").ToList();

            new HtmlConvector().Convert(tokenList).Should().Be("test");
        }

        [Test]
        public void Convert_BoldTag_ShouldBeCorrectly()
        {
            var tokenList = tokenizer.Parse("__test__").ToList();

            new HtmlConvector().Convert(tokenList).Should().Be("<strong>test</strong>");
        }

        [Test]
        public void Convert_ItalicTag_ShouldBeCorrectly()
        {
            var tokenList = tokenizer.Parse("_test_").ToList();

            new HtmlConvector().Convert(tokenList).Should().Be("<em>test</em>");
        }

        [Test]
        public void Convert_NestedTag_ShouldBeCorrectly()
        {
            var tokenList = tokenizer.Parse("Заголовок __с _разными_ символами__").ToList();

            new HtmlConvector().Convert(tokenList).Should()
                .Be("Заголовок <strong>с <em>разными</em> символами</strong>");
        }

        [Test]
        public void Convert_EmptyString_ShouldBeEmptyString()
        {
            var tokenList = tokenizer.Parse("").ToList();

            new HtmlConvector().Convert(tokenList).Should()
                .Be("");
        }

        [Test]
        public void Convert_SpaceString_ShouldBeSpaceString()
        {
            var tokenList = tokenizer.Parse(" ").ToList();

            new HtmlConvector().Convert(tokenList).Should()
                .Be(" ");
        }

        [TestCase("__test")]
        [TestCase("test__")]
        public void Convert_UnclosedBoldTag_ReturnsStringToken(string input)
        {
            var tokenList = tokenizer.Parse(input).ToList();

            new HtmlConvector().Convert(tokenList).Should()
                .Be(input);
        }

        [TestCase("_ test_")]
        [TestCase("__ test__")]
        [TestCase("_test _")]
        [TestCase("__test __")]
        [TestCase("__ test __")]
        public void Convert_WhiteSpaceAroundTag_ReturnsStringToken(string input)
        {
            var tokenList = tokenizer.Parse(input).ToList();

            new HtmlConvector().Convert(tokenList).Should()
                .Be(input);
        }
    }
}