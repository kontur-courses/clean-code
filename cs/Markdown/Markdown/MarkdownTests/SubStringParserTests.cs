using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class SubStringParserTests
    {
        [TestCase("A_", ExpectedResult = true, TestName = "TrueWhen_UnderscoreAfterSymbol")]
        [TestCase("Abc_", ExpectedResult = true, TestName = "TrueWhen_UnderscoreAfterSymbolSequence")]
        [TestCase("/__", ExpectedResult = true, TestName = "TrueWhen_UnderscoreAfterEscapedSpecialSymbol")]
        [TestCase("Abc/__", ExpectedResult = true, TestName = "TrueWhen_UnderscoreAfterEscapedSpecialSymbolAfterCommonSequence")]
        [TestCase("/_", ExpectedResult = false, TestName = "FalseWhen_UnderscoreAfterEscapeSymbol")]
        [TestCase("__", ExpectedResult = false, TestName = "FalseWhen_UnderscoreAfterNotEscapedSpecialSymbol")]
        [TestCase("_", ExpectedResult = false, TestName = "FalseWhen_UnderscoreWithoutLetters")]
        [TestCase("A", ExpectedResult = false, TestName = "FalseWhen_NoUnderscore")]
        [TestCase("", ExpectedResult = false, TestName = "FalseWhen_EmptyString")]
        [TestCase(null, ExpectedResult = false, TestName = "FalseWhen_Null")]
        public bool EndsWithSingleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            return SubstringParser.EndsWithSingleUnderscore(str, escapeSymbol);
        }

        [TestCase("A__", ExpectedResult = true, TestName = "TrueWhen_UnderscoresAfterSymbol")]
        [TestCase("Abc__", ExpectedResult = true, TestName = "TrueWhen_UnderscoresAfterSymbolSequence")]
        [TestCase("/___", ExpectedResult = true, TestName = "TrueWhen_UnderscoresAfterEscapedSpecialSymbol")]
        [TestCase("Abc/___", ExpectedResult = true, TestName = "TrueWhen_UnderscoresAfterEscapedSpecialSymbolAfterCommonSequence")]
        [TestCase("/__", ExpectedResult = false, TestName = "FalseWhen_UnderscoresAfterEscapeSymbol")]
        [TestCase("___", ExpectedResult = false, TestName = "FalseWhen_UnderscoresAfterNotEscapedSpecialSymbol")]
        [TestCase("__", ExpectedResult = false, TestName = "FalseWhen_UnderscoresWithoutLetters")]
        [TestCase("_", ExpectedResult = false, TestName = "FalseWhen_SingleUnderscoreWithoutLetters")]
        [TestCase("A_", ExpectedResult = false, TestName = "FalseWhen_SingleUnderscore")]
        [TestCase("A", ExpectedResult = false, TestName = "FalseWhen_NoUnderscores")]
        [TestCase("", ExpectedResult = false, TestName = "FalseWhen_EmptyString")]
        [TestCase(null, ExpectedResult = false, TestName = "FalseWhen_Null")]
        public bool EndsWithDoubleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            return SubstringParser.EndsWithDoubleUnderscore(str, escapeSymbol);
        }

        [TestCase("_A", ExpectedResult = true, TestName = "TrueWhen_UnderscoreBeforeSymbol")]
        [TestCase("_Abc", ExpectedResult = true, TestName = "TrueWhen_UnderscoreBeforeSymbolSequence")]
        [TestCase("_/_", ExpectedResult = true, TestName = "TrueWhen_UnderscoreBeforeEscapedSpecialSymbol")]
        [TestCase("_/_Abc", ExpectedResult = true, TestName = "TrueWhen_UnderscoreBeforeEscapedSpecialSymbolAndCommonSequence")]
        [TestCase("_/", ExpectedResult = false, TestName = "FalseWhen_UnderscoreBeforeEscapeSymbol")]
        [TestCase("__", ExpectedResult = false, TestName = "FalseWhen_UnderscoreBeforeNotEscapedSpecialSymbol")]
        [TestCase("_", ExpectedResult = false, TestName = "FalseWhen_UnderscoreWithoutLetters")]
        [TestCase("A", ExpectedResult = false, TestName = "FalseWhen_NoUnderscore")]
        [TestCase("", ExpectedResult = false, TestName = "FalseWhen_EmptyString")]
        [TestCase(null, ExpectedResult = false, TestName = "FalseWhen_Null")]
        public bool StartsWithSingleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            return SubstringParser.StartsWithSingleUnderscore(str, escapeSymbol);
        }

        [TestCase("__A", ExpectedResult = true, TestName = "TrueWhen_UnderscoresBeforeSymbol")]
        [TestCase("__Abc", ExpectedResult = true, TestName = "TrueWhen_UnderscoresBeforeSymbolSequence")]
        [TestCase("__/_", ExpectedResult = true, TestName = "TrueWhen_UnderscoresBeforeEscapedSpecialSymbol")]
        [TestCase("__/_Abc", ExpectedResult = true, TestName = "TrueWhen_UnderscoresBeforeEscapedSpecialSymbolAndCommonSequence")]
        [TestCase("__/", ExpectedResult = false, TestName = "FalseWhen_UnderscoresBeforeEscapeSymbol")]
        [TestCase("___", ExpectedResult = false, TestName = "FalseWhen_UnderscoresBeforeNotEscapedSpecialSymbol")]
        [TestCase("__", ExpectedResult = false, TestName = "FalseWhen_UnderscoresWithoutLetters")]
        [TestCase("_A", ExpectedResult = false, TestName = "FalseWhen_SingleUnderscoreBeforeCommonSymbol")]
        [TestCase("A", ExpectedResult = false, TestName = "FalseWhen_NoUnderscores")]
        [TestCase("", ExpectedResult = false, TestName = "FalseWhen_EmptyString")]
        [TestCase(null, ExpectedResult = false, TestName = "FalseWhen_Null")]
        public bool StartsWithDoubleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            return SubstringParser.StartsWithDoubleUnderscore(str, escapeSymbol);
        }

        [TestCase("A6", ExpectedResult = true, TestName = "TrueWhen_OneLetterAndOneDigit")]
        [TestCase("A666", ExpectedResult = true, TestName = "TrueWhen_OneLetterAndFewDigits")]
        [TestCase("AAA6", ExpectedResult = true, TestName = "TrueWhen_FewLetterAndOneDigit")]
        [TestCase("_Ab_65", ExpectedResult = true, TestName = "TrueWhen_LettersAndDigitsWithUnderscores")]
        [TestCase("Abc", ExpectedResult = false, TestName = "FalseWhen_OnlyLetters")]
        [TestCase("666", ExpectedResult = false, TestName = "FalseWhen_OnlyDigits")]
        [TestCase("", ExpectedResult = false, TestName = "FalseWhen_EmptyString")]
        [TestCase(null, ExpectedResult = false, TestName = "FalseWhen_Null")]
        public bool StrContainsSymbolsAndDigitsShouldBe(string str)
        {
            return SubstringParser.StrContainsSymbolsAndDigits(str);
        }

        [TestCase("", TestName = "EmptyInput")]
        public void SubStringParserShould(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>() {new StringPart("")};

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("Abc")]
        public void SubStringParserShould0(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("Abc") };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("_")]
        public void SubStringParserShould1(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("_") };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("__")]
        public void SubStringParserShould2(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("__") };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("___")]
        public void SubStringParserShould21(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("_"),
                new StringPart("__")
            };

            result.ShouldBeEquivalentTo(expected);
        }


        [TestCase("A_")]
        public void SubStringParserShould3(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("A"),
                new StringPart("_", ActionType.Close, TagType.Em)
            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("A__")]
        public void SubStringParserShould4(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("A"),
                new StringPart("__", ActionType.Close, TagType.Strong)
            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("__A")]
        public void SubStringParserShould5(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("__", ActionType.Open, TagType.Strong),
                new StringPart("A")

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("_A")]
        public void SubStringParserShould6(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("_", ActionType.Open, TagType.Em),
                new StringPart("A")

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a_b")]
        public void SubStringParserShould7(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a"),
                new StringPart("_", ActionType.OpenOrClose, TagType.Em),
                new StringPart("b")

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a__b")]
        public void SubStringParserShould8(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a"),
                new StringPart("__", ActionType.OpenOrClose, TagType.Strong),
                new StringPart("b")

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a/_b")]
        public void SubStringParserShould9(string subString)
        {
            var result = new SubstringParser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a_b"),
            };

            result.ShouldBeEquivalentTo(expected);
        }
    }
}
