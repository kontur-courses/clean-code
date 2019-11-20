using FluentAssertions;
using MarkdownProcessing.Converters;
using MarkdownProcessing.Markdowns;
using MarkdownProcessing.Tokens;
using NUnit.Framework;

namespace MarkdownProcessing.MarkdownProcessing_Should
{
    public class MarkdownToTokenConverter_Should
    {
        [Test]
        public void TokenParser_ShouldHaveParentToken()
        {
            new MarkdownToTokenConverter("Hello", new HtmlMarkdown()).AllTokens.Pop().Type.Should()
                .Be(TokenType.Parent);
        }

        [Test]
        public void TokenParser_SimplePhrase_ParentShouldHaveSimpleChild()
        {
            var parser = new MarkdownToTokenConverter("Hello", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.PlainText);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveComplicatedChild()
        {
            var parser = new MarkdownToTokenConverter("_Hello_", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.Italic);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveOnlyOneChild()
        {
            var parser = new MarkdownToTokenConverter("_Hello_", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseSimplePhrase()
        {
            var parser = new MarkdownToTokenConverter("Hello world", new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should().Be("<p>Hello world</p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseComplicatedPhrase()
        {
            var parser = new MarkdownToTokenConverter("_1_", new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should().Be("<p><em>1</em></p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseTwoComplicatedPhrases()
        {
            var parser = new MarkdownToTokenConverter("_Hello_ _world_", new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should().Be("<p><em>Hello</em> <em>world</em></p>");
        }

        [Test]
        public void TokenParser_TokensStack_ShouldContainOneTokenAtTheEnd()
        {
            var parser = new MarkdownToTokenConverter("_Hello world_", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldSeeComplicatedTags()
        {
            var parser = new MarkdownToTokenConverter("__a__", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldRemainOneTokenAtTheEnd()
        {
            var parser = new MarkdownToTokenConverter("__a__", new HtmlMarkdown());
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseCorrectly()
        {
            var parser = new MarkdownToTokenConverter("__a__", new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should().Be("<p><strong>a</strong></p>");
        }

        [TestCase("__a__ __b _c_ __", "<p><strong>a</strong> <strong>b <em>c</em> </strong></p>")]
        [TestCase("__a _b_ __", "<p><strong>a <em>b</em> </strong></p>")]
        [TestCase("___a_____b _a___", "<p><strong><em>a</em></strong><strong>b <em>a</em></strong></p>")]
        [TestCase("___a______b___", "<p><strong><em>a</em></strong><strong><em>b</em></strong></p>")]
        [TestCase("_a__b__a_", "<p><em>a</em><em>b</em><em>a</em></p>")]
        public void TokenParser_ParseInnerToken_ShouldParseCorrectly(string input, string expected)
        {
            var parser = new MarkdownToTokenConverter(input, new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should()
                .Be(expected);
        }

        [TestCase(@"\__a_", "<xml>_<italic>a</italic></xml>")]
        [TestCase(@"\\_a_", "<xml>\\<italic>a</italic></xml>")]
        [TestCase(@"\\\\\\\__a_", "<xml>\\\\\\_<italic>a</italic></xml>")]
        [TestCase(@"\\\\\\\\\\\__a_", "<xml>\\\\\\\\\\_<italic>a</italic></xml>")]
        public void TokenParser_ParseToken_ShouldIgnoreScreenedSymbols(string input, string expected)
        {
            var parser = new MarkdownToTokenConverter(input, new XmlMarkdown());
            parser.ParseInputIntoTokens().Should()
                .Be(expected);
        }

        [Test, Timeout(100)]
        public void TokenParser_ParseToken_ShouldBeFast()
        {
            var parser = new MarkdownToTokenConverter("___a______b___", new HtmlMarkdown());
            parser.ParseInputIntoTokens().Should()
                .Be("<p><strong><em>a</em></strong><strong><em>b</em></strong></p>");
        }
    }
}