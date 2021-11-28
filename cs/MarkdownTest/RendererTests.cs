using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.TokenCreator.Tokens;
using NUnit.Framework;

namespace MarkdownTest
{
    public class RendererTests
    {
        private Renderer renderer;
        [SetUp]
        public void Setup()
        {
            renderer = new Renderer();
        }

        [TestCaseSource(nameof(RendererTestCaseData))]
        public void Render_ShouldRenderRightHTMLCode_When(TokenTree[] trees, string expected)
        {
            var actual = renderer.Render(trees);

            actual.Should().Be(expected);
        }

        private static IEnumerable<TestCaseData> RendererTestCaseData()
        {
            yield return new TestCaseData(new TokenTree[]
            {
                new TokenTree(new TokenHeader1(),
                    new List<TokenTree>
                    {
                        new TokenTree(new TokenStrong(),
                            new List<TokenTree>
                            {
                                new TokenTree("text")
                            }),
                        new TokenTree(new TokenItalics(),
                            new List<TokenTree>
                            {
                                new TokenTree("text")
                            })
                    })
            }, "<h1><strong>text</strong><em>text</em></h1>").SetName("Nested structure"); 
            
            yield return new TestCaseData(new TokenTree[]
            {
                new TokenTree("text "),
                new TokenTree("text")
            }, "text text").SetName("Not nested structure"); 
        }
    }
}