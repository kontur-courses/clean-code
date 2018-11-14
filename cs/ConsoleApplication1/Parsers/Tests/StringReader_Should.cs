using NUnit.Framework;
using System.Text;
using FluentAssertions;
using System;
using ConsoleApplication1.Extensions;

namespace ConsoleApplication1.Parsers.Tests
{
    [TestFixture]
    public class StringReader_Should
    {
        [TestCase("a", TestName = "string's length is zero")]
        [TestCase("ab", TestName = "string's length is not-zero")]
        [TestCase("askdjsakldjakldjasklsdjakldajkladjaskl", TestName = "string's length is quite big")]
        [TestCase("", TestName = "string is empty")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaa", TestName = "string consists of the same symbols")]
        [TestCase("       \n \t ", TestName = "string consists of white-spaces")]
        [TestCase("133899389238932", TestName = "string consists of digits")]
        public void ReadNextSymbol_CanBeExecutedMinimumLengthOfTransmittedTextTimes(string text)
        {
            var reader = new StringReader(text);
            var textLength = text.Length;

            Assert.DoesNotThrow(() => ReadSymbols(reader, textLength));
        }
        

        private string ReadSymbols(StringReader reader, int countOperations)
        {
            var text = new StringBuilder();
            for (var index = 0; index < countOperations; index++)
                text.Append(reader.ReadNextSymbol());
            return text.ToString();
        }
        
        [TestCase("a", TestName = "text consists of one letter")]
        [TestCase("", TestName = "transimitted text is empty")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaa", TestName = "text consists only one repeated symbol")]
        [TestCase("askdjskdasjdklasjdklasjdkljsdklajsakl", TestName = "text length is quiet big")]
        public void ReadNextSymbol_ThrowsInvalidOperationException_WhenReadNextSymbolExecuterLengthOfTransmittedTextsTimes(string text)
        {
            var reader = new StringReader(text);
            var textLength = text.Length;

            ReadSymbols(reader, textLength);
            Assert.Throws<InvalidOperationException>(() => reader.ReadNextSymbol());
        }

        

        [TestCase("a", TestName = "text length is one")]
        [TestCase("", TestName = "text is empty")]
        [TestCase("djssksdsdjsds", TestName = "length of text is bigger than one")]
        [TestCase("29820982902", TestName = "text consists of digits")]
        [TestCase("\n \t", TestName = "text consists of white-spaces")]
        public void GetNextSymbol_ReturnsTransmittedTextInTotal_WhenExecutedTillAnySymbolsReturnsFalse(string text)
        {
            var reader = new StringReader(text);

            var readText = reader.ReadToEnd();
            readText.Should().Be(text);
        }
        
        [Test]
        public void GetNextSymbol_ReturnsTransmittedTextInTotal_WhenSendVeryBigString()
        {
            var textLength = 100000;
            var text = GenerateRandomString(textLength);
            var reader = new StringReader(text);

            var readerText = reader.ReadToEnd();

            readerText.Should().Be(text);
        }

        [TestCase("aaaa", TestName = "string has positive length")]
        [TestCase("", TestName = "string is empty")]
        public void AnySymbols_ReturnsFalse_WhenReadNextSymbolExecutedLengthOfTransmittedTextTimes(string text)
        {
            var reader = new StringReader(text);
            var textLength = text.Length;

            ReadSymbols(reader, textLength);

            reader.AnySymbols().Should().BeFalse();
        }

        private string GenerateRandomString(int stringLength)
        {
            var text = new StringBuilder();
            for (var index = 0; index < stringLength; index++)
                text.Append((char)(index % 128));
            return text.ToString();
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfAString()
        {
            Assert.Throws<ArgumentException>(() => new StringReader(null));
        }

        [Test]
        public void ReadNextSymbol_ReturnsTransmittedStringInTotalAtFirstReader_WhenCreatedSecondStringReader()
        {
            var text = "aaaab";
            var reader = new StringReader(text);
            var secondReader = new StringReader("sdjsd");

            reader.ReadToEnd().Should().Be(text);
        }

        [Test]
        public void ReadNextSymbol_ReturnsTransmittedStringInTotalAtSecondReader_WhenCreatedTwoReaders()
        {
            var text = "sdjsd";
            var reader = new StringReader("aaaab");
            var secondReader = new StringReader(text);

            secondReader.ReadToEnd().Should().Be(text);
        }

        [Test]
        public void ReadNextSymbol_ThrowsException_WhenAfterReadingAllSymbolsCreatedSecondStringReader()
        {
            var text = "aaaab";
            var reader = new StringReader(text);
            reader.ReadToEnd();
            var secondReader = new StringReader("sdjsd");

            Assert.Throws<InvalidOperationException>(() => reader.ReadNextSymbol());
        }

        [Test, Timeout(1000)]
        public void ReadNextSymbol_WorksFast_WhenExecutedManyTimes()
        {
            const int textLength = 100000;
            var text = GenerateRandomString(textLength);
            var reader = new StringReader(text);

            reader.ReadToEnd();
        }

    }
}
