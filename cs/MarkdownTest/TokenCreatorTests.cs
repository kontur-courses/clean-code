using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using Markdown.Engine.Tokens;
using NUnit.Framework;

namespace MarkdownTest
{
    public class TokenCreatorTests
    {
        private TokenCreator tokenCreator;

        [SetUp]
        public void Setup()
        {
            tokenCreator = new TokenCreator();
        }

        [Test]
        public void Create_ShouldThrowArgumentException_OnNull()
        {
            FluentActions.Invoking(
                    () => tokenCreator.Create(null))
                .Should().Throw<ArgumentException>();
        }

        [TestCaseSource(nameof(TokenCreatorTestCaseData))]
        public void Create_ShouldReturnCorrectTokens_When(string text, IEnumerable<IToken> expected)
        {
            var actual = tokenCreator.Create(text);

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> TokenCreatorTestCaseData()
        {
            yield return new TestCaseData("text", new List<IToken>
            {
                TokenText.FromText("text")
            }).SetName("No markup symbols");

            yield return new TestCaseData("# text", new List<IToken>
            {
                Token.Header1, TokenText.FromText("text")
            }).SetName("Header and text");

            yield return new TestCaseData("#text", new List<IToken>
            { 
                TokenText.FromText("#text")
            }).SetName("Wrong header and text");

            yield return new TestCaseData("text# text", new List<IToken>
            { 
                TokenText.FromText("text"), Token.Header1, TokenText.FromText("text")
            }).SetName("Wrong header and text");
            
            yield return new TestCaseData("# _\\_text_", new List<IToken>
            {
                Token.Header1, Token.Italics, Token.Escape,
                Token.Italics, TokenText.FromText("text"), Token.Italics
            }).SetName("Composite of different markup symbols");

            yield return new TestCaseData("# __text___text_", new List<IToken>
            {
                Token.Header1, Token.Strong, TokenText.FromText("text"), Token.Strong,
                Token.Italics, TokenText.FromText("text"), Token.Italics
            }).SetName("Strong is close to italics");

            yield return new TestCaseData("_\\\\_", new List<IToken>
            {
                Token.Italics, Token.Escape, Token.Escape, Token.Italics
            }).SetName("Escape");

            yield return new TestCaseData("text text", new List<IToken>
            {
                TokenText.FromText("text"), Token.WhiteSpace, TokenText.FromText("text")
            }).SetName("Whitespace");

            yield return new TestCaseData("[text](link)", new List<IToken>
            {
                Token.OpenSquareBracket, TokenText.FromText("text"), Token.CloseSquareBracket, 
                Token.OpenBracket, TokenText.FromText("link"), Token.CloseBracket
            }).SetName("Link");
        }
    }
}