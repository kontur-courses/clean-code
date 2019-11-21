using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        [TestCase(@"\_text\_", @"_text_", @"NONE", @"NONE", TestName = "when text surrounded with shielded emphasis")]
        [TestCase(@"\_text_", @"_text_", @"NONE", @"NONE", TestName = "when text with shielded emphasis at the beginning")]
        [TestCase(@"_text\_", @"_text_", @"NONE", @"NONE", TestName = "when text with shielded emphasis at the end")]
        [TestCase(@"\__text\__", @"__text__", @"NONE", @"NONE", TestName = "when text surrounded with shielded bold symbols")] 
        [TestCase(@"\__text__", @"__text__", @"NONE", @"NONE", TestName = "when text with shielded bold symbols at the beginning")]
        [TestCase(@"__text\__", @"__text__", @"NONE", @"NONE", TestName = "when text with shielded bold symbols at the end")]
        [TestCase("ipsumdolor_sit_23_", "ipsumdolor_sit_23_", "NONE", "NONE", TestName = "when text with emphasis and numbers")]
        [TestCase("text_", "text_", "NONE", "NONE", TestName = "when text with singular emphasis at the end")]
        [TestCase("_text", "_text", "NONE", "NONE", TestName = "when text with singular emphasis at the beginning")]
        [TestCase("__text_", "__text_", "NONE", "NONE", TestName = "when text with emphasis at the end and bold symbols at the beginning")]
        [TestCase("_text__", "_text__", "NONE", "NONE", TestName = "when text with bold symbols at the end and emphasis at the beginning")]
        [TestCase("text__", "text__", "NONE", "NONE", TestName = "when text with singular bold symbol at the end")]
        [TestCase("__text", "__text", "NONE", "NONE", TestName = "when text with singular bold symbol at the beginning")]
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

        [Test] public void MakeTokens_Should_IgnoreNesting_When_NestedTokenCantBeNested()
        {
            var tokens = tokenizer.MakeTokens("_Lorem __ipsum__ dolor sit amet_").ToList();
            var firstToken = tokens[0];
            var secondToken = tokens[1];
            var lastToken = tokens.Last();
            var expectedFirstToken = new MdToken("Lorem", "_", "NONE");
            firstToken.Should().BeEquivalentTo(expectedFirstToken);
            var expectedSecondToken = new MdToken("__ipsum__", "NONE", "NONE");
            secondToken.Should().BeEquivalentTo(expectedSecondToken);
            var expectedLastToken = new MdToken("amet", "NONE", "_");
            lastToken.Should().BeEquivalentTo(expectedLastToken);
        }
        
        [Test]
        public void MakeTokens_Should_ParseIncorrectEmphasis_When_TextWithNestedTokens()
        {
            var tokens = tokenizer.MakeTokens("__Lorem _ipsum dolor sit amet__").ToList();
            var firstToken = tokens[0];
            var secondToken = tokens[1];
            var lastToken = tokens.Last();
            var expectedFirstToken = new MdToken("Lorem", "__", "NONE");
            firstToken.Should().BeEquivalentTo(expectedFirstToken);
            var expectedSecondToken = new MdToken("_ipsum", "NONE", "NONE");
            secondToken.Should().BeEquivalentTo(expectedSecondToken);
            var expectedLastToken = new MdToken("amet", "NONE", "__");
            lastToken.Should().BeEquivalentTo(expectedLastToken);
        }

        [Test]
        public void MakeTokens_Should_WorkLinearly()
        {
            var strBuilder = new StringBuilder();
            for (var i = 0; i < 160000; i++)
                strBuilder.Append(" " + Guid.NewGuid().ToString("N"));
            var builderRes = strBuilder.ToString();
            var timer = Stopwatch.StartNew();
            tokenizer.MakeTokens(builderRes).Last();
            timer.Stop();
            timer.Elapsed.TotalMilliseconds.Should().BeLessThan(1000);
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