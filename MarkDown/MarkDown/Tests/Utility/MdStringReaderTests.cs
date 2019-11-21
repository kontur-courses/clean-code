using FluentAssertions;
using NUnit.Framework;

namespace MarkDown.Tests.Utility
{
    [TestFixture]
    public class MdStringReaderTests
    {
        [TestCase("Hello", 0, 'H')]
        [TestCase("Hello", 3, 'l')]
        [TestCase("Hello", 4, 'o')]
        public void LookAhead_ReturnsCurrentCharacter(string line, int startPosition, char expected)
        {
            var reader = new MdStringReader(line);
            for(var i = 0; i < startPosition; i++)
                reader.ShiftPointer();
            reader.LookAhead().Should().Be(expected);
        }

        [TestCase("Hello", 0)]
        [TestCase("Hello", 3)]
        [TestCase("Hello", 4)]
        public void LookBehind_ReturnsPreviousCharacter(string line, int amountOfCheckings)
        {
            var reader = new MdStringReader(line);
            var currentChar = reader.LookAhead();
            for (var i = 0; i < amountOfCheckings; i++)
            {
                reader.ShiftPointer();
                reader.LookBehind().Should().Be(currentChar);
                currentChar = reader.LookAhead();
            }
        }

        [TestCase("Hello", 0, 1, 'H')]
        [TestCase("Hello", 3, 2, 'l', 'o')]
        [TestCase("Hello", 2, 3, 'l', 'l', 'o')]
        public void LookNextChars_ReturnsCorrectCharacters(string line, int startPosition, int length, params char[] expected)
        {
            var reader = new MdStringReader(line);
            for (var i = 0; i < startPosition; i++)
                reader.ShiftPointer();
            reader.LookNextChars(length).Should().BeEquivalentTo(expected);
        }

        [TestCase("Hello", 3, 3, 'H', 'e', 'l')]
        [TestCase("Hello", 5, 2, 'l', 'o')]
        [TestCase("Hello", 1, 1, 'H')]
        public void LookPreviousChars_ReturnsCorrectCharacters(string line, int startPosition, int length, params char[] expected)
        {
            var reader = new MdStringReader(line);
            for (var i = 0; i < startPosition; i++)
                reader.ShiftPointer();
            reader.LookPreviousChars(length).Should().BeEquivalentTo(expected);
        }
    }
}