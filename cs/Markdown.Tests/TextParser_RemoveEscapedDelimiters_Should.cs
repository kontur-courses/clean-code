using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_RemoveEscapedDelimiters_Should
    {
        [Category("UnderscoreRule")]
        [Test]
        public void NotChangeDelimiters_WhenNoEscaping()
        {
            var processor = TextParser.For("ab_c_")
                                      .GetDelimiterPositions();
            processor.RemoveEscapedDelimiters()
                     .Delimiters.Should()
                     .BeEquivalentTo(processor.Delimiters);
        }

        [Category("UnderscoreRule")]
        [Test]
        public void RemoveEscapedDoubleUnderscoreAndPutUnderscore_WhenDoubleIsEscaped()
        {
            TextParser.For("ab\\__c")
                      .GetDelimiterPositions()
                      .RemoveEscapedDelimiters()
                      .Delimiters.Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(new Delimiter("_", 4));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void RemoveEscapedUnderscore_WhenOneExists()
        {
            TextParser.For("ab\\_c")
                      .GetDelimiterPositions()
                      .RemoveEscapedDelimiters()
                      .Delimiters.Should()
                      .BeEmpty();
        }

        [Category("UnderscoreRule")]
        [Test]
        public void RemoveTwoEscapedUnderscores_WhenTheyFollowEachOther()
        {
            TextParser.For("ab\\_\\_c")
                      .GetDelimiterPositions()
                      .RemoveEscapedDelimiters()
                      .Delimiters.Should()
                      .BeEmpty();
        }
    }
}
