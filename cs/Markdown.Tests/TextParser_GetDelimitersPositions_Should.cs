using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_GetDelimitersPositions_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new TextParser();//new TextParser(new ILexerRule[] { new PairedSingleTagRule('_'), new PairedDoubleTagRule('_'),  });

        }

        private TextParser parser;

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnEmptyList_WhenNoDelimiters()
        {
            parser.GetDelimiterPositions("abcd efg")
                  .Should()
                  .BeEmpty();
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnFirstAndLastDelimiter()
        {
            parser.GetDelimiterPositions("_del_")
                  .Should()
                  .HaveCount(2)
                  .And.Subject.ShouldBeEquivalentTo(new[] {new Delimiter("_", 0), new Delimiter("_", 4)});
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.GetDelimiterPositions("abcd__efg")
                  .First()
                  .ShouldBeEquivalentTo(new Delimiter("__", 4));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRuleAndOneOfUnderscore_WhenThereAre3Underscores()
        {
            parser.GetDelimiterPositions("abcd___efg")
                  .ShouldBeEquivalentTo(new List<Delimiter>() {new Delimiter("__", 4), new Delimiter("_", 6)});
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.GetDelimiterPositions("abcd_efg")
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new Delimiter("_", 4));
        }
    }
}
