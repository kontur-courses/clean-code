using NUnit.Framework;
using System;
using FluentAssertions;
using ConsoleApplication1.Extensions;
using System.Linq;
using ConsoleApplication1.UsefulEnums;

namespace ConsoleApplication1.Parsers.Tests
{
    [TestFixture]
    public class TextParser_Should
    {
        private readonly char[] baseParseSymbols = new [] { '_' };

        private TextParser GetCommonParser(String text)
        {
            var reader = new StringReader(text);
            return new TextParser(reader, Array.AsReadOnly(baseParseSymbols), true);

        }

        [Test]
        public void GetNextPart_ReturnsEmptyEndItem_WhenGetsEmptyReader()
        {
            var parserText = "";
            var parser = GetCommonParser(parserText);
            var expectedNextPart = new TextPart("", TextType.End);

            parser.GetNextPart()
                .Should()
                .BeEquivalentTo(expectedNextPart);
        }

        [Test]
        public void GetNextPart_ThrowsException_WhenGetsEndItemTwice()
        {
            var parserText = "";
            var parser = GetCommonParser(parserText);
            parser.GetNextPart();

            Assert.Throws<InvalidOperationException>(() => parser.GetNextPart());
        }

        [TestCase("aaaaa", "aaaaa", TextType.SimpleText, TestName = "text is repeated many times letter")]
        [TestCase("     ", "     ", TextType.WhiteSpaces, TestName = "text is spaces")]
        [TestCase(@"_____", @"_____", TextType.SpecialSymbols, TestName = "text is special symbols")]
        [TestCase(" \t\r", " \t\r", TextType.WhiteSpaces, TestName = "text with different white-spaces characters")]
        [TestCase("qwertyuio", "qwertyuio", TextType.SimpleText, TestName = "text is different letters")]
        [TestCase("aaaaaa     ", "aaaaaa", TextType.SimpleText, TestName = "text contains simple text and white-spaces then")]
        [TestCase(@"aaaa\a", "aaaaa", TextType.SimpleText, TestName = "simple text contains escape")]
        [TestCase(@"aaaa\\a", @"aaaa\a", TextType.SimpleText, TestName = "simple text contains escaped slash")]
        [TestCase(@"aaaa\u", @"aaaau", TextType.SimpleText, TestName = "simple text contains escaped special symbol")]
        public void GetNextPart_ReturnsCorrectPartWithSimpleText_WhenGetsStringNotEmptyText(string text, 
            string expectedFirstPartText, 
            TextType expectedFirstPartType)
        {
            var parser = GetCommonParser(text);
            var expectedPart = new TextPart(expectedFirstPartText, expectedFirstPartType);

            parser.GetNextPart().Should()
                .BeEquivalentTo(expectedPart);
        }
        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfAString()
        {
            Assert.Throws<ArgumentException>(() => GetCommonParser(null));
        }

        [Test]
        public void Creation_RaisesException_WhenGetsNullInsteadOfAReader()
        {
            Assert.Throws<ArgumentException>(() => new TextParser(null, Array.AsReadOnly(baseParseSymbols), true));
        }

        [Test]
        public void Creation_RaisesException_WhenParsingWhiteSpacesIsEnabledAndSpecialSymbolsContainWhiteSpace()
        {
            var parseSymbols = new[] { 'a', ' ' };
            var reader = new StringReader("");

            Assert.Throws<ArgumentException>(() => new TextParser(reader, Array.AsReadOnly(parseSymbols), true));            
        }

        [Test]
        public void Creation_DoesNotRaiseException_WhenParsingWhiteSpacesIsNotEnabledAndSpecialSymbolsContainWhiteSpace()
        {
            var parseSymbols = new[] { 'a', ' ' };
            var reader = new StringReader("");

            Assert.DoesNotThrow(() => new TextParser(reader, Array.AsReadOnly(parseSymbols), false));
        }

        [Test]
        public void Creation_RaisesException_WhenSpecialSymbolsContainSlash()
        {
            var parseSymbols = new [] {'\\'};
            var reader = new StringReader("");

            Assert.Throws<ArgumentException>(() => new TextParser(reader, Array.AsReadOnly(parseSymbols), false));
        }

        

