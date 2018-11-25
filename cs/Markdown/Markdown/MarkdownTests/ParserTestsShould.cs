using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class ParserTestsShould
    {
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
            var result = Parser.Parse(str);

            var expected = new List<StringPart>() {new StringPart("a_b")};

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("a_b")]
        public void ParseShould0(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("a"),
                new StringPart("_", ActionType.OpenOrClose, TagType.Em),
                new StringPart("b")
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc")]
        public void ParseShould1(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>() { new StringPart("Abc") };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("_Abc")]
        public void ParseShould2(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>
            {
                new StringPart("_", ActionType.Open, TagType.Em), new StringPart("Abc")
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("__Abc")]
        public void ParseShould3(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("__", ActionType.Open, TagType.Strong), new StringPart("Abc")
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc_")]
        public void ParseShould4(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("Abc"), new StringPart("_", ActionType.Close, TagType.Em)
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("Abc__")]
        public void ParseShould5(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("Abc"), new StringPart("__", ActionType.Close, TagType.Strong)
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("  _ab cd_\n __ef_gh__ij /_")]
        public void ParseShould6(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("  "),
                new StringPart("_", ActionType.Open, TagType.Em),
                new StringPart("ab"),

                new StringPart(" "),
                new StringPart("cd"),
                new StringPart("_", ActionType.Close, TagType.Em),

                new StringPart("\n "),
                new StringPart("__", ActionType.Open, TagType.Strong),
                new StringPart("ef"),
                new StringPart("_", ActionType.OpenOrClose, TagType.Em),
                new StringPart("gh"),
                new StringPart("__", ActionType.OpenOrClose, TagType.Strong),
                new StringPart("ij"),

                new StringPart(" "),
                new StringPart("_"),
            };

            expected.ShouldBeEquivalentTo(result);
        }

        [TestCase("_a __abc__")]
        public void ParseShould7(string str)
        {
            var result = Parser.Parse(str);

            var expected = new List<StringPart>()
            {
                new StringPart("_", ActionType.Open, TagType.Em),
                new StringPart("a"),
                new StringPart(" "),
                new StringPart("__", ActionType.Open, TagType.Strong),
                new StringPart("abc"),
                new StringPart("__", ActionType.Close, TagType.Strong),
            };

            expected.ShouldBeEquivalentTo(result); 
        }
    }
}
