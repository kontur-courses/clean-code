using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_ValidatePairs_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(new[] {new UnderscoreRule()});
        }

        private TextParser parser;

        [Category("UnderscoreRule")]
        [TestCase("aa_ bb", TestName = "one underscore")]
        [TestCase("aa__ bb", TestName = "one  double underscore")]
        public void RemoveOneValidDelimiter(string text)
        {
            var delimiters = GetDelimiters(text);
            parser.ValidatePairs(delimiters, text)
                  .Should()
                  .BeEmpty();
        }

        private List<Delimiter> GetDelimiters(string text)
        {
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            return delimiters;
        }

        [Category("UnderscoreRule")]
        [TestCase("_aa_ bb", TestName = "two underscore")]
        [TestCase("__aa__ bb", TestName = "two  double underscore")]
        public void SetPartnerDelimitersForEachPairedOnes(string text)
        {
            var delimiters = GetDelimiters(text);

            delimiters = parser.ValidatePairs(delimiters, text);
            delimiters[0]
                .Partner.ShouldBeEquivalentTo(delimiters[1]);
            delimiters[1]
                .Partner.ShouldBeEquivalentTo(delimiters[0]);
            delimiters[0]
                .IsFirst.Should()
                .BeTrue();
            delimiters[1]
                .IsLast.Should()
                .BeTrue();
        }
    }
}
