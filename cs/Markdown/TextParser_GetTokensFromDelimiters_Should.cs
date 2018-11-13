using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class TextParser_GetTokensFromDelimiters_Should
    {
        [SetUp]
        public void SetUp()
        {
            parser = new TextParser(new[] {new UnderscoreRule()});
        }

        private TextParser parser;

        [Category("UnderscoreRule")]
        [TestCase("_delimiter_", 0, 11, TestName = "single underscore")]
        [TestCase("__delimiter__", 0, 13, TestName = "double underscore")]
        public void ReturnOneUnderscoreToken_WhenTwoDelimitersAtEdges(string text, int start, int length)
        {
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            delimiters = parser.ValidatePairs(delimiters, text);
            parser.GetTokensFromDelimiters(delimiters, text)
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new UnderscoreToken(0, length, text.Substring(start, length)));
        }

        [Category("UnderscoreRule")]
        [TestCase("_delimiter", 1, 9, TestName = "single single underscore")]
        [TestCase("_delimiter", 1, 9, TestName = "single double underscore")]
        //  [TestCase("__delimiter__", 2, 9, TestName = "double underscore")]
        public void ReturnStringToken_When(string text, int start, int length)
        {
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            delimiters = parser.ValidatePairs(delimiters, text);
            parser.GetTokensFromDelimiters(delimiters, text)
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new StringToken(0, 10, text));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneStringToken_WhenNoDelimiters()
        {
            const string text = "no text at all";
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            delimiters = parser.ValidatePairs(delimiters, text);
            parser.GetTokensFromDelimiters(delimiters, text)
                  .Should()
                  .HaveCount(1)
                  .And.Subject.First()
                  .ShouldBeEquivalentTo(new StringToken(0, 14, text));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnStringTokenAndUnderscoreTokenAndStringToken_When()
        {
            var text = "it`s a _del_ imiter";
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            delimiters = parser.ValidatePairs(delimiters, text);
            parser.GetTokensFromDelimiters(delimiters, text)
                  .ShouldBeEquivalentTo(new List<Token>
                  {
                      new StringToken(0, 7, "it`s a "),
                      new UnderscoreToken(7, 5, "_del_"),
                      new StringToken(12, 7, " imiter")
                  });
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnUnderscoreTokenAndStringToken_When()
        {
            var text = "_del_ imiter";
            var delimiters = parser.GetDelimiterPositions(text);
            delimiters = parser.RemoveEscapedDelimiters(delimiters, text);
            delimiters = parser.RemoveNonValidDelimiters(delimiters, text);
            delimiters = parser.ValidatePairs(delimiters, text);
            parser.GetTokensFromDelimiters(delimiters, text)
                  .ShouldBeEquivalentTo(new List<Token>
                  {
                      new UnderscoreToken(0, 5, "_del_"), new StringToken(5, 7, " imiter")
                  });
        }
    }
}
