﻿using System;
using NUnit.Framework;
using FluentAssertions;
using Markdown.Readers;
using Markdown.Tokens;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextReader_Should
    {
        [TestCase(null, 0, TestName = "When text is null")]
        [TestCase("", 0, TestName = "When text is empty")]
        [TestCase("abc", -1, TestName = "When offset is negative")]
        [TestCase("abc", 5, TestName = "When offset is more than text's length")]
        public void ThrowArgumentException_When(string text, int offset)
        {
            var textReader = new TextReader("");
            Action readingAction = () => textReader.ReadToken(text, offset, options: null);
            readingAction.Should().Throw<ArgumentException>();
        }

        [Test]
        public void NotReadBannedSymbol()
        {
            var textReader = new TextReader("_");

            var (token, read) = textReader.ReadToken("abb_ccc", 1, null);

            read.Should().Be(2);
            token.Should().Be(new TextToken("bb"));
        }

        [Test]
        public void ReturnNullTokenAndZero_WhenReadingStartsWithBannedSymbol()
        {
            var textReader = new TextReader("_");

            var (token, read) = textReader.ReadToken("_abc", 0, null);
            
            token.Should().BeNull();
            read.Should().Be(0);
        }

        [Test]
        public void StopReading_OnFirstBannedSymbol()
        {
            var textReader = new TextReader("_*");

            var (token, read) = textReader.ReadToken("aa_bb*cc", 0, null);

            read.Should().Be(2);
            token.Should().Be(new TextToken("aa"));
        }

        [Test]
        public void StopReading_OnTheEndOfTheText()
        {
            var textReader = new TextReader("");

            var (token, read) = textReader.ReadToken("abcd", 0, null);

            read.Should().Be(4);
            token.Should().Be(new TextToken("abcd"));
        }

        [Test]
        public void StartReadingOnGivenIndex_WhenIdxMoreThanZero()
        {
            var textReader = new TextReader("");

            var (token, read) = textReader.ReadToken("aaabbb", 3, null);

            read.Should().Be(3);
            token.Should().Be(new TextToken("bbb"));
        }
    }
}