using FluentAssertions;
using Markdown.Renderer;
using Markdown.Tags;
using Markdown.Tokenizer;
using Markdown.Tokenizer.Tokens;

namespace MarkdownTests
{
    [TestFixture]
    public class HtmlRendererTests
    {
        private HtmlRenderer renderer;

        [SetUp]
        public void SetUp()
        {
            renderer = new HtmlRenderer();
        }

        [Test]
        public void Render_ShouldReturnEmptyString_WhenNoTokens()
        {
            var result = renderer.Render(new List<IToken>());
            result.Should().BeEmpty();
        }

        [Test]
        public void Render_ShouldRenderTextToken()
        {
            var tokens = new List<IToken> { new TextToken("Hello, World!") };
            var result = renderer.Render(tokens);
            result.Should().Be("Hello, World!");
        }

        [Test]
        public void Render_ShouldRenderStrongTagToken()
        {
            var tokens = new List<IToken>
            {
                new TagToken(new StrongTag())
                {
                    Children = new List<IToken> { new TextToken("Hello, World!") }
                }
            };
            var result = renderer.Render(tokens);
            result.Should().Be("<strong>Hello, World!</strong>");
        }

        [Test]
        public void Render_ShouldRenderNestedTagTokens()
        {
            var tokens = new List<IToken>
            {
                new TagToken(new StrongTag())
                {
                    Children = new List<IToken>
                    {
                        new TagToken(new CursiveTag())
                        {
                            Children = new List<IToken> { new TextToken("Hello, World!") }
                        }
                    }
                }
            };
            var result = renderer.Render(tokens);
            result.Should().Be("<strong><em>Hello, World!</em></strong>");
        }

        [Test]
        public void Render_ShouldRenderImgTagToken()
        {
            var tokens = new List<IToken>
            {
                new TagToken(new ImageTag())
                {
                    Attributes = new Dictionary<string, string> { ["src"] = "img.jpg", ["alt"] = "Hello, Image!" }
                }
            };
            var result = renderer.Render(tokens);
            result.Should().Be("<img src=\"img.jpg\" alt=\"Hello, Image!\"></img>");
        }

        [Test]
        public void Render_ShouldEscapeSpecialCharacters()
        {
            var tokens = new List<IToken> { new TextToken("Hello, <div>World</div>, &!") };
            var result = renderer.Render(tokens);
            result.Should().Be("Hello, &lt;div&gt;World&lt;/div&gt;, &amp;!");
        }
    }
}