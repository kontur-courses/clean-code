using System;
using System.Collections;
using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using Markdown.Tags;
using Markdown.TagStore;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class ConverterTests
    {
        [TestCase("_Lorem _ipsum dolor_ sit_ amet", "<em>Lorem <em>ipsum dolor</em> sit</em> amet",
            TestName = "Two nested paired tags")]
        [TestCase("Lorem _ipsum dolor_ sit_ amet", "Lorem <em>ipsum dolor</em> sit_ amet",
            TestName = "One paired tag and one unnecessary")]
        [TestCase("Lorem _ipsum_ _dolor_ sit amet", "Lorem <em>ipsum</em> <em>dolor</em> sit amet",
            TestName = "Two consecutive paired tags")]
        [TestCase("__Lorem__", "<strong>Lorem</strong>", TestName = "Strong tag")]
        [TestCase("\\_Lorem_", "_Lorem_", TestName = "Escaped tag")]
        [TestCase("\\_L\\orem_", "_L\\orem_", TestName = "Escaped tag")]
        public void Convert_ShouldReplaceMdTagToHtml(string text, string convertedText)
        {
            var from = new MdTagStore();
            var to = new HtmlTagStore();
            var sut = new Converter(from, to);
            
            var converted = sut.Convert(text);
            
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