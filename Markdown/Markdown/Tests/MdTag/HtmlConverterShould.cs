using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Markdown.Parser;
using NUnit.Framework;

namespace Markdown.Tests.MdTag
{
    class HtmlConverterShould
    {
        private MdTagParser mdTagParser;

        [SetUp]
        public void SetUp()
        {
            mdTagParser = new MdTagParser();
        }

        [Test]
        public void WrapIntoEmTag()
        {
            var wrapped = mdTagParser.Parse("_a b c_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<em>a b c</em>" });
        }

        [Test]
        public void WrapIntoStrongTag()
        {
            var wrapped = mdTagParser.Parse("__a b c__").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>a b c</strong>" });
        }

        [Test]
        public void NotWrap_When_NoTags()
        {
            var wrapped = mdTagParser.Parse("a b c").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "a", "b", "c" });
        }

        [Test]
        public void NotWrap_When_DigitsInsideTag()
        {
            var wrapped = mdTagParser.Parse("_13_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_13_" });
        }

        [Test]
        public void NotWrap_When_SpaceAfterTag()
        {
            var wrapped = mdTagParser.Parse("_ ab_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_", "ab_" });
        }

        [Test]
        public void NotWrap_When_SpaceBeforeTag()
        {
            var wrapped = mdTagParser.Parse("_ab _").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_ab _" });
        }


        [Test]
        public void WrapNestedTags()
        {
            var wrapped = mdTagParser.Parse("__s _a _b_ c_ _b_ _dv_ vs__").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>s<em>a<em>b</em> c</em><em>b</em><em>dv</em> vs</strong>" });
        }
    }
}
