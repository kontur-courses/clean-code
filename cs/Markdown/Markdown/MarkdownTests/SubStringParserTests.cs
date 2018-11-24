using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.MarkdownTests
{
    [TestFixture]
    class SubStringParserTests
    {
        [TestCase("", TestName = "EmptyInput")]
        public void SubStringParserShould(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>() {new StringPart("", ActionType.NotAnAction, TagType.String)};

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("Abc")]
        public void SubStringParserShould0(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("Abc", ActionType.NotAnAction, TagType.String) };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("_")]
        public void SubStringParserShould1(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("_", ActionType.NotAnAction, TagType.String) };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("__")]
        public void SubStringParserShould2(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>() { new StringPart("__", ActionType.NotAnAction, TagType.String) };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("___")]
        public void SubStringParserShould21(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("_", ActionType.NotAnAction, TagType.String),
                new StringPart("__", ActionType.NotAnAction, TagType.String)
            };

            result.ShouldBeEquivalentTo(expected);
        }


        [TestCase("A_")]
        public void SubStringParserShould3(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("A", ActionType.NotAnAction, TagType.String),
                new StringPart("_", ActionType.Close, TagType.Em)
            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("A__")]
        public void SubStringParserShould4(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("A", ActionType.NotAnAction, TagType.String),
                new StringPart("__", ActionType.Close, TagType.Strong)
            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("__A")]
        public void SubStringParserShould5(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("__", ActionType.Open, TagType.Strong),
                new StringPart("A", ActionType.NotAnAction, TagType.String)

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("_A")]
        public void SubStringParserShould6(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("_", ActionType.Open, TagType.Em),
                new StringPart("A", ActionType.NotAnAction, TagType.String)

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a_b")]
        public void SubStringParserShould7(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a", ActionType.NotAnAction, TagType.String),
                new StringPart("_", ActionType.OpenOrClose, TagType.Em),
                new StringPart("b", ActionType.NotAnAction, TagType.String)

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a__b")]
        public void SubStringParserShould8(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a", ActionType.NotAnAction, TagType.String),
                new StringPart("__", ActionType.OpenOrClose, TagType.Strong),
                new StringPart("b", ActionType.NotAnAction, TagType.String)

            };

            result.ShouldBeEquivalentTo(expected);
        }

        [TestCase("a/_b")]
        public void SubStringParserShould9(string subString)
        {
            var result = new Parser().ParseSubString(subString);

            var expected = new List<StringPart>()
            {
                new StringPart("a_b", ActionType.NotAnAction, TagType.String),
            };

            result.ShouldBeEquivalentTo(expected);
        }
    }
}
