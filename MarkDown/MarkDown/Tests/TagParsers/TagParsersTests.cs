using System;
using System.Collections.Generic;
using FluentAssertions;
using MarkDown.TagParsers;
using NUnit.Framework;

namespace MarkDown.Tests
{
    [TestFixture]
    public class TagParsersTests
    {
        private readonly List<TagParser> parsers = new List<TagParser>()
        {
            new EmTagParser(),
            new StrongTagParser()
        };

        [Test]
        public void GetParsedLine_ThrowsArgumentException_OnNull()
        {
            foreach (var parser in parsers)
            {
                Action action = () => parser.GetParsedLineFrom(null);
                action.Should().Throw<ArgumentException>();
            }
        }


    }
}