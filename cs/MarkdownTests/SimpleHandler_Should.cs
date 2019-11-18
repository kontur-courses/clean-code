﻿using FluentAssertions;
using Markdown;
using Markdown.RenderUtilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownTests
{
    [TestFixture]
    public class SimpleHandler_Should
    {
        private TokenReader tokenReader;
        private Renderer renderer;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            tokenReader = new TokenReader(MarkdownTokenUtilities.GetMarkdownTokenDescriptions());
            renderer = new Renderer(new List<ITokenHandler>()
            {
                 new SimpleHandler(MarkdownRenderUtilities.GetSimpleTokenHandleDescriptions())
            });
        }

        [TestCase("abcd")]
        [TestCase("dcba")]
        public void RenderSimpleText_Properly(string text)
        {
            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);
            resultText.Should().BeEquivalentTo(text);
        }

        [Test]
        public void RenderEscapedText_Properly()
        {
            var text = @"\a\b\c\d\\";
            var tokens = tokenReader.TokenizeText(text);
            var resultText = renderer.RenderText(tokens);
            resultText.Should().BeEquivalentTo(@"abcd\");
        }
    }
}
