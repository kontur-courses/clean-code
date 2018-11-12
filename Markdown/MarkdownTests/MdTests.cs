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
        //Одинаковые тесты по сути то

        [TestCase("aa_bb_aa", @"aa<em>bb<\em>aa")]
        [TestCase("_abc_", @"<em>abc<\em>")]
        [TestCase(" _abc_", @" <em>abc<\em>")]
        [TestCase("_abc_ ", @"<em>abc<\em> ")]
        [TestCase("_abc_ d _abc_", @"<em>abc<\em> d <em>abc<\em>")]
        public void ShouldParse_Italic(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(@"\_asd\_", "_asd_")]
        [TestCase(@" \_ ", " _ ")]
        [TestCase(@" \_\__a_ \_", @" __<em>a<\em> _")]
        [TestCase(@" \_abcdefg\_ ", " _abcdefg_ ")]
        //[TestCase(@"\", @"\")]
        public void ShouldParse_EscapeSymbols(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("__abc__", @"<strong>abc<\strong>")]
        public void ShouldParse_Strong(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(@"_abc_ __abc__", @"<em>abc<\em> <strong>abc<\strong>")]
        [TestCase(@"__abc__ _abc_", @"<strong>abc<\strong> <em>abc<\em>")]
        public void ShouldParse_StrongAndItalic(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase(@"a_abc __cde__ abc_a", @"a<em>abc <strong>cde<\strong> abc<\em>a")]
        [TestCase(@"_abc __cde__ abc_", @"<em>abc <strong>cde<\strong> abc<\em>")]
        [TestCase(@"__abc _cde_ abc__", @"<strong>abc <em>cde<\em> abc<\strong>")]
        [TestCase(@"__abc _cde_ _cde_ abc__", @"<strong>abc <em>cde<\em> <em>cde<\em> abc<\strong>")]
        public void ShouldParse_StrongInItalic(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
