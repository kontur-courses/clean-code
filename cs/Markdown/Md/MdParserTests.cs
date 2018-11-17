using System;
using System.Collections;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Md
{
    [TestFixture]
    public class MdParserTests
    {
        private MdParser lexicalParser;

        [SetUp]
        public void DoBeforeAnyTest()
        {
            lexicalParser = new MdParser();
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

            data = new TestCaseData(new Tuple<string, MdToken[]>("plain text", new[]
                {
                    new MdToken(MdType.Text, "plain text"),
                }))
                .SetName("plain text");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("text_with_underscores", new[]
            {
                new MdToken(MdType.Text, "text"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "with"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "underscores"),
            })).SetName("text with underscores, not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("text__with double__underscores", new[]
            {
                new MdToken(MdType.Text, "text"),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, "with double"),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, "underscores"),
            })).SetName("text with double underscores, not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("\\_tag escape\\_", new[]
            {
                new MdToken(MdType.Text, "\\"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "tag escape\\"),
                new MdToken(MdType.Text, "_"),
            })).SetName("escape symbol working");

            yield return data;
        }

        private static IEnumerable DifferentCases()
        {
            TestCaseData data;
            data = new TestCaseData(new Tuple<string, MdToken[]>("_italic_", new[]
            {
                new MdToken(MdType.OpenEmphasis, "_"),
                new MdToken(MdType.Text, "italic"),
                new MdToken(MdType.CloseEmphasis, "_"),
            })).SetName("italic");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("__bold__", new[]
            {
                new MdToken(MdType.OpenStrongEmphasis, "__"),
                new MdToken(MdType.Text, "bold"),
                new MdToken(MdType.CloseStrongEmphasis, "__"),
            })).SetName("bold");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("__bold with _italic_ z__", new[]
            {
                new MdToken(MdType.OpenStrongEmphasis, "__"),
                new MdToken(MdType.Text, "bold with "),
                new MdToken(MdType.OpenEmphasis, "_"),
                new MdToken(MdType.Text, "italic"),
                new MdToken(MdType.CloseEmphasis, "_"),
                new MdToken(MdType.Text, " z"),
                new MdToken(MdType.CloseStrongEmphasis, "__"),
            })).SetName("italic inside bold");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("_italic with __ignored bold__ z_", new[]
            {
                new MdToken(MdType.OpenEmphasis, "_"),
                new MdToken(MdType.Text, "italic with "),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, "ignored bold"),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, " z"),
                new MdToken(MdType.CloseEmphasis, "_"),
            })).SetName("ignored bold inside italic");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("_12_3", new[]
            {
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "12"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "3"),
            })).SetName("underscore between numbers is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("__42__42", new[]
            {
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "42"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "42"),
            })).SetName("double underscore between numbers is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("__непарные _символы не считаются выделением", new[]
            {
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, "непарные "),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "символы не считаются выделением"),
            })).SetName("unpaired tags are not tags");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("эти_ подчерки_ не считаются", new[]
            {
                new MdToken(MdType.Text, "эти"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, " подчерки"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, " не считаются"),
            })).SetName("opening emphasis tag with whitespace ahead is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("эти__ подчерки__ не считаются", new[]
            {
                new MdToken(MdType.Text, "эти"),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, " подчерки"),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, " не считаются"),
            })).SetName("opening strong tag with whitespace ahead is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("эти _подчерки не считаются _", new[]
            {
                new MdToken(MdType.Text, "эти "),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "подчерки не считаются "),
                new MdToken(MdType.Text, "_"),
            })).SetName("closing emphasis tag with whitespace behind is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("эти __подчерки не считаются __", new[]
            {
                new MdToken(MdType.Text, "эти "),
                new MdToken(MdType.Text, "__"),
                new MdToken(MdType.Text, "подчерки не считаются "),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "_"),
            })).SetName("closing strong tag with whitespace behind is not tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("___te_st___", new[]
            {
                new MdToken(MdType.OpenStrongEmphasis, "__"),
                new MdToken(MdType.OpenEmphasis, "_"),
                new MdToken(MdType.Text, "te"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "st"),
                new MdToken(MdType.CloseEmphasis, "_"),
                new MdToken(MdType.CloseStrongEmphasis, "__"),
            })).SetName("underscore in text is not a tag");

            yield return data;

            data = new TestCaseData(new Tuple<string, MdToken[]>("__\\_hello\\___", new[]
            {
                new MdToken(MdType.OpenStrongEmphasis, "__"),
                new MdToken(MdType.Text, "\\"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.Text, "hello\\"),
                new MdToken(MdType.Text, "_"),
                new MdToken(MdType.CloseStrongEmphasis, "__"),
            })).SetName("escape is working inside strong emphasis");

            yield return data;
        }

        [TestCaseSource(nameof(SimpleCases))]
        [TestCaseSource(nameof(DifferentCases))]
        public void Parse_WhenCorrectTokens_ReturnsCorrectResult(Tuple<string, MdToken[]> data)
        {
            var result = lexicalParser.Parse(data.Item1);

            result.Should()
                .BeEquivalentTo(data.Item2);
        }
    }
}