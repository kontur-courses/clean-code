using System;
using System.Collections;
using System.Collections.Generic;
using Fclp.Internals.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Md
{
    [TestFixture]
    public class ParserTests
    {
        private Parser lexicalParser;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            lexicalParser = new Parser(MdSpecification.GetTagHandlerChain());
        }

        [Test]
        public void Parse_WhenNullString_ThrowsException()
        {
            Action a = () => lexicalParser.Parse(null);
            a
                .Should()
                .Throw<ArgumentException>();
        }

        [TestCase("plain text", "text")]
        [TestCase("text_with_underscores", "text text text text text")]
        [TestCase("text__with double__underscores", "text text text")]
        [TestCase("\\_tag escape\\_", "text text text text")]
        [TestCase("_italic_", "emphasis text emphasis")]
        [TestCase("__bold__", "strong text strong")]
        [TestCase("__bold with _italic_ z__", "strong text emphasis text emphasis text strong")]
        [TestCase("_italic with __ignored bold__ z_", "emphasis text text text text text emphasis")]
        [TestCase("_12_3", "text text text text")]
        [TestCase("__42__42", "text text text text")]
        [TestCase("__непарные _символы не считаются выделением", "text text")]
        [TestCase("эти_ подчерки_ не считаются", "text text text")]
        [TestCase("эти__ подчерки__ не считаются", "text text text")]
        [TestCase("эти _подчерки не считаются _", "text text text")]
        [TestCase("эти __подчерки не считаются __", "text text text")]
        [TestCase("___te_st___", "strong emphasis text text text emphasis strong")]
        [TestCase("__\\_hello\\___", "strong text text text text strong")]
        public void Parse_WhenCorrectTokens_ReturnsCorrectResult(string input, string types)
        {
            var tag = lexicalParser.Parse(input);
            var tagTypes = string.Join(" ", GetTagsType(tag));
            tagTypes.Should()
                .BeEquivalentTo(types);
        }

        static IEnumerable<string> GetTagsType(Tag root)
        {
            if (root.Type != "root")
            {
                yield return root.Type;
            }

            if (root.Tags == null)
            {
                yield break;
            }

            foreach (var tag in root.Tags)
            {
                foreach (var subtag in GetTagsType(tag))
                {
                    yield return subtag;
                }
            }

            if (root.Type != "root")
            {
                yield return root.Type;
            }
        }
    }
}