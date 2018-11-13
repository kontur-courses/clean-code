using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
namespace Markdown
{
    [TestFixture]
    public class TextParser_GetDelimitersPositions_Should
    {
        private TextParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(new[] { new UnderscoreRule() });
        }


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
                  .HaveCount(2).And.Subject.ShouldBeEquivalentTo(new[] { new Delimiter("_",0), new Delimiter("_",4), });
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd__efg")
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new Delimiter("__", 4));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfDoubleUnderscoreRuleAndOneOfUnderscore_WhenThereAre3Underscores()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd___efg")
                  .Should()
                  .HaveCount(2)
                  .And.Subject.ShouldBeEquivalentTo(new[] { new Delimiter("__", 4), new Delimiter("_", 6) });
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneDelimiterOfUnderscoreRule_WhenOneExistsOfThisRule()
        {
            parser.AddRule(new UnderscoreRule());
            parser.GetDelimiterPositions("abcd_efg")
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new Delimiter("_", 4));
        }

       
    }
}