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
        [TestCase("", TestName = "OnEmptyString")]
        [TestCase(null, TestName = "OnNullString")]
        public void ThrowExpetionOnInvalidString(string source)
        {
            Action act = () => MarkdownTokenizer.Tokenize(source);

            act.Should().ThrowExactly<ArgumentException>().And.Message.Should().Contain("can't be null or empty");
        }

        [Test, TestCaseSource(nameof(EmphasizeTagTestCases))]
        public void ProcessEmphasizeTagsCorrectly(string source, params Token[] expected)
        {
            MarkdownTokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable EmphasizeTagTestCases
        {
            get
            {
                yield return new TestCaseData("_people_ hello", new[]
                {
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " hello"),
                }).SetName("OnTagAtTheStart");

                yield return new TestCaseData("hello _people_", new[]
                {
                    new Token(Tag.Raw, false, "hello "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                }).SetName("OnTagAtTheEnd");

                yield return new TestCaseData("_hello_ dear _people_ !", new[]
                {
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "hello"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " dear "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " !"),
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
