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
        private Md parser;
        //Одинаковые тесты по сути то
        [SetUp]
        public void SetUp()
        {
            parser = new Md();
        }

        [TestCase("aa_bb_aa", @"aa<em>bb</em>aa")]
        [TestCase("_abc_", @"<em>abc</em>")]
        [TestCase(" _abc_", @" <em>abc</em>")]
        [TestCase("_abc_ ", @"<em>abc</em> ")]
        [TestCase("_abc_ d _abc_", @"<em>abc</em> d <em>abc</em>")]
        public void ShouldParse_Italic(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(@"\_asd\_", "_asd_")]
        [TestCase(@" \_ ", " _ ")]
        [TestCase(@" \_\__a_ \_", @" __<em>a</em> _")]
        [TestCase(@" \_abcdefg\_ ", " _abcdefg_ ")]
        //[TestCase(@"\", @"\")]
        public void ShouldParse_EscapeSymbols(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("__abc__", @"<strong>abc</strong>")]
        public void ShouldParse_Strong(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(@"_abc_ __abc__", @"<em>abc</em> <strong>abc</strong>")]
        [TestCase(@"__abc__ _abc_", @"<strong>abc</strong> <em>abc</em>")]
        public void ShouldParse_StrongAndItalic(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }
        
        [TestCase(@"__abc _cde_ abc__", @"<strong>abc <em>cde</em> abc</strong>")]
        [TestCase(@"__abc _cde_ _cde_ abc__", @"<strong>abc <em>cde</em> <em>cde</em> abc</strong>")]
        public void ShouldParse_TagInTag(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("_a", "_a")]
        [TestCase("_a __b", "_a __b")]
        [TestCase("_a __abc__", @"_a <strong>abc</strong>")]
        [TestCase("__a _abc_", @"__a <em>abc</em>")]
        [TestCase("_ __a _", "<em> __a </em>")]
        public void ShouldParse_NotClosed_AsSymbols(string rowString, string expected)
        {
            var result = parser.Render(rowString, Markups.Markdown, Markups.Html);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
