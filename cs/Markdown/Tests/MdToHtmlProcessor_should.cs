using System;
using FluentAssertions;
using Markdown.MdProcessing;
using Markdown.MdTokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdToHtmlProcessor_should
    {
        private MdToHtmlProcessor processor;
        [SetUp]
        public void SetUp()
        {
            processor = new MdToHtmlProcessor();
        }

        [Test]
        public void GetProcessedResult_Should_ThrowArgumentNullException_When_TokenIsNull()
        {
            Following.Code(() => processor.GetProcessedResult(null)).Should().Throw<ArgumentNullException>();
        }
        
        [Test]
        public void GetProcessedResult_Should_ThrowArgumentException_When_TokenHasEmptyContent()
        {
            var token = new MdToken("", "NONE", "NONE");
            Following.Code(() => processor.GetProcessedResult(token)).Should().Throw<ArgumentException>();
        }

        [TestCase("text", "_", "_", "<em>text</em>", TestName = "when token with both emphasis")]
        [TestCase("text", "__", "__", "<strong>text</strong>", TestName = "when token with both bold symbols")]
        [TestCase("text", "_", "NONE", "<em>text", TestName = "when token with emphasis in beginning")]
        [TestCase("text", "NONE", "_", "text</em>", TestName = "when token with emphasis in ending")]
        [TestCase("text", "__", "NONE", "<strong>text", TestName = "when token with bold symbol in beginning")]
        [TestCase("text", "NONE", "__", "text</strong>", TestName = "when token with bold symbol in ending")]
        [TestCase("text", "NONE", "NONE", "text", TestName = "when token with no special symbols ending")]
        [TestCase("_text_", @"NONE", @"NONE", "_text_", TestName = "when token with shielded emphasis")]
        [TestCase("text_", @"_", @"NONE", "<em>text_", TestName = "when token with shielded emphasis at the end")]
        [TestCase("_text", @"NONE", @"_", "_text</em>", TestName = "when token with shielded emphasis at the beginning")]
        [TestCase("__text__", @"NONE", @"NONE", "__text__", TestName = "when token with shielded bold symbols")]
        [TestCase("text__", @"__", @"NONE", "<strong>text__", TestName = "when token with shielded bold symbol at the end")]
        [TestCase("__text", @"NONE", @"__", "__text</strong>", TestName = "when token with shielded bold symbol at the beginning")]
        [TestCase("abc", "#", "#", "<h1>abc</h1>", TestName = "when token with header first level")]
        [TestCase("abc", "##", "##", "<h2>abc</h2>", TestName = "when token with header second level")]
        [TestCase("abc", "###", "###", "<h3>abc</h3>", TestName = "when token with header third level")]
        [TestCase("abc", "####", "####", "<h4>abc</h4>", TestName = "when token with header fourth level")]
        [TestCase("abc", "#####", "#####", "<h5>abc</h5>", TestName = "when token with header fifth level")]
        [TestCase("abc", "######", "######", "<h6>abc</h6>", TestName = "when token with header sixth level")]
        public void GetProcessedResult_Should_ProcessTokenToHtml(string tokenContent,
            string beginningSymbol,
            string endingSymbol,
            string parsedExpectedText)
        {
            var token = new MdToken(tokenContent, beginningSymbol, endingSymbol);
            var parsedText = processor.GetProcessedResult(token);
            parsedText.Should().Be(parsedExpectedText);
        }

    }
}