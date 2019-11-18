using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser;
using NUnit.Framework;

namespace Markdown.Tests.MdTag
{
    class HtmlConverter
    {
        private MdTagParser mdTagParser;

        [SetUp]
        public void SetUp()
        {
            mdTagParser = new MdTagParser();
        }

        [Test]
        public void Should_WrapIntoEmTag()
        {
            var wrapped = mdTagParser.Parse("_a b c_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<em>a b c</em>" });
        }

        [Test]
        public void Should_WrapIntoStrongTag()
        {
            var wrapped = mdTagParser.Parse("__a b c__").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>a b c</strong>" });
        }

        [Test]
        public void Should_WrapIntoStrongTag_With_AnotherTag()
        {
            var wrapped = mdTagParser.Parse("**a b c**").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>a b c</strong>" });
        }

        [Test]
        public void Should_NotWrap_When_NoTags()
        {
            var wrapped = mdTagParser.Parse("a b c").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "a b c" });
        }

        [Test]
        public void Should_NotWrap_When_DigitsInsideTag()
        {
            var wrapped = mdTagParser.Parse("_13_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_13_" });
        }

        [Test]
        public void Should_NotWrap_When_SpaceAfterTag()
        {
            var wrapped = mdTagParser.Parse("_ ab_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_ ab_" });
        }

        [Test]
        public void Should_NotWrap_When_SpaceBeforeTag()
        {
            var wrapped = mdTagParser.Parse("_ab _").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_ab _" });
        }


        [Test]
        public void Should_WrapNestedTags()
        {
            var wrapped = mdTagParser.Parse("__s _a _b_ c_ _b_ _dv_ vs__").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>s <em>a <em>b</em> c</em> <em>b</em> <em>dv</em> vs</strong>" });
        }

        [Test]
        public void Should_NotWork_WithBackSlash()
        {
            var wrapped = mdTagParser.Parse(@"\_abc\_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "_abc_" });
        }

        [Test]
        public void Should_WorkWith_TwoSlashes()
        {
            var wrapped = mdTagParser.Parse(@"\\_abc\\_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { @"\", @"<em>abc\</em>" });
        }

        [Test]
        public void Should_NotWork_When_DifferentTagsAtTheEnds()
        {
            var wrapped = mdTagParser.Parse("_b__").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> {"_b__"});
        }

        [Test]
        public void Should_Work_WithUnclosedTags()
        {
            var wrapped = mdTagParser.Parse("__d _f _v_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "__d _f <em>v</em>" });
        }

        [Test]
        public void Should_Work_WithManyTags()
        {
            var wrapped = mdTagParser.Parse("__d__ _f_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strong>d</strong>", " ", "<em>f</em>" });
        }

        [Test]
        public void Should_Work_When_TagInsideTagWithDigits()
        {
            var wrapped = mdTagParser.Parse("__12 _f_ 23_").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "__12 ", "<em>f</em>", " 23_" });
        }

        [Test]
        public void Should_NotWork_When_ManyUnclosedTags()
        {
            var wrapped = mdTagParser.Parse("__f _a __v").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "__f _a __v" });
        }

        [Test]
        public void Should_WrapIntoStrikeTag()
        {
            var wrapped = mdTagParser.Parse("~abc~").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<strike>abc</strike>" });
        }

        [Test]
        public void Should_WrapIntoCodeTag()
        {
            var wrapped = mdTagParser.Parse("`abc`").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<code>abc</code>" });
        }

        [Test]
        public void Should_WrapIntoHeaderTag()
        {
            var wrapped = mdTagParser.Parse("##abc\n").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<h2>abc</h2>" });
        }

        [Test]
        public void Should_WrapIntoHeaderWithOutClosingTag()
        {
            var wrapped = mdTagParser.Parse("####abc").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<h4>abc</h4>" });
        }

        [Test]
        public void Should_WrapIntoHorizontalTag()
        {
            var wrapped = mdTagParser.Parse("***").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<hr>" });
        }

        [Test]
        public void Should_WrapIntoHorizontalTag_With_AnotherTag()
        {
            var wrapped = mdTagParser.Parse("___").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<hr>" });
        }

        [Test]
        public void Should_WrapIntoBlockQuoteTag()
        {
            var wrapped = mdTagParser.Parse(">abc").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<blockquote>abc</blockquote>" });
        }

        [Test]
        public void Should_WrapIntoNestedBlockQuoteTag()
        {
            var wrapped = mdTagParser.Parse(">ab>c").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<blockquote>ab<blockquote>c</blockquote></blockquote>" });
        }

        [Test]
        public void Should_WrapIntoListTag()
        {
            var wrapped = mdTagParser.Parse("*abc").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<li>abc</li>" });
        }

        [Test]
        public void Should_WrapIntoNestedListTag()
        {
            var wrapped = mdTagParser.Parse("*ab*c*d").Select(tag => tag.WrapTagIntoHtml()).ToList();
            wrapped.Should().BeEquivalentTo(new List<string> { "<li>ab<li>c<li>d</li></li></li>" });
        }
    }
} 
