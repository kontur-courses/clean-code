using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
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
                new TokenHeader1(), TokenText.FromText("text")
            }).SetName("header and text");

            yield return new TestCaseData("#text", new List<IToken>
            { 
                TokenText.FromText("#text")
            }).SetName("wrong header and text");

            yield return new TestCaseData("text# text", new List<IToken>
            { 
                TokenText.FromText("text"), Token.Header1, Token.FromText("text")
            }).SetName("wrong header and text");
            
            yield return new TestCaseData("# _\\_text_", new List<IToken>
            {
                new TokenHeader1(), new TokenItalics(),
                new TokenEscape(), new TokenItalics(),
                TokenText.FromText("text"), new TokenItalics()
            }).SetName("Composite of different markup symbols");

            yield return new TestCaseData("# __text___text_", new List<IToken>
            {
                new TokenHeader1(), new TokenStrong(), TokenText.FromText("text"), new TokenStrong(),
                new TokenItalics(), TokenText.FromText("text"), new TokenItalics()
            }).SetName("Strong is close to italics");

            yield return new TestCaseData("_\\\\_", new List<IToken>
            {
                Token.Italics, Token.Escape, Token.Escape, Token.Italics
            }).SetName("Parse escape");
            
            
            yield return new TestCaseData("[text](link)", new List<IToken>
            {
                Token.OpenSquareBracket, Token.FromText("text"), Token.CloseSquareBracket, 
                Token.OpenBracket, Token.FromText("link"), Token.CloseBracket
            }).SetName("Parse escape");
        }
    }
}