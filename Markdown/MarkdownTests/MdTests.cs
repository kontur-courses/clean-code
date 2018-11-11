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
        public void ShouldParse_Italic(string markdown, string expected)
        {
            var parser = new Md();
            var result = parser.Render(markdown);
            result.Should().BeEquivalentTo(expected);
        }
    }
}
