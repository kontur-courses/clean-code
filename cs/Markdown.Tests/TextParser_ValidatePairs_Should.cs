using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_ValidatePairs_Should
    {
        [TestCase("_a __ b_")]
        public void RemoveOneDelimiterBetweenPairedOthers(string text)
        {
            TextParser.For(text)
                      .GetDelimiterPositions()
                      .ValidatePairs()
                      .Delimiters.Should()
                      .HaveCount(2);
        }

        [Category("UnderscoreRule")]
        [TestCase("aa_ bb", TestName = "one underscore")]
        [TestCase("aa__ bb", TestName = "one  double underscore")]
        public void RemoveOneValidDelimiter(string text)
        {
            TextParser.For(text)
                      .GetDelimiterPositions()
                      .ValidatePairs()
                      .Delimiters.Should()
                      .BeEmpty();
        }

        [Category("UnderscoreRule")]
        [TestCase("_aa_ bb", TestName = "two underscore")]
        [TestCase("__aa__ bb", TestName = "two  double underscore")]
        public void SetPartnerDelimitersForEachPairedOnes(string text)
        {
            var delimiters = TextParser.For(text)
                                       .GetDelimiterPositions()
                                       .ValidatePairs()
                                       .Delimiters;
            delimiters[0]
                .Partner.ShouldBeEquivalentTo(delimiters[1]);
            delimiters[1]
                .Partner.ShouldBeEquivalentTo(delimiters[0]);
            delimiters[0]
                .IsOpening.Should()
                .BeTrue();
            delimiters[1]
                .IsClosing.Should()
                .BeTrue();
        }
    }
}
