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

        //Одинаковые тесты по сути то

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
        public void ShouldParse_StrongAndItalic(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
