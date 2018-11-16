using System;
using NUnit.Framework;
using Markdown;
using FluentAssertions;
using FluentAssertions.Common;

namespace MarkdownTests
{
    [TestFixture]
    public class MdTests
    {
        [TestCase("_abc_", @"<em>abc</em>", TestName = "Tagged italic word")]
        [TestCase("__abc__", @"<strong>abc</strong>", TestName = "Tagged strong word")]
        [TestCase("aa _bb_ aa", @"aa <em>bb</em> aa", TestName = "Tagged word between words")]
        [TestCase("_abc_ d _abc_", @"<em>abc</em> d <em>abc</em>", TestName = "Word between tagged words")]
        [TestCase(@"\_asd\_", "_asd_", TestName = "Escape both tags")]
        [TestCase(@" \_ ", " _ ", TestName = "Escape one symbol")]
        [TestCase(@"_\_a_", @"<em>_a</em>", TestName = "Escape half of tag")]
        [TestCase(@"_abc_ __abc__", @"<em>abc</em> <strong>abc</strong>", TestName = "Italic and strong")]
        [TestCase(@"__abc__ _abc_", @"<strong>abc</strong> <em>abc</em>", TestName = "Strong and italic")]
        [TestCase(@"__abc _cde_ abc__", @"<strong>abc <em>cde</em> abc</strong>", TestName = "Italic in strong")]
        [TestCase(@"__abc _cde_ _cde_ abc__", @"<strong>abc <em>cde</em> <em>cde</em> abc</strong>", TestName = "Two italic in strong")]
        [TestCase(@"_abc __cde__ abc_", @"<em>abc __cde__ abc</em>", TestName = "Strong in italic")]
        [TestCase(@"_abc __cde__ __cde__ abc_", @"<em>abc __cde__ __cde__ abc</em>", TestName = "Two strong in italic")]
        [TestCase("_a", "_a", TestName = "Not closed tag")]
        [TestCase("_a __b", "_a __b", TestName = "Not closed tags")]
        [TestCase("_a __abc__", @"_a <strong>abc</strong>", TestName = "Not closed before closed")]
        [TestCase("_a __a a_", "<em>a __a a</em>", TestName = "Not closed in closed")]
        [TestCase("a_ a_", "a_ a_", TestName = "No whitespace after tag open")]
        [TestCase("_a _a", "_a _a", TestName = "No whitespace before tag close")]
        [TestCase("a_ _a", "a_ _a")]
        [TestCase("_3", "_3")]
        [TestCase("1_", "1_")]
        [TestCase("abc_12_3", "abc_12_3")]
        public void ParserShould(string rawString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rawString);
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void HtmlConverter_ShouldConvert()
        {
            var converter = new HtmlConverter();
            var span = new Span(new Tag(TagValue.Italic, "_", "_"), 0, 2);
            var result = converter.Convert("_a_", span);
            result.Should().BeEquivalentTo("<em>a</em>");
        }
    }
}
