using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Md
{
    [TestFixture]
    public class MdParserTests
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

        private static IEnumerable SimpleCases()
        {
            TestCaseData data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("plain text", new[]
                {
                    new TokenNode(MdSpecification.Text, "plain text"),
                }))
                .SetName("plain text");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("text_with_underscores", new[]
            {
                new TokenNode(MdSpecification.Text, "text"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "with"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "underscores"),
            })).SetName("text with underscores, not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("text__with double__underscores", new[]
            {
                new TokenNode(MdSpecification.Text, "text"),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, "with double"),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, "underscores"),
            })).SetName("text with double underscores, not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("\\_tag escape\\_", new[]
            {
                new TokenNode(MdSpecification.Text, "\\"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "tag escape\\"),
                new TokenNode(MdSpecification.Text, "_"),
            })).SetName("escape symbol working");

            yield return data;
        }

        private static IEnumerable DifferentCases()
        {
            TestCaseData data;
            data = new TestCaseData(new Tuple<string, TokenNode[]>("_italic_", new[]
            {
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "italic"),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Close),
            })).SetName("italic");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("__bold__", new[]
            {
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "bold"),
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Close),
            })).SetName("bold");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("__bold with _italic_ z__", new[]
            {
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "bold with "),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "italic"),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Close),
                new TokenNode(MdSpecification.Text, " z"),
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Close),
            })).SetName("italic inside bold");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("_italic with __ignored bold__ z_", new[]
            {
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "italic with "),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, "ignored bold"),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, " z"),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Close),
            })).SetName("ignored bold inside italic");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("_12_3", new[]
            {
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "12"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "3"),
            })).SetName("underscore between numbers is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("__42__42", new[]
            {
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "42"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "42"),
            })).SetName("double underscore between numbers is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("__непарные _символы не считаются выделением",
                new[]
                {
                    new TokenNode(MdSpecification.Text, "__"),
                    new TokenNode(MdSpecification.Text, "непарные "),
                    new TokenNode(MdSpecification.Text, "_"),
                    new TokenNode(MdSpecification.Text, "символы не считаются выделением"),
                })).SetName("unpaired tags are not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("эти_ подчерки_ не считаются", new[]
            {
                new TokenNode(MdSpecification.Text, "эти"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, " подчерки"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, " не считаются"),
            })).SetName("opening emphasis tag with whitespace ahead is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("эти__ подчерки__ не считаются", new[]
            {
                new TokenNode(MdSpecification.Text, "эти"),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, " подчерки"),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, " не считаются"),
            })).SetName("opening strong tag with whitespace ahead is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("эти _подчерки не считаются _", new[]
            {
                new TokenNode(MdSpecification.Text, "эти "),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "подчерки не считаются "),
                new TokenNode(MdSpecification.Text, "_"),
            })).SetName("closing emphasis tag with whitespace behind is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("эти __подчерки не считаются __", new[]
            {
                new TokenNode(MdSpecification.Text, "эти "),
                new TokenNode(MdSpecification.Text, "__"),
                new TokenNode(MdSpecification.Text, "подчерки не считаются "),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "_"),
            })).SetName("closing strong tag with whitespace behind is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("___te_st___", new[]
            {
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Open),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "te"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "st"),
                new TokenNode(MdSpecification.Emphasis, "_", TokenPairType.Close),
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Close),
            })).SetName("underscore in text is not a tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, TokenNode[]>("__\\_hello\\___", new[]
            {
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Open),
                new TokenNode(MdSpecification.Text, "\\"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.Text, "hello\\"),
                new TokenNode(MdSpecification.Text, "_"),
                new TokenNode(MdSpecification.StrongEmphasis, "__", TokenPairType.Close),
            })).SetName("escape is working inside strong emphasis");

            yield return data;
        }

        [TestCaseSource(nameof(SimpleCases))]
        [TestCaseSource(nameof(DifferentCases))]
        public void Parse_WhenCorrectTokens_ReturnsCorrectResult(Tuple<string, TokenNode[]> data)
        {
            var tokenNode = lexicalParser.Parse(data.Item1);
            var result = GetNextTokenNode(tokenNode.Children);
            result.Should()
                .BeEquivalentTo(data.Item2);
        }

        public IEnumerable<ITokenNode> GetNextTokenNode(ICollection<ITokenNode> token)
        {
            foreach (var child in token)
            {
                yield return child;

                foreach (var subChild in GetNextTokenNode(child.Children))
                    yield return subChild;
            }
        }
    }
}