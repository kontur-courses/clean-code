using FluentAssertions;
using Markdown.InlinePieces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Tests
{
    [TestFixture]
    public class HtmlRenderer_Tests
    {
        private static void AssertOneBlockHtml(RenderedBlock block, string expected)
        {
            HtmlRenderer.Render(new RenderedBlock[] { block }).Should().Be(expected);
        }


        [Test]
        public void HtmlRenderer_Render_ReturnsExpectedHtmlStringSmallTests()
        {
            AssertOneBlockHtml(new RenderedBlock(BlockType.Heading, new InlinePiece[] {
                new TextInlinePiece(@"Heading")
            }),
            @"<h1>Heading</h1>"
            );

            AssertOneBlockHtml(new RenderedBlock(BlockType.Heading, new InlinePiece[] {
                new TextInlinePiece(@"Heading "),
                new EmInlinePiece( new InlinePiece[] {new TextInlinePiece(@"With em text") })
            }),
            @"<h1>Heading <em>With em text</em></h1>"
            );

            AssertOneBlockHtml(new RenderedBlock(BlockType.Heading, new InlinePiece[] {
                new TextInlinePiece(@"Heading "),
                new StrongInlinePiece( new InlinePiece[] {new TextInlinePiece(@"With strong text") })
            }),
            @"<h1>Heading <strong>With strong text</strong></h1>"
            );

            AssertOneBlockHtml(new RenderedBlock(BlockType.Paragraph, new InlinePiece[] {
                new TextInlinePiece(@"abc "),
                new EmInlinePiece( new InlinePiece[] {new TextInlinePiece(@"em") }),
                new TextInlinePiece(@" "),
                new StrongInlinePiece( new InlinePiece[] {new TextInlinePiece(@"strong") })
            }),
            @"<p>abc <em>em</em> <strong>strong</strong></p>"
            );

            AssertOneBlockHtml(new RenderedBlock(BlockType.Paragraph, new InlinePiece[] {
            }),
            @"<p></p>"
            );
        }

        [Test]
        public void HtmlRenderer_Render_ReturnsExpectedHtmlStringRealTests()
        {
        }
    }
}
