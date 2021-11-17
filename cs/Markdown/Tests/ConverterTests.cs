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
            var converter = new Converter(from, to);

            var converted = converter.Convert(text);
            Console.WriteLine(converted);
            converted.Should().Be(convertedText);
        }
    }
}