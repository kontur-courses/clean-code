using System;
using FluentAssertions;
using Markdown.Readers;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class Reader_Should<TReader> where TReader : AbstractReader
    {
        public TReader defaultReader;

        [TestCase(null, 0, TestName = "When text is null")]
        [TestCase("", 0, TestName = "When text is empty")]
        [TestCase("abc", -1, TestName = "When offset is negative")]
        [TestCase("abc", 5, TestName = "When offset is more than text's length")]
        public void ThrowArgumentException_When(string text, int offset)
        {
            Action readingAction = () => defaultReader.ReadToken(text, offset, options: null);
            readingAction.Should().Throw<ArgumentException>();
        }
    }
}