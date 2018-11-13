using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class TextParser_RemoveEscapedDelimiters_Should
    {
        private TextParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(null);
        }
        [Category("UnderscoreRule")]
        [Test]
        public void NotChangeDelimiters_WhenNoEscaping()
        {
            parser.AddRule(new UnderscoreRule());
            var text = "ab_c_";
            var delimiters = parser.GetDelimiterPositions(text);
            parser.RemoveEscapedDelimiters(delimiters, text)
                  .Should()
                  .BeEquivalentTo(delimiters);
        }
        [Category("UnderscoreRule")]
        [Test]
        public void RemoveEscapedDoubleUnderscoreAndPutUnderscore_WhenDoubleIsEscaped()
        {
            parser.AddRule(new UnderscoreRule());
            var text = "ab\\__c";
            var delimiters = parser.GetDelimiterPositions(text);
            parser.RemoveEscapedDelimiters(delimiters, text)
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new Delimiter("_", 4));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void RemoveEscapedUnderscore_WhenOneExists()
        {
            parser.AddRule(new UnderscoreRule());
            var text = "ab\\_c";
            var delimiters = parser.GetDelimiterPositions(text);
            parser.RemoveEscapedDelimiters(delimiters, text)
                  .Should()
                  .BeEmpty();
        }

        [Category("UnderscoreRule")]
        [Test]
        public void RemoveTwoEscapedUnderscores_WhenTheyFollowEachOther()
        {
            parser.AddRule(new UnderscoreRule());
            var text = "ab\\_\\_c";
            var delimiters = parser.GetDelimiterPositions(text);
            parser.RemoveEscapedDelimiters(delimiters, text)
                  .Should()
                  .BeEmpty();
        }

        

    }
}