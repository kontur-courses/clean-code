using FluentAssertions;
using NUnit.Framework;

namespace MarkdownProcessing.MarkdownProcessing_Should
{
    public class TokenParser_Should
    {
        [Test]
        public void TokenParser_DictionaryShouldWorkOnStringKeys()
        {
            var parser = new TokenParser("_Hello_");
            parser.possibleTags.ContainsKey("_").Should().BeTrue();
        }

        [Test]
        public void TokenParser_ShouldHaveParentToken()
        {
            new TokenParser("Hello").AllTokens.Pop().Type.Should().Be(TokenType.Parent);
        }

        [Test]
        public void TokenParser_SimplePhrase_ParentShouldHaveSimpleChild()
        {
            var parser = new TokenParser("Hello");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.PlainText);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveComplicatedChild()
        {
            var parser = new TokenParser("_Hello_");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens[0].Type.Should().Be(TokenType.Italic);
        }

        [Test]
        public void TokenParser_ComplicatedPhrase_ParentShouldHaveOnlyOneChild()
        {
            var parser = new TokenParser("_Hello_");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseSimplePhrase()
        {
            var parser = new TokenParser("Hello world");
            parser.ParseInputIntoTokens().Should().Be("<p>Hello world</p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseComplicatedPhrase()
        {
            var parser = new TokenParser("_1_");
            parser.ParseInputIntoTokens().Should().Be("<p><em>1</em></p>");
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseTwoComplicatedPhrases()
        {
            var parser = new TokenParser("_Hello_ _world_");
            parser.ParseInputIntoTokens().Should().Be("<p><em>Hello</em> <em>world</em></p>");
        }

        [Test]
        public void TokenParser_TokensStack_ShouldContainOneTokenAtTheEnd()
        {
            var parser = new TokenParser("_Hello world_");
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldSeeComplicatedTags()
        {
            var parser = new TokenParser("__a__");
            parser.ParseInputIntoTokens();
            var parent = parser.AllTokens.Peek() as ComplicatedToken;
            parent?.ChildTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldRemainOneTokenAtTheEnd()
        {
            var parser = new TokenParser("__a__");
            parser.ParseInputIntoTokens();
            parser.AllTokens.Count.Should().Be(1);
        }

        [Test]
        public void TokenParser_ParseToken_ShouldParseCorrectly()
        {
            var parser = new TokenParser("__a__");
            parser.ParseInputIntoTokens().Should().Be("<p><strong>a</strong></p>");
        }
    }
}