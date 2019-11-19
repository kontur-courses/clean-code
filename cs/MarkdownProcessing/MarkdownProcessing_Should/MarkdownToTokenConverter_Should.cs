using FluentAssertions;
using MarkdownProcessing.Converters;
using MarkdownProcessing.Tokens;
using NUnit.Framework;

namespace MarkdownProcessing.MarkdownProcessing_Should
{
    public class MarkdownToTokenConverter_Should
    {
        [Test]
        public void TokenParser_ShouldHaveParentToken()
        {
            new MarkdownToTokenConverter("Hello").AllTokens.Pop().Type.Should().Be(TokenType.Parent);
        }

        [Test]
        public void TokenParser_SimplePhrase_ParentShouldHaveSimpleChild()
        {
            var parser = new MarkdownToTokenConverter("Hello");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.PlainText);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveComplicatedChild()
        {
            var parser = new MarkdownToTokenConverter("_Hello_");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.Italic);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveOnlyOneChild()
        {
            var parser = new MarkdownToTokenConverter("_Hello_");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseSimplePhrase()
        {
            var parser = new MarkdownToTokenConverter("Hello world");
            parser.ParseInputIntoTokens().Should().Be("<p>Hello world</p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseComplicatedPhrase()
        {
            var parser = new MarkdownToTokenConverter("_1_");
            parser.ParseInputIntoTokens().Should().Be("<p><em>1</em></p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseTwoComplicatedPhrases()
        {
            var parser = new MarkdownToTokenConverter("_Hello_ _world_");
            parser.ParseInputIntoTokens().Should().Be("<p><em>Hello</em> <em>world</em></p>");
        }

        [Test]
        public void TokenParser_TokensStack_ShouldContainOneTokenAtTheEnd()
        {
            var parser = new MarkdownToTokenConverter("_Hello world_");
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldSeeComplicatedTags()
        {
            var parser = new MarkdownToTokenConverter("__a__");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldRemainOneTokenAtTheEnd()
        {
            var parser = new MarkdownToTokenConverter("__a__");
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseCorrectly()
        {
            var parser = new MarkdownToTokenConverter("__a__");
            parser.ParseInputIntoTokens().Should().Be("<p><strong>a</strong></p>");
        }

        [TestCase("__a__ __b _c_ __", "<p><strong>a</strong> <strong>b <em>c</em> </strong></p>")]
        [TestCase("__a _b_ __", "<p><strong>a <em>b</em> </strong></p>")]
        [TestCase("___a_____b _a___", "<p><strong><em>a</em></strong><strong>b <em>a</em></strong></p>")]
        [TestCase("___a______b___", "<p><strong><em>a</em></strong><strong><em>b</em></strong></p>")]
        public void TokenParser_ParseInnerToken_ShouldParseCorrectly(string input, string expected)
        {
            var parser = new MarkdownToTokenConverter(input);
            parser.ParseInputIntoTokens().Should()
                .Be(expected);
        }
    }
}