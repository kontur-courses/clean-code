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

        #region Tag.Emphasize

        [Test, TestCaseSource(nameof(WrapInEmphasizeTagTestCases))]
        public void WrapInEmphasizeTag(string source, params Token[] expected)
        {
            MarkdownTokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable WrapInEmphasizeTagTestCases
        {
            get
            {
                yield return new TestCaseData("_people_ hello", new[]
                {
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " hello"),
                }).SetName("when tag is at the start");

                yield return new TestCaseData("hello _people_", new[]
                {
                    new Token(Tag.Raw, false, "hello "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Emphasize, false),
                }).SetName("when tag is at the end");

                yield return new TestCaseData("start _middle_ end!", new[]
                {
                    new Token(Tag.Raw, false, "start "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "middle"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " end!"),
                }).SetName("when tag is in the middle");

                yield return new TestCaseData("Word _another_ pretty _word_ !", new[]
                {
                    new Token(Tag.Raw, false, "Word "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "another"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " pretty "),
                    new Token(Tag.Emphasize, true),
                    new Token(Tag.Raw, false, "word"),
                    new Token(Tag.Emphasize, false),
                    new Token(Tag.Raw, false, " !"),
                }).SetName("with multiple tags");
            }
        }

        [Test, TestCaseSource(nameof(NotWrapInEmphasizeTagTestCases))]
        public void NotWrapInEmphasizeTag(string source, params Token[] expected)
        {
            MarkdownTokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable NotWrapInEmphasizeTagTestCases()
        {
            yield return new TestCaseData(@"hello \_people_", new[]
            {
                new Token(Tag.Raw, false, "hello _people_"),
            }).SetName("when first underscore is escaped");

            yield return new TestCaseData(@"hello \_people", new[]
            {
                new Token(Tag.Raw, false, "hello _people"),
            }).SetName("when first underscore is escaped without second underscore");

            yield return new TestCaseData(@"hello _people\_", new[]
            {
                new Token(Tag.Raw, false, @"hello _people_"),
            }).SetName("when second underscore is escaped");

            yield return new TestCaseData(@"hello people\_", new[]
            {
                new Token(Tag.Raw, false, @"hello people_"),
            }).SetName("when second underscore is escaped without first underscore");

            yield return new TestCaseData(@"hello \_people\_", new[]
            {
                new Token(Tag.Raw, false, @"hello _people_"),
            }).SetName("when both underscores are escaped");

            yield return new TestCaseData("start _end", new[]
            {
                new Token(Tag.Raw, false, "start _end"),
            }).SetName("when first underscore is unpaired");

            yield return new TestCaseData("start_ end", new[]
            {
                new Token(Tag.Raw, false, "start_ end"),
            }).SetName("when second underscore is unpaired");
        }

        #endregion

        #region Tag.Strong

        [Test, TestCaseSource(nameof(WrapInStrongTagTestCases))]
        public void WrapInStrongTag(string source, params Token[] expected)
        {
            MarkdownTokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable WrapInStrongTagTestCases
        {
            get
            {
                yield return new TestCaseData("__people__ hello", new[]
                {
                    new Token(Tag.Strong, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Strong, false),
                    new Token(Tag.Raw, false, " hello"),
                }).SetName("when tag is at the start");

                yield return new TestCaseData("hello __people__", new[]
                {
                    new Token(Tag.Raw, false, "hello "),
                    new Token(Tag.Strong, true),
                    new Token(Tag.Raw, false, "people"),
                    new Token(Tag.Strong, false),
                }).SetName("when tag is at the end");

                yield return new TestCaseData("start __middle__ end!", new[]
                {
                    new Token(Tag.Raw, false, "start "),
                    new Token(Tag.Strong, true),
                    new Token(Tag.Raw, false, "middle"),
                    new Token(Tag.Strong, false),
                    new Token(Tag.Raw, false, " end!"),
                }).SetName("when tag is in the middle");

                yield return new TestCaseData("Word __another__ pretty __word__ !", new[]
                {
                    new Token(Tag.Raw, false, "Word "),
                    new Token(Tag.Strong, true),
                    new Token(Tag.Raw, false, "another"),
                    new Token(Tag.Strong, false),
                    new Token(Tag.Raw, false, " pretty "),
                    new Token(Tag.Strong, true),
                    new Token(Tag.Raw, false, "word"),
                    new Token(Tag.Strong, false),
                    new Token(Tag.Raw, false, " !"),
                }).SetName("with multiple tags");
            }
        }

        [Test, TestCaseSource(nameof(NotWrapInStrongTagTestCases))]
        public void NotWrapInStrongTag(string source, params Token[] expected)
        {
            MarkdownTokenizer.Tokenize(source).Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        private static IEnumerable NotWrapInStrongTagTestCases()
        {
            yield return new TestCaseData(@"hello \__people__", new[]
            {
                new Token(Tag.Raw, false, @"hello __people__"),

            }).SetName("when first double underscore is escaped");

            yield return new TestCaseData(@"hello \__people", new[]
            {
                new Token(Tag.Raw, false, "hello __people"),
            }).SetName("when first double underscore is escaped without second double underscore");

            yield return new TestCaseData(@"hello __people\__", new[]
            {
                new Token(Tag.Raw, false, @"hello __people__"),
            }).SetName("when second double underscore is escaped");

            yield return new TestCaseData(@"hello people\__", new[]
            {
                new Token(Tag.Raw, false, @"hello people__"),
            }).SetName("when second double  underscore is escaped without first double underscore");

            yield return new TestCaseData(@"hello \__people\__", new[]
            {
                new Token(Tag.Raw, false, @"hello __people__"),
            }).SetName("when both double underscores are escaped");

            yield return new TestCaseData("start __end", new[]
            {
                new Token(Tag.Raw, false, "start __end"),
            }).SetName("when first double underscore is unpaired");

            yield return new TestCaseData("start__ end", new[]
            {
                new Token(Tag.Raw, false, "start__ end"),
            }).SetName("when second double underscore is unpaired");
        }

        #endregion
    }
}