        [TestCase("", 1, TestName = "Text is empty")]
        [TestCase("aaa", 2, TestName = "Text consists of simple text")]
        [TestCase("aaaaaa    ", 3, TestName = "Text consists of simple text and white-spaces parts")]
        [TestCase("a a a a", 8, TestName = "Text consists of many parts")]
        public void GetNextPart_WillBeExecutedCorrect_WhenItStopsAfterAnyPartsReturnsFalse(String text, int expectedCount)
        {
            var parser = GetCommonParser(text);

            parser.GetAllParts().Should().HaveCount(expectedCount);
        }   

        [Test]
        public void AnyParts_ReturnsFalse_WhenThereIsNoMoreParts()
        {
            var parser = GetCommonParser("a a a");
            var partsCount = 6;

            for (var index = 0; index < partsCount; index++)
                parser.GetNextPart();

            parser.AnyParts().Should().BeFalse();
        }

        [Test]
        public void GetNextPart_ReturnsAllTextInTotal_WhenItStopsAfterAnyPartsReturnsFalse()
        {
            var text = "skjdks_a_j_d_skdjsasdkjskd jslk dsaj kdl\tsajlkdsjsdalksjdlksdjlsakjwiewwedwquoiwquwoi";
            var parser = GetCommonParser(text);

            parser.GetText().Should().Be(text);
        }

        [Test, Timeout(1000)]
        public void GetNextPart_WorksFast_WhenParserHasOneBigPart()
        {
            var countRepetitions = 1000000;
            var text = new string('a', countRepetitions);
            var parser = GetCommonParser(text);

            parser.GetNextPart();
        }

        [Test, Timeout(1000)]
        public void GetNextPart_WorksFast_WhenParserHasManyDifferentParts()
        {
            var countRepetitions = 250000;
            var text = string.Concat(Enumerable.Repeat("a ", countRepetitions));
            var parser = GetCommonParser(text);

            var parts = parser.GetAllParts();
        }

        [Test]
        public void GetNextPart_ReturnsCorrectOnFirstParser_WhenTwoParsersAreCreated()
        {
            var firstParserText = "aaaaa";
            var firstParser = GetCommonParser(firstParserText);
            var secondParser = GetCommonParser("      ");
            var expectedPart = new TextPart(firstParserText, TextType.SimpleText);

            firstParser.GetNextPart().Should().BeEquivalentTo(expectedPart);
        }

        [Test]
        public void GetNextPart_ReturnsCorrectOnSecondParser_WhenTwoParsersAreCreated()
        {
            var secondParserText = "aaaaa";
            var firstParser = GetCommonParser("ababa");
            var secondParser = GetCommonParser(secondParserText);
            var expectedPart = new TextPart(secondParserText, TextType.SimpleText);


            secondParser.GetNextPart().Should().BeEquivalentTo(expectedPart);
        }

        [Test]
        public void GetNextPart_CountsWhiteSpacesInSimpleText_WhenWhiteSpacesAreDisabled()
        {
            var text = "      ";
            var reader = new StringReader(text);
            var parser = new TextParser(reader, Array.AsReadOnly(baseParseSymbols), false);
            var expectedPart = new TextPart(text, TextType.SimpleText);

            parser.GetNextPart().Should().BeEquivalentTo(expectedPart);
        }

        [Test]
        public void GetNextPart_CountsPartSymbolsInSpecialSymbols_WhenThereAreManyParseSymbols()
        {
            var parseSymbols = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'z', 'h' };
            var text = string.Concat(parseSymbols);
            var reader = new StringReader(text);
            var parser = new TextParser(reader, Array.AsReadOnly(parseSymbols), true);
            var expectedPart = new TextPart(text, TextType.SpecialSymbols);

            parser.GetNextPart().Should().BeEquivalentTo(expectedPart);
        }

        [Test]
        public void GetNextPart_CountsWhiteSpacesInSpecialSymbols_WhenWhiteSpacesAreDisabledAndParseSymbolsContainsSpaces()
        {
            var parseSymbols = new char[] { ' ', '\t', '\r' };
            var text = string.Concat(parseSymbols);
            var reader = new StringReader(text);
            var parser = new TextParser(reader, Array.AsReadOnly(parseSymbols), false);
            var expectedPart = new TextPart(text, TextType.SpecialSymbols);

            parser.GetNextPart().Should().BeEquivalentTo(expectedPart);
        }
    }
}
