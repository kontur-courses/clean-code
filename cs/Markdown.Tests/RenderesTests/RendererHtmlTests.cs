using FluentAssertions;
using Markdown.Renderers;
using Markdown.Tokens.HtmlTokens;

namespace Markdown.Tests.RenderesTests
{
    [TestFixture]
    public class RendererHtmlTests
    {
        private RendererHTML _renderer;
        private readonly string _dummyText = "Dummy";

        [SetUp]
        public void ClearRenderer()
        {
            _renderer = new RendererHTML();
        }

        [Test]
        public void RenderBold_ThrowsException_ReceivingNullAsToken()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.RenderBold(null!));
        }

        [Test]
        public void RenderBold_RendersCorrectly()
        {
            _renderer.RenderBold(
                new BoldToken(
                    new TextToken(_dummyText)));
            _renderer.ToString().Should().Be($"<strong>{_dummyText}</strong>");
        }

        [Test]
        public void RenderHeader_ThrowsException_ReceivingNullAsToken()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.RenderHeader(null!));
        }

        [Test]
        public void RenderHeader_RendersCorrectly()
        {
            _renderer.RenderHeader(
                new HeaderToken(
                    new TextToken(_dummyText)));
            _renderer.ToString().Should().Be($"<h1>{_dummyText}</h1>");
        }

        [Test]
        public void RenderItalic_ThrowsException_ReceivingNullAsToken()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.RenderItalic(null!));
        }

        [Test]
        public void RenderItalic_RendersCorrectly()
        {
            _renderer.RenderItalic(
                new ItalicToken(
                    new TextToken(_dummyText)));
            _renderer.ToString().Should().Be($"<em>{_dummyText}</em>");
        }

        [Test]
        public void RenderSet_ThrowsException_ReceivingNullAsToken()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.RenderSet(null!));
        }

        [Test]
        public void RenderSet_RendersCorrectly()
        {
            _renderer.RenderSet(
                new SetToken(
                    new ItalicToken(
                        new TextToken("Set")),
                    new TextToken(" "),
                    new BoldToken(
                        new TextToken("of")),
                    new TextToken(" "),
                    new TextToken("tokens")));
            _renderer.ToString().Should().Be($"<em>Set</em> <strong>of</strong> tokens");
        }

        [Test]
        public void RenderText_ThrowsException_ReceivingNullAsToken()
        {
            Assert.Throws<ArgumentNullException>(() => _renderer.RenderText(null!));
        }

        [Test]
        public void RenderText_RendersCorrectly()
        {
            _renderer.RenderText(
                new TextToken(_dummyText));
            _renderer.ToString().Should().Be(_dummyText);
        }

        [Test]
        public void Render_ComplexTextCorrectly()
        {
            _renderer.RenderHeader(
                new HeaderToken(
                    new SetToken(
                        new ItalicToken(
                            new TextToken("Set")),
                        new TextToken(" "),
                        new BoldToken(
                            new TextToken("of")),
                        new TextToken(" "),
                        new TextToken("tokens"))));
            _renderer.ToString().Should().Be("<h1><em>Set</em> <strong>of</strong> tokens</h1>");
        }
    }
}
