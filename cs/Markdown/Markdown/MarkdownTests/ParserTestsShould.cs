using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class ParserTestsShould
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
        public bool IsEndsWithSingleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            var parser = new Parser(escapeSymbol);
            return parser.IsEndsWithSingleUnderscore(str);
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
        public bool IsEndsWithDoubleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            var parser = new Parser(escapeSymbol);
            return parser.IsEndsWithDoubleUnderscore(str);
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
        public bool IsStartsWithSingleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            var parser = new Parser(escapeSymbol);
            return parser.IsStartsWithSingleUnderscore(str);
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
        public bool IsStartsWithDoubleUnderscoreShouldBe(string str, char escapeSymbol = '/')
        {
            var parser = new Parser(escapeSymbol);
            return parser.IsStartsWithDoubleUnderscore(str);
        }

        [TestCase('0', ExpectedResult = true, TestName = "TrueWhen_Null")]
        [TestCase('5', ExpectedResult = true, TestName = "TrueWhen_Five")]
        [TestCase('9', ExpectedResult = true, TestName = "TrueWhen_Nine")]
        [TestCase('a', ExpectedResult = false, TestName = "FalseWhen_CommonLetter")]
        [TestCase('_', ExpectedResult = false, TestName = "FalseWhen_Underscore")]
        [TestCase('-', ExpectedResult = false, TestName = "FalseWhen_Minus")]
        [TestCase('+', ExpectedResult = false, TestName = "FalseWhen_Plus")]
        [TestCase(' ', ExpectedResult = false, TestName = "FalseWhen_Space")]
        [TestCase('\n', ExpectedResult = false, TestName = "FalseWhen_BreakLine")]
        public bool IsDigitShouldBe(char symbol)
        {
            return Parser.IsDigit(symbol);
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
            return Parser.StrContainsSymbolsAndDigits(str);
        }

        [TestCase(' ', ExpectedResult = true, TestName = "TrueWhen_Space")]
        [TestCase('\n', ExpectedResult = true, TestName = "TrueWhen_BreaksLine")]
        [TestCase('A', ExpectedResult = false, TestName = "FalseWhen_Letter")]
        [TestCase('_', ExpectedResult = false, TestName = "FalseWhen_Underscore")]
        public bool IsWhitespaceShouldBe(char symbol)
        {
            return Parser.IsWhitespace(symbol);
        }

        [TestCase(" Abc", ExpectedResult = " ", TestName = "BeSpace_IfStartsWithSpace")]
        [TestCase("   Abc", ExpectedResult = "   ", TestName = "BeSpaces_IfStartsWithSpaces")]
        [TestCase("\nAbc", ExpectedResult = "\n", TestName = "BeBreakLine_IfStartsWithBreakLine")]
        [TestCase("\n\nAbc", ExpectedResult = "\n\n", TestName = "BeBreaksLine_IfStartsWithBreaksLine")]
        [TestCase(" \n Abc", ExpectedResult = " \n ", TestName = "BeWhitespaceSymbols_IfStartsWithWhitespaceSymbols")]
        [TestCase("A", ExpectedResult = "A", TestName = "BeLetter_IfOnlyOneLetter")]
        [TestCase("A ", ExpectedResult = "A", TestName = "BeLetter_IfStartsWithLetter")]
        [TestCase("Abc \n", ExpectedResult = "Abc", TestName = "BeLetterSequence_IfStartsWithLetterSequence")]
        [TestCase("", ExpectedResult = "", TestName = "BeEmpty_OnEmptyInput")]
        public string GetSubStringShould(string str)
        {
            return Parser.GetSubString(str, 0);
        }

        [TestCase(null, TestName = "OnNullInput")]
        public void GetSubStringShouldThrowException(string str)
        {
            Action act = () => Parser.GetSubString(null, 0);

            act.ShouldThrow<ArgumentNullException>();
        }

        [TestCase("a/_b")]
        public void ParseShould(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() {new StringPart("a/_b", ActionType.NotAnAction, TagType.String)};

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc")]
        public void ParseShould1(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("Abc", ActionType.NotAnAction, TagType.String) };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("_Abc")]
        public void ParseShould2(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("_", ActionType.Open, TagType.Em), new StringPart("Abc", ActionType.NotAnAction, TagType.String) };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("__Abc")]
        public void ParseShould3(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("__", ActionType.Open, TagType.Strong), new StringPart("Abc", ActionType.NotAnAction, TagType.String) };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc_")]
        public void ParseShould4(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("Abc", ActionType.NotAnAction, TagType.String), new StringPart("_", ActionType.Close, TagType.Em) };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc__")]
        public void ParseShould5(string str)
        {
            var parser = new Parser('/');
            var result = parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("Abc", ActionType.NotAnAction, TagType.String), new StringPart("__", ActionType.Close, TagType.Strong) };

            expected.ShouldBeEquivalentTo(result);
        }
    }
}
