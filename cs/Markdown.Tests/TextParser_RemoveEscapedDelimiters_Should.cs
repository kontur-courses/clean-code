using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_RemoveEscapedDelimiters_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(); //new ILexerRule[] {new PairedDoubleTagRule('_'), new PairedSingleTagRule('_')});
        }

        private TextParser parser;

        [Category("UnderscoreRule")]
        [Test]
        public void NotChangeDelimiters_WhenNoEscaping()
        {
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
            var text = "ab\\_\\_c";
            var delimiters = parser.GetDelimiterPositions(text);
            parser.RemoveEscapedDelimiters(delimiters, text)
                  .Should()
                  .BeEmpty();
        }
    }
}
