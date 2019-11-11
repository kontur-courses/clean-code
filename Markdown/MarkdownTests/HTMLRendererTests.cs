using System;
using FluentAssertions;
using Markdown;
using Markdown.MarkdownDocument;
using NUnit.Framework;

namespace MarkdownTests
{
    public class HtmlRendererTests
    {
        [Test]
        public void RendersPlaintText()
        {
            var line = new Line(Lexer.ExtractLexemes("Hello world!"));
            
            var html = HtmlRenderer.RenderParagraph(line);
            
            html.Should().Be("<p>Hello world!</p>");
        }
    }
}