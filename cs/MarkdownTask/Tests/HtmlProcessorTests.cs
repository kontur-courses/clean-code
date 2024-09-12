using FluentAssertions;
using MarkdownTask.HtmlTools;
using NUnit.Framework;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class HtmlProcessorTests
    {
        [Test]
        public void Process_WithStrongToken_ReturnsCorrectHtml()
        {
            var inputText = "a __bbb__ c";
            var tokens = new List<Token>
            {
                new Token(TagType.Strong, 2, Tag.Open, 2),
                new Token(TagType.Strong, 7, Tag.Close, 2)
            };

            var result = HtmlProcessor.Process(inputText, tokens);

            result.Should().Be(@"a <strong>bbb</strong> c");
        }

        [Test]
        public void Process_WithItalicToken_ReturnsCorrectHtml()
        {
            var inputText = "a _bbb_ c";
            var tokens = new List<Token>
            {
                new Token(TagType.Italic, 2, Tag.Open, 1),
                new Token(TagType.Italic, 6, Tag.Close, 1)
            };

            var result = HtmlProcessor.Process(inputText, tokens);

            result.Should().Be(@"a <em>bbb</em> c");
        }

        [Test]
        public void Process_WithHeaderToken_ReturnsCorrectHtml()
        {
            var inputText = "# a b c";
            var tokens = new List<Token>
            {
                new Token(TagType.Header, 0, Tag.Open, 2),
                new Token(TagType.Header, 7, Tag.Close, 0)
            };

            var result = HtmlProcessor.Process(inputText, tokens);

            result.Should().Be(@"<h1>a b c</h1>");
        }

        [Test]
        public void Process_WithLinkToken_ReturnsCorrectHtml()
        {
            var inputText = "a [b](c) d";
            var tokens = new List<Token>
            {
                new Token(TagType.Link, 2, Tag.Open, 6),
                new Token(TagType.Link, 7, Tag.Close, 0)
            };

            var result = HtmlProcessor.Process(inputText, tokens);

            result.Should().Be(@"a <a href=b>c</a> d");
        }
    }
}