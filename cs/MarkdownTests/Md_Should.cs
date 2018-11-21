using System;
using System.Diagnostics;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Markdown;

namespace MarkdownTests
{
    [TestFixture]
    class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            var forbiddenSymbols = "0123456789".ToCharArray();
            var emTag = MakeTag("_", "<em>", new List<string> { "__" });
            var strongTag = MakeTag("__", "<strong>");
            var tagList = new List<Tag>
            {
                emTag,
                strongTag
            };
            md = new Md(tagList, forbiddenSymbols);
        }

        private Tag MakeTag(
            string md,
            string html,
            IEnumerable<string> ignoringNested = null)
            => new Tag(md, html, ignoringNested);

        [Test]
        public void ReturnEmptyString_WhenInputIsEmpty()
        {
            var input = "";

            var result = md.Render(input);

            result.Should().BeEmpty();
        }

        [Test]
        public void ReplaceOneGroundSymbolsToEmTags()
        {
            var input = "_word_";
            var expected = "<em>word</em>";

            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [Test]
        public void ReplaceTwoGroundSymbolsToStrongTags()
        {
            var input = "__word__";
            var expected = "<strong>word</strong>";

            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [TestCase("_ word_", "_ word_", TestName = "when whitespace after opener tag")]
        [TestCase("_word _", "_word _", TestName = "when whitespace before closer tag")]
        [TestCase("_word1_ 3_", "_word1_ 3_", TestName = "when tag inside digits")]
        [TestCase(@"\_word\_", "_word_", TestName = "when symbols are escaped")]
        [TestCase("_some __sentence__ here_", "<em>some __sentence__ here</em>", TestName = "when double grounding tags inside once grounding tags")]
        public void DoesntReplace(string input, string expected)
        {
            var result = md.Render(input);

            result.Should().Be(expected);
        }

        [Test]
        public void WorkWithLinearTimeHardness()
        {
            var input = @"\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__\_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_ \_quoted words\_
                \_quoted words\_ _unquoted_ __double sdaakfajsldfoehifwaef;sdfa;jhasfdh __ _f wfealkfsjad _ asfj;sdkjf;awehf;h
                asdfkha;wkjefh;ajehf ;kjasdhf;_ __ asdlfkjhal __ ___ asdfh_ _ sdfakjhkh__ dflakjhwefkjhsf;adkjh__";
            var time = DetectExecutionTime(md.Render, input);
            time.Should().BeLessThan(60);
        }

        private int DetectExecutionTime(Func<string, string> func, string input)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            func.Invoke(input);
            stopwatch.Stop();
            return stopwatch.Elapsed.Milliseconds;
        }
    }
}
