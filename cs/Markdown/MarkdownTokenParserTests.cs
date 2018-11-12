using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class MarkdownTokenParserTests
    {
        private MarkdownTokenParser parser;

        [SetUp]
        public void SetUp()
        {
            var tags = new[] { "_", "__" };
            parser = new MarkdownTokenParser(tags);
        }

        [TestCase(null, new string[0], TestName = "Null")]
        [TestCase("", new string[0], TestName = "EmptyString")]
        [TestCase("abc", new[] { "abc" }, TestName = "Letters")]
        [TestCase("123", new[] { "123" }, TestName = "Digits")]
        [TestCase("!?(),.:;\"\'-", new[] { "!", "?", "(", ")", ",", ".", ":", ";", "\"", "\'", "-" }, TestName = "PunctuationSymbols")]
        [TestCase("_", new[] { "_" }, TestName = "ShortTag")]
        [TestCase("__", new[] { "__" }, TestName = "LongTag")]
        [TestCase(" ", new[] { " " }, TestName = "OneSpace")]
        [TestCase("\\", new[] { "\\" }, TestName = "EscapeSymbol")]
        [TestCase("  ", new[] { "  " }, TestName = "TwoSpaces")]
        [TestCase("\t", new[] { "\t" }, TestName = "Tab")]
        [TestCase("a ", new[] { "a", " " }, TestName = "LetterAndSpace")]
        [TestCase(" a", new[] { " ", "a" }, TestName = "SpaceAndLetter")]
        [TestCase("a b", new[] { "a", " ", "b" }, TestName = "SpaceBetweenLetters")]
        [TestCase("_a", new[] { "_", "a" }, TestName = "ShortTagAndLetter")]
        [TestCase("a_", new[] { "a", "_" }, TestName = "LetterAndShortTag")]
        [TestCase("_a_", new[] { "_", "a", "_" }, TestName = "LetterBetweenShortTag")]
        [TestCase("__a", new[] { "__", "a" }, TestName = "LongTagAndLetter")]
        [TestCase("a__", new[] { "a", "__" }, TestName = "LetterAndLongTag")]
        [TestCase("__a__", new[] { "__", "a", "__" }, TestName = "LetterBetweenLongTag")]
        [TestCase("\\a", new[] { "\\", "a" }, TestName = "EscapeSymbolAndLetter")]
        [TestCase("a\\", new[] { "a", "\\" }, TestName = "LetterAndEscapeSymbol")]
        [TestCase("___", new[] { "__", "_" }, TestName = "BothTags")]
        [TestCase(" _ ", new[] { " ", "_", " " }, TestName = "ShortTagBetweenSpaces")]
        [TestCase("a_b", new[] { "a", "_", "b" }, TestName = "ShortTagBetweenLetters")]
        public void TestGetTokens(string inputString, string[] expectedResult)
        {
            parser.GetTokens(inputString).Should().BeEquivalentTo(expectedResult);
        }
    }
}