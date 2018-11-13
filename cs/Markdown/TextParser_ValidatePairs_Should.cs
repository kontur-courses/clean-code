using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class TextParser_ValidatePairs_Should
    {
        private TextParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(new []{new UnderscoreRule()});
        }
        [Category("UnderscoreRule")]
        [TestCase("aa_ bb", TestName = "one underscore")]
        [TestCase("aa__ bb", TestName = "one  double underscore")]
        public void RemoveOneValidDelimiter(string text)
        {
            parser.AddRule(new UnderscoreRule());
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            parser.ValidatePairs(delimiters, text)
                  .Should()
                  .BeEmpty();
        }
        [Category("UnderscoreRule")]
        [TestCase("_aa_ bb", TestName = "two underscore")]
        [TestCase("__aa__ bb", TestName = "two  double underscore")]
        public void SetPartnerDelimitersForEachPairedOnes(string text)
        {
            parser.AddRule(new UnderscoreRule());
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
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