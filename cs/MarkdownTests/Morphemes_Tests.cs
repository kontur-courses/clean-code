using FluentAssertions;
using Markdown.Morphemes;
using NUnit.Framework;

namespace MarkdownTests
{
    public class Morphemes_Tests
    {
        [TestCase("_a", 0, true)]
        [TestCase("_a", 1, false)]
        [TestCase("a_", 1, false)]
        [TestCase("__aa", 0, false)]
        [TestCase("__aa", 1, true)]
        [TestCase("_a_", 2, false)]
        [TestCase("_a_", 0, true)]
        [TestCase("1_123", 1, false)]
        [TestCase("a_123", 1, false)]
        [TestCase("1_a23", 1, false)]
        [TestCase("_", 0, false)]
        [TestCase("_ adsda", 0, false)]
        [TestCase("ds_da", 2, true)]
        [TestCase("___a___", 0, false)]
        [TestCase("___a___", 1, false)]
        [TestCase("___a___", 2, true)]
        public void PrefixItalic(string text, int position, bool expected)
        {
            var a = new PrefixItalic();
            var result = a.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("_a", 0, false)]
        [TestCase("_a", 1, false)]
        [TestCase("a_", 1, true)]
        [TestCase("__aa", 0, false)]
        [TestCase("__aa", 1, false)]
        [TestCase("_a_", 2, true)]
        [TestCase("_a_", 0, false)]
        [TestCase("1_123", 1, false)]
        [TestCase("a_123", 1, false)]
        [TestCase("1_a23", 1, false)]
        [TestCase("_", 0, false)]
        [TestCase("_ adsda", 0, false)]
        [TestCase("abc _", 4, false)]
        [TestCase("ds_da", 2, true)]
        [TestCase("___a___", 6, false)]
        [TestCase("___a___", 5, false)]
        [TestCase("___a___", 4, true)]
        public void PostfixItalic(string text, int position, bool expected)
        {
            var a = new PostfixItalic();
            var result = a.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("__ab", 0, true)]
        [TestCase("__ab", 1, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("__", 0, false)]
        [TestCase("ab__", 2, false)]
        [TestCase("ab__", 3, false)]
        [TestCase("__ ", 0, false)]
        [TestCase("asda__dsada", 4, true)]
        [TestCase("asda__dsada", 5, false)]
        [TestCase("___a", 0, true)]
        [TestCase("___a", 1, true)]
        [TestCase("___a", 2, false)]
        [TestCase("a___", 2, false)]
        [TestCase("a___", 3, false)]
        [TestCase("a___", 4, false)]
        public void PrefixBold(string text, int position, bool expected)
        {
            var a = new PrefixBold();
            var result = a.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }


        [TestCase("b__", 2, false)]
        [TestCase("b__", 1, true)]
        [TestCase("a_", 1, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("_ab", 0, false)]
        [TestCase("__ab", 0, false)]
        [TestCase("__ab", 1, false)]
        [TestCase("__", 0, false)]
        [TestCase("__", 1, false)]
        [TestCase(" __", 2, false)]
        [TestCase("asda__dsada", 4, true)]
        [TestCase("a___", 1, true)]
        [TestCase("a___", 2, true)]
        [TestCase("a___", 3, false)]
        public void PostfixBold(string text, int position, bool expected)
        {
            var a = new PostfixBold();
            var result = a.CheckForCompliance(text, position);
            result.Should().Be(expected);
        }
    }
}