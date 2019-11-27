using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownDocumentTests
    {
        private readonly MarkdownParser parser = new MarkdownParser(new TokenInfo());

        [Test]
        public void ToHtml_ShouldNotWrap_WhenPlainText()
        {
            var markdownDocument = parser.Parse(" a ab cde  ");

            markdownDocument.ToHtml().Should().BeEquivalentTo(" a ab cde  ");
        }

        [TestCase("_", TestName = "Em tag escaped")]
        [TestCase("__", TestName = "Strong tag escaped")]
        [TestCase("`", TestName = "Code tag escaped")]
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

        [Test]
        public void ToHtml_ShouldNotWrap_WhenSpaceAfterOpenTag()
        {
            var markdownDocument = parser.Parse("__ aa aa__");

            markdownDocument.ToHtml().Should().BeEquivalentTo("__ aa aa__");
        }

        [Test]
        public void ToHtml_ShouldNotWrap_WhenSpaceBeforeCloseTag()
        {
            var markdownDocument = parser.Parse("__aa aa __");

            markdownDocument.ToHtml().Should().BeEquivalentTo("__aa aa __");
        }

        [Test]
        public void ToHtml_ShouldNotWrap_WhenTagsInsideText()
        {
            var markdownDocument = parser.Parse("a_a_a");

            markdownDocument.ToHtml().Should().BeEquivalentTo("a_a_a");
        }

        [TestCase("_", "em", TestName = "Em tag")]
        [TestCase("__", "strong", TestName = "Strong tag")]
        [TestCase("`", "code", TestName = "Code tag")]
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

            markdownDocument
                .ToHtml()
                .Should()
                .BeEquivalentTo($"<{htmlTag}>cf</{htmlTag}> <{htmlTag}>ba</{htmlTag}>");
        }

        [Test]
        public void ToHtml_ShouldWrap_WhenMultipleNotNestedStrongAndEmTags()
        {
            var markdownDocument = parser.Parse($"_c c_ __b b__");

            markdownDocument
                .ToHtml()
                .Should()
                .BeEquivalentTo($"<em>c c</em> <strong>b b</strong>");
        }

        [Test]
        public void ToHtml_ShouldWrap_WhenEmInsideStrong()
        {
            var markdownDocument = parser.Parse("__a _b b_ a__");

            markdownDocument.ToHtml().Should().BeEquivalentTo("<strong>a <em>b b</em> a</strong>");
        }

        [TestCase("#", "h1", TestName = "First level header")]
        [TestCase("##", "h2", TestName = "Second level header")]
        [TestCase("###", "h3", TestName = "Third level header")]
        [TestCase("####", "h4", TestName = "Fourth level header")]
        [TestCase("#####", "h5", TestName = "Fifth level header")]
        public void ToHtml_ShouldWrap_WhenHeader(string tag, string htmlTag)
        {
            var markdownDocument = parser.Parse($"{tag} header\n");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"<{htmlTag}>header</{htmlTag}>");
        }

        [TestCase(">", "\n\n", TestName = "Blockquote without new line")]
        [TestCase("#", "\n\n", TestName = "Header without new line")]
        [TestCase("-", "\n\n", TestName = "List without new line")]
        public void ToHtml_ShouldNotWrap_WhenNoNewLine(string openTag, string closeTag)
        {
            var markdownDocument = parser.Parse($"should {openTag} not parse{closeTag}");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"should {openTag} not parse");
        }

        [TestCase(">", "\n\n", TestName = "Blockquote no space after open tag")]
        [TestCase("#", "\n", TestName = "Header no space after open tag")]
        [TestCase("-", "\n", TestName = "List no space after open tag")]
        public void ToHtml_ShouldNotWrap_WhenNoSpaceAfterOpenTag(string openTag, string closeTag)
        {
            var markdownDocument = parser.Parse($"{openTag}should not parse{closeTag}");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"{openTag}should not parse");
        }

        [TestCase(">", "\n\n", "blockquote", TestName = "Blockquote ignores start spaces")]
        [TestCase("#", "\n", "h1", TestName = "Header ignores start spaces")]
        public void ToHtml_ShouldIgnoreSpacesAfterOpenTag(string openTag, string closeTag, string htmlTag)
        {
            var markdownDocument = parser.Parse($"{openTag}      data{closeTag}");

            markdownDocument.ToHtml().Should().BeEquivalentTo($"<{htmlTag}>data</{htmlTag}>");
        }

        [TestCase("code", "pre", "m\nn", "```m\nn```", TestName = "Multiline code")]
        [TestCase("li", "ul", "l l", "- l l\n", TestName = "List")]
        public void ToHtml_ShouldExtraWrap(string htmlTag, string wrapTag, string data, string markdown)
        {
            var markdownDocument = parser.Parse(markdown);

            markdownDocument
                .ToHtml()
                .Should()
                .BeEquivalentTo($"<{wrapTag}><{htmlTag}>{data}</{htmlTag}></{wrapTag}>");
        }
    }
}