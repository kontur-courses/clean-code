using FluentAssertions;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class EscapeSymbolTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BasicEscape()
        {
            Markdown.Markdown.Render(@"\# \\ \_ \_\_")
                .Should().Be(@"# \ _ __");
        }

        [Test]
        public void EscapeInMarkdown()
        {
            Markdown.Markdown.Render(@"#\# _a \_ b_ __a \_\_ b__")
                .Should().Be(@"<h1>\# <em>a _ b<\em> <strong>a __ b<\strong><\h1>");
        }
    }
}