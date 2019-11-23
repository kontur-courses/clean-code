using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.BasicTextTokenizer.Tests
{
    public class TokenReaderExtensionsTests
    {
        [Test]
        public void ReadUntilWithEscapeProcessing_ShouldReturnTwoTokens_OnTextWithEscapeSymbol()
        {
            const string text = "abf efk e!fg hij";
            bool IsStopPosition(string s, int i) => false;
            bool IsEscapePosition(string s, int i) => i < s.Length && s[i] == '!';
            var reader = new TokenReader(text);
            var expectedResult = new List<Token>
            {
                Token.CreateTextToken(0, "abf efk e".Length),
                Token.CreateTextToken("abf efk e!".Length, "fg hij".Length)
            };

            var result = reader.ReadUntilWithEscapeProcessing(IsStopPosition, IsEscapePosition);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadUntilWithEscapeProcessing_ShouldReturnManyToken_OnTextWithTwoEscapeSymbols()
        {
            const string text = "abf e!fk e!fg hij";
            bool IsStopPosition(string s, int i) => false;
            bool IsEscapePosition(string s, int i) => i < s.Length && s[i] == '!';
            var reader = new TokenReader(text);
            var expectedResult = new List<Token>
            {
                Token.CreateTextToken(0, "abf e".Length),
                Token.CreateTextToken("abf e!".Length, "fk e".Length),
                Token.CreateTextToken("abf e!fk e!".Length, "fg hij".Length)
            };

            var result = reader.ReadUntilWithEscapeProcessing(IsStopPosition, IsEscapePosition);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadUntilWithEscapeProcessing_ShouldReturnOneToken_OnTextWithNoEscapeSymbols()
        {
            const string text = "abf efk efg hij";
            bool IsStopPosition(string s, int i) => false;
            bool IsEscapePosition(string s, int i) => false;
            var reader = new TokenReader(text);
            var expectedResult = new List<Token> { Token.CreateTextToken(0, text.Length) };

            var result = reader.ReadUntilWithEscapeProcessing(IsStopPosition, IsEscapePosition);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
