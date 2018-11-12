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
        [TestCase("_aaa_", "<em>aaa</em>")]
        [TestCase(" _aaa_", " <em>aaa</em>")]
        [TestCase("_aaa_ ", "<em>aaa</em> ")]
        [TestCase("_aaa_ a _bbb_", "<em>aaa</em> a <em>bbb</em>")]
        public void ShouldParse_Italic(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        //Одинаковые тесты по сути то

        [TestCase("/_asd/_", "_asd_")]
        [TestCase(" /_ ", " _ ")]
        [TestCase(" /_/__a_ /_", " __<em>a</em> _")]
        public void ShouldParse_EscapeSymbols(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }

        [TestCase("__aaa__", "<strong>aaa</strong>")]
        public void ShouldParse_Strong(string rowString, string expected)
        {
            var parser = new Md();
            var result = parser.Render(rowString);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
