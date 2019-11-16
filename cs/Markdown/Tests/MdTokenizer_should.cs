using System;
using System.Linq;
using FluentAssertions;
using Markdown.MdTokens;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MdTokenizer_should
    {
        private MdTokenizer tokenizer;

        [SetUp]
        public void SetUp()
        {
            tokenizer = new MdTokenizer();
        }

        [Test]
        public void MakeTokens_Should_ThrowArgumentNullException_When_ArgumentIsNull()
        {
            Following.Code(() => tokenizer.MakeTokens(null).First()).Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void MakeTokens_Should_ThrowArgumentException_When_EmptyText()
        {
            Following.Code(() => tokenizer.MakeTokens("").First()).Should().Throw<ArgumentException>();
        }

        [TestCase("text", "text", "NONE", "NONE", TestName = "when no special symbols found")]
        [TestCase("_text_", "text", "_", "_", TestName = "when text surrounded with emphasis")]
        [TestCase("__text__", "text", "__", "__", TestName = "when text surrounded with bold symbols")]
        [TestCase(@"\_text\_", @"_text_", @"\", @"\", TestName = "when text surrounded with shielded emphasis")]
        [TestCase("ipsumdolor_sit_23_", "ipsumdolor_sit_23", "NONE", "_", TestName = "when text with emphasis and numbers")]
        [TestCase("text_", "text", "NONE", "_", TestName = "when text with singular emphasis and the end")]
        [TestCase("_text", "text", "_", "NONE", TestName = "when text with singular emphasis and the beginning")]
        [TestCase("text__", "text", "NONE", "__", TestName = "when text with singular bold symbol and the end")]
        [TestCase("__text", "text", "__", "NONE", TestName = "when text with singular bold symbol and the beginning")]
        [TestCase("b_aa__a_c", "b_aa__a_c", "NONE", "NONE", TestName = "when text with incorrect special symbols")]
        [TestCase("__", "__", "NONE", "NONE", TestName = "when text with only special symbols")]
        public void MakeTokens_Should_ParseToken(string text, 
            string expectedContent, 
            string expectedSpecialSymbolBeginning, 
            string expectedSpecialSymbolEnding)
        {
            var actualToken = (MdToken)tokenizer.MakeTokens(text).ToList().First(); 
            var expectedToken = new MdToken(expectedContent, expectedSpecialSymbolBeginning, expectedSpecialSymbolEnding);
            actualToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void MakeTokens_Should_ReturnSeveralTokens_When_TextWithSpaces()
        {
            tokenizer.MakeTokens("abc abc").Count().Should().Be(2);
        }
        
        [Test]
        public void MakeTokens_Should_ReturnSeveralTokensWithEmphasisToken_When_TextWithSpacesAndEmphasis()
        {
            tokenizer.MakeTokens("abc     abc").Count().Should().Be(2);
        }
        
        [Test]
        public void MakeTokens_Should_ReturnSeveralEmptyTokens_When_TextWithSpacesAndNoSpecialSymbols()
        {
            var tokens = tokenizer.MakeTokens("abc abc").ToList();
            var firstToken = tokens.First();
            var expectedToken = new MdToken("abc", "NONE", "NONE");
            firstToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void MakeTokens_Should_ReturnSeveralEmptyTokens_When_TextWithMultipleSpacesAndNoSpecialSymbols()
        {
            var tokens = tokenizer.MakeTokens("abc      bc").ToList();
            var firstToken = tokens.First();
            var expectedToken = new MdToken("abc", "NONE", "NONE");
            firstToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void MakeTokens_Should_ReturnSeveralTokensWithEmphasis_When_TextWithSeveralTokensWithEmphasis()
        {
            var tokens = tokenizer.MakeTokens("_abc_ _abc_").ToList();
            var lastToken = tokens.Last();
            var expectedToken = new MdToken("abc", "_", "_");
            lastToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void MakeTokens_Should_ReturnSeveralDifferentTokens_When_TextWithSeveralTokensWithSymbols()
        {
            var tokens = tokenizer.MakeTokens("_abc_ _abc_ __ade__").ToList();
            var lastToken = tokens.Last();
            var expectedToken = new MdToken("ade", "__", "__");
            lastToken.Should().BeEquivalentTo(expectedToken);
        }
        
        [Test]
        public void MakeTokens_Should_ParseNestedTokens_When_TextWithNestedTokens()
        {
            var tokens = tokenizer.MakeTokens("__Lorem _ipsum_ dolor sit amet__").ToList();
            var firstToken = tokens[0];
            var secondToken = tokens[1];
            var lastToken = tokens.Last();
            var expectedFirstToken = new MdToken("Lorem", "__", "NONE");
            firstToken.Should().BeEquivalentTo(expectedFirstToken);
            var expectedSecondToken = new MdToken("ipsum", "_", "_");
            secondToken.Should().BeEquivalentTo(expectedSecondToken);
            var expectedLastToken = new MdToken("amet", "NONE", "__");
            lastToken.Should().BeEquivalentTo(expectedLastToken);
        }
    }

    public static class Following
    {
        public static Action Code(Action action)
        {
            return action;
        }
    }
}