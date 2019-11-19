using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using MarkDown;
using MarkDown.TokenParsers;
using NUnit.Framework;

namespace MarkDown_Tests
{
    class EMTokenParser_Tests
    {
        //TODO Additional functionalTests
        protected static EMParser Parser { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Parser = new EMParser();
        }

        [TestCase("\\__a_", 2)]
        [TestCase("_a_", 0)]
        [TestCase(" _a_ ", 1)]
        [TestCase("_a _a_ ", 3)]
        public void IsTag_CorrectOpenTag_ReturnTrue(string line, int startIndex)
        {
            var result = Parser.IsTag(line, startIndex, true);

            result.Should().BeTrue();
        }

        [TestCase("\\_a_", 1)]
        [TestCase(" _ a_ ", 1)]
        [TestCase("_a __a_ ", 3)]
        public void IsTag_IncorrectOpenedTag_ReturnFalse(string line, int startIndex)
        {
            var result = Parser.IsTag(line, startIndex, true);

            result.Should().BeFalse();
        }

        [TestCase("_a_", 2)]
        [TestCase(" _a_ ", 3)]
        public void IsTag_CorrectClosedTag_ReturnTrue(string line, int startIndex)
        {
            var result = Parser.IsTag(line, startIndex, false);

            result.Should().BeTrue();
        }

        [TestCase("_a\\_", 2)]
        [TestCase(" _a _ ", 3)]
        public void IsTag_IncorrectClosedTag_ReturnFalse(string line, int startIndex)
        {
            var result = Parser.IsTag(line, startIndex, false);

            result.Should().BeFalse();
        }
       
    }
}