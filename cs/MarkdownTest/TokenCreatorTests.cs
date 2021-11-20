using System;
using System.Collections.Generic;
using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTest
{
    public class TokenCreatorTests
    {
        private TokenCreator sut;
        
        [SetUp]
        public void Setup()
        {
            sut = new TokenCreator();
        }

        [Test]
        public void Create_ShouldThrowArgumentException_OnNull()
        {
            FluentActions.Invoking(
                () => sut.Create(null))
                .Should().Throw<ArgumentException>();
        }

        [TestCaseSource(nameof(TokenCreatorTestCaseData))]
        public void Create_ShouldReturnCorrectTokens_When(string text, IEnumerable<Token> expected)
        {
            var actual = sut.Create(text);

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> TokenCreatorTestCaseData()
        {
            yield return new TestCaseData("text", new List<Token>
            {
                new(TokenType.Text, "text")
            }).SetName("No markup symbols");
            
            yield return new TestCaseData("#text", new List<Token>
            {
                new(TokenType.Header1, "#"), new(TokenType.Text, "text")
            }).SetName("# symbol and text");
            
            yield return new TestCaseData("text #text", new List<Token>
            {
                new(TokenType.Text, "text #text")
            }).SetName("# symbol not in beginning");
            
            yield return new TestCaseData("text ##text", new List<Token>
            {
                new(TokenType.Text, "text ##text")
            }).SetName("Two # symbols");

            yield return new TestCaseData("#_\\_text_", new List<Token>
            {
                new(TokenType.Header1, "#"), new(TokenType.Italics, "_"), 
                new(TokenType.Escape, "\\"), new(TokenType.Italics, "_"),
                new(TokenType.Text, "text"), new(TokenType.Italics, "_")
            }).SetName("Composite of different markup symbols");
            
            yield return new TestCaseData("#__text1___text2_", new List<Token>
            {
                new(TokenType.Header1, "#"), new(TokenType.Strong, "__"), 
                new(TokenType.Text, "text1"), new(TokenType.Strong, "__"), 
                new(TokenType.Italics, "_"), new(TokenType.Text, "text2"),
                new(TokenType.Italics, "_")
            }).SetName("Symbol __ is close to _");
        }
    }
}