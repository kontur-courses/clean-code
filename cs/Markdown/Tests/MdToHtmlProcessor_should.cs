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