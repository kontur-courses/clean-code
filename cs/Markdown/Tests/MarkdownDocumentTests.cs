using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownDocumentTests
    {
        private readonly MarkdownParser parser = new MarkdownParser();

        [Test]
        public void ToHtml_ShouldNotWrap_WhenPlainText()
        {
            var markdownDocument = parser.Parse(" a ab cde  ");

            markdownDocument.ToHtml().Should().BeEquivalentTo(" a ab cde  ");
        }

        [TestCase("_", TestName = "Em tag escaped")]
        [TestCase("__", TestName = "Strong tag escaped")]
        public void ToHtml_ShouldNotWrap_WhenEscaped(string mdTag)
        {
            var markdownDocument = parser.Parse($"\\{mdTag}ba cf\\{mdTag}");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"{mdTag}ba cf{mdTag}");
        }

        [Test]
        public void ToHtml_ShouldNotWrap_WhenStrongInsideEm()
        {
            var markdownDocument = parser.Parse("_aa __bb bb__ aa_");

            markdownDocument.ToHtml().Should().BeEquivalentTo("<em>aa __bb bb__ aa</em>");
        }

        [TestCase("_", "em", TestName = "Em tag")]
        [TestCase("__", "strong", TestName = "Strong tag")]
        public void ToHtml_ShouldWrap_WhenSingleNotNested(string mdTag, string htmlTag)
        {
            var markdownDocument = parser.Parse($"{mdTag}ba cf{mdTag}");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"<{htmlTag}>ba cf</{htmlTag}>");
        }

        [TestCase("_", "em", TestName = "Em tags")]
        [TestCase("__", "strong", TestName = "Strong tags")]
        public void ToHtml_ShouldWrap_WhenMultipleNotNested(string mdTag, string htmlTag)
        {
            var markdownDocument = parser.Parse($"{mdTag}cf{mdTag} {mdTag}ba{mdTag}");

            markdownDocument.ToHtml()
                .Should().BeEquivalentTo($"<{htmlTag}>cf</{htmlTag}> <{htmlTag}>ba</{htmlTag}>");
        }

        [Test]
        public void ToHtml_ShouldWrap_WhenMultipleNotNestedStrongAndEmTags()
        {
            var markdownDocument = parser.Parse($"_c c_ __b b__");

            markdownDocument.ToHtml()
                .Should().BeEquivalentTo($"<em>c c</em> <strong>b b</strong>");
        }

        [Test]
        public void ToHtml_ShouldWrap_WhenEmInsideStrong()
        {
            var markdownDocument = parser.Parse("__a _b b_ a__");

            markdownDocument.ToHtml().Should().BeEquivalentTo("<strong>a <em>b b</em> a</strong>");
        }
    }
}