using FluentAssertions;
using NUnit.Framework;

namespace Markdown.BasicTextTokenizer.Tests
{
    public class TokenReaderTests
    {
        [Test]
        public void ReadUntil_ShouldReturnTextBeforeStopSequence_OnTextWithStopSequence()
        {
            const string text = "abf efk efg hij";
            bool IsStopPosition(string s, int i) => i > 0
                                                    && i < text.Length - 2
                                                    && s.Substring(i - 1, 3) == "efg";
            var reader = new TokenReader(text);
            var expectedResult = Token.CreateTextToken(0, "abf efk e".Length);

            var result = reader.ReadUntil(IsStopPosition);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadUntil_ShouldReturnAllText_OnTextWithoutStopSequence()
        {
            const string text = "abf efk efg hij";
            bool IsStopPosition(string s, int i) => i > 0
                                                    && i < text.Length - 2
                                                    && s.Substring(i - 1, 3) == "afg";
            var reader = new TokenReader(text);
            var expectedResult = Token.CreateTextToken(0, text.Length);

            var result = reader.ReadUntil(IsStopPosition);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadWhile_ShouldReturnAcceptedText_OnTextWithAcceptedSymbols()
        {
            const string text = "aaaa gfhj";
            bool IsAccepted(char c) => c == 'a';
            var reader = new TokenReader(text);
            var expectedResult = Token.CreateTextToken(0, "aaaa".Length);

            var result = reader.ReadWhile(IsAccepted);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadWhile_ShouldReturnEmptyToken_OnTextWithoutAcceptedSymbols()
        {
            const string text = "aaaa gfhj";
            bool IsAccepted(char c) => c == 'z';
            var reader = new TokenReader(text);
            var expectedResult = Token.CreateTextToken(0, 0);

            var result = reader.ReadWhile(IsAccepted);

            result.Should().BeEquivalentTo(expectedResult);
        }

       [Test]
        public void ReadCount_ShouldReadTokenWithCountLength_OnText()
        {
            var text = "abcd";
            var reader = new TokenReader(text);
            var expectedResult = Token.CreateTextToken(0, 2);

            var result = reader.ReadCount(2);

            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void ReadCount_ShouldMovePositionOnCount_OnText()
        {
            var text = "abcd";
            var reader = new TokenReader(text);

            reader.ReadCount(2);

            reader.Position.Should().Be(2);
        }
    }
}
