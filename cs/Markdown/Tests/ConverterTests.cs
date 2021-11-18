using System;
using FluentAssertions;
using Markdown.TagStore;
using Markdown.Tokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ConverterTests
    {
        [TestCase("_Lorem ipsum dolor_ sit amet", "<em>Lorem ipsum dolor</em> sit amet")]
        [TestCase("_Lorem _ipsum dolor_ sit_ amet", "<em>Lorem <em>ipsum dolor</em> sit</em> amet")]
        [TestCase("Lorem _ipsum dolor_ sit_ amet", "Lorem <em>ipsum dolor</em> sit_ amet")]
        public void Convert_ShouldReplaceMdTagToHtml(string text, string convertedText)
        {
            var from = new MdTagStore();
            var to = new HtmlTagStore();
            var sut = new Converter(from, to);

            var converted = sut.Convert(text);
            Console.WriteLine(converted);
            converted.Should().Be(convertedText);
        }

        [Test]
        public void Convert_ShouldReplaceHtmlTagToMd()
        {
            var from = new HtmlTagStore();
            var to = new MdTagStore();
            var sut = new Converter(from, to);

            var converted = sut.Convert("Lorem <em>ipsum dolor</em> sit_ amet");

            converted.Should().Be("Lorem _ipsum dolor_ sit_ amet");
        }
    }
}