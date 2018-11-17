using System;
using System.Collections;
using Markdown.Tokenizing;
using NUnit.Framework;
using FluentAssertions;

namespace MarkdownTests
{
    [TestFixture]
    public class MarkdownTokenizerShould
    {
        private Tokenizer tokenizer;

        [SetUp]
        public void SetUp()
        {
            tokenizer = new Tokenizer(new Markdown.Languages.MarkdownLanguage());
        }

        [Test, TestCaseSource(nameof(InvalidStringsTestCases))]
        public void ProcessInvalidStringCorrectly(string source)
        {
            Action act = () => tokenizer.Tokenize(source);

            act.Should().ThrowExactly<ArgumentException>();
        }

        private static IEnumerable InvalidStringsTestCases
        {
            get
            {
                yield return new TestCaseData("").SetName("EmptyString");
                yield return new TestCaseData(null).SetName("NullString");
            }
        }


        [Test, TestCaseSource(nameof(EmphasizeTagTestCases))]
        public void ProcessEmphasizeTagsCorrectly(string source, params Token[] expected)
        {
            tokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable EmphasizeTagTestCases
        {
            get
            {
                yield return new TestCaseData("_people_, hello", new[]
                {
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, ", hello"),
                }).SetName("OnTagAtTheStart");

                yield return new TestCaseData("hello _people_", new[]
                {
                    new Token(Tag.Raw, false, "hello "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                }).SetName("OnTagAtTheEnd");

                yield return new TestCaseData("_hello_ dear _people_!", new[]
                {
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "hello"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " dear "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, "!"),
                }).SetName("OnMultipleTags");

                yield return new TestCaseData(@"hello \_people\_", new[]
                {
                    new Token(Tag.Raw, false, @"hello \_people\_"),
                }).SetName("OnEscapedCharacters");

                yield return new TestCaseData(".. _hello_ _people !", new[]
                {
                    new Token(Tag.Raw, false, ".. "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "hello"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " _people !"),
                }).SetName("OnUnpairedTags");
            }
        }
    }
}
