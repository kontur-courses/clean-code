using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_GetDelimitersPositions_Should
    {
        [Category("UnderscoreRule")]
        [Test]
        public void ReturnEmptyList_WhenNoDelimiters()
        {
            TextParser.For("abcd efg")
                      .GetDelimiterPositions()
                      .Delimiters.Should()
                      .BeEmpty();
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnFirstAndLastDelimiter()
        {
            TextParser.For("_del_")
                      .GetDelimiterPositions()
                      .Delimiters.Should()
                      .HaveCount(2)
                      .And.Subject.ShouldBeEquivalentTo(new[] {new Delimiter("_", 0), new Delimiter("_", 4)});
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnFirstAndLastDelimiter_OfDoubleUnderscore()
        {
            TextParser.For("__del__")
                      .GetDelimiterPositions()
                      .Delimiters.Should()
                      .HaveCount(2)
                      .And.Subject.ShouldBeEquivalentTo(new[] {new Delimiter("__", 0), new Delimiter("__", 5)});
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRule_WhenOneExistsOfThisRule()
        {
            TextParser.For("abcd__efg")
                      .GetDelimiterPositions()
                      .Delimiters.First()
                      .ShouldBeEquivalentTo(new Delimiter("__", 4));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRuleAndOneOfUnderscore_WhenThereAre3Underscores()
        {
            TextParser.For("abcd___efg")
                      .GetDelimiterPositions()
                      .Delimiters.ShouldBeEquivalentTo(new List<Delimiter> {new Delimiter("___", 4)});
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfUnderscoreRule_WhenOneExistsOfThisRule()
        {
            TextParser.For("abcd_efg")
                      .GetDelimiterPositions()
                      .Delimiters.Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(new Delimiter("_", 4));
        }
    }
}
