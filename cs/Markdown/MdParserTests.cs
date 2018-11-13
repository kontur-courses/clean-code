using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
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
        public void Parse_ThrowsException_OnNullString()
        {
            Action a = () => lexicalParser.Parse(null);
            a
                .Should()
                .Throw<ArgumentException>();
        }

        private static Tuple<string, Token[]>[] LexerCasesWithoutMarkdownTags =
        {
            new Tuple<string, Token[]>("plain text", new[]
            {
                new Token(MdType.Text, "plain text"),
            }),
            new Tuple<string, Token[]>("text_with_underscores", new[]
            {
                new Token(MdType.Text, "text"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "with"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "underscores"),
            }),
            new Tuple<string, Token[]>("text__with double__underscores", new[]
            {
                new Token(MdType.Text, "text"),
                new Token(MdType.Text, "__"),
                new Token(MdType.Text, "with double"),
                new Token(MdType.Text, "__"),
                new Token(MdType.Text, "underscores"),
            }),
            new Tuple<string, Token[]>("\\_tag escape\\_", new[]
            {
                new Token(MdType.Text, "\\"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "tag escape\\"),
                new Token(MdType.Text, "_"),
            }),
        };

        private static Tuple<string, Token[]>[] LexerCasesWithMarkdownTags =
        {
            new Tuple<string, Token[]>("_italic_", new[]
            {
                new Token(MdType.OpenEmphasis, "_"),
                new Token(MdType.Text, "italic"),
                new Token(MdType.CloseEmphasis, "_"),
            }),
            new Tuple<string, Token[]>("__bold__", new[]
            {
                new Token(MdType.OpenStrongEmphasis, "__"),
                new Token(MdType.Text, "bold"),
                new Token(MdType.CloseStrongEmphasis, "__"),
            }),
            new Tuple<string, Token[]>("__bold with _italic_ z__", new[]
            {
                new Token(MdType.OpenStrongEmphasis, "__"),
                new Token(MdType.Text, "bold with "),
                new Token(MdType.OpenEmphasis, "_"),
                new Token(MdType.Text, "italic"),
                new Token(MdType.CloseEmphasis, "_"),
                new Token(MdType.Text, " z"),
                new Token(MdType.CloseStrongEmphasis, "__"),
            }),
            new Tuple<string, Token[]>("_italic with __ignored bold__ z_", new[]
            {
                new Token(MdType.OpenEmphasis, "_"),
                new Token(MdType.Text, "italic with "),
                new Token(MdType.Text, "__"),
                new Token(MdType.Text, "ignored bold"),
                new Token(MdType.Text, "__"),
                new Token(MdType.Text, " z"),
                new Token(MdType.CloseEmphasis, "_"),
            }),
            new Tuple<string, Token[]>("_12_3", new[]
            {
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "12"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "3"),
            }),
            new Tuple<string, Token[]>("__42__42", new[]
            {
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "42"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "42"),
            }),
            new Tuple<string, Token[]>("a_a_", new[]
            {
                new Token(MdType.Text, "a"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "a"),
                new Token(MdType.Text, "_"),
            }),
            new Tuple<string, Token[]>("__непарные _символы не считаются выделением", new[]
            {
                new Token(MdType.Text, "__"),
                new Token(MdType.Text, "непарные "),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "символы не считаются выделением"),
            }),
            new Tuple<string, Token[]>("эти_ подчерки_ не считаются", new[]
            {
                new Token(MdType.Text, "эти"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, " подчерки"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, " не считаются"),
            }),
            new Tuple<string, Token[]>("эти _подчерки не считаются _", new[]
            {
                new Token(MdType.Text, "эти "),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "подчерки не считаются "),
                new Token(MdType.Text, "_"),
            }),
            new Tuple<string, Token[]>("___te_st___", new[]
            {
                new Token(MdType.OpenStrongEmphasis, "__"),
                new Token(MdType.OpenEmphasis, "_"),
                new Token(MdType.Text, "te"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "st"),
                new Token(MdType.CloseEmphasis, "_"),
                new Token(MdType.CloseStrongEmphasis, "__"),
            }),
            new Tuple<string, Token[]>("__\\_hello\\___", new[]
            {
                new Token(MdType.OpenStrongEmphasis, "__"),
                new Token(MdType.Text, "\\"),
                new Token(MdType.Text, "_"),
                new Token(MdType.Text, "hello\\"),
                new Token(MdType.Text, "_"),
                new Token(MdType.CloseStrongEmphasis, "__"),
            }),
        };

        [TestCaseSource(nameof(LexerCasesWithoutMarkdownTags))]
        [TestCaseSource(nameof(LexerCasesWithMarkdownTags))]
        public void Parse_ReturnsCorrectResult_AtStringWithoutMarkdownTags(Tuple<string, Token[]> data)
        {
            var result = lexicalParser.Parse(data.Item1);

            result.Should()
                .BeEquivalentTo(data.Item2);
        }
    }
}