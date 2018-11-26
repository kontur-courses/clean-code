using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class TextParser_GetTokensFromDelimiters_Should
    {
        [Category("UnderscoreRule")]
        [TestCase("_delimiter_", 0, 11, TestName = "single underscore")]
        [TestCase("__delimiter__", 0, 13, TestName = "double underscore")]
        public void ReturnOneUnderscoreToken_WhenTwoDelimitersAtEdges(string text, int start, int length)
        {
            TextParser.For(text)
                      .Parse()
                      .Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(new PairedTagToken(0, length, text.Substring(start, length)));
        }

        [Category("UnderscoreRule")]
        [TestCase("_delimiter", 1, 9, TestName = "single single underscore")]
        [TestCase("_delimiter", 1, 9, TestName = "single double underscore")]
        //  [TestCase("__delimiter__", 2, 9, TestName = "double underscore")]
        public void ReturnStringToken_When(string text, int start, int length)
        {
            TextParser.For(text)
                      .Parse()
                      .Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(new StringToken(0, 10, text));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnNestedUnderscoreTokens()
        {
            const string text = "_a __b__ a_";
            var outerToken = new PairedTagToken(0, 11, text);
            var innerToken = new PairedTagToken(3, 5, "__b__") {ParentToken = outerToken};

            outerToken.InnerTokens = new List<Token>
            {
                innerToken,
            };

            TextParser.For(text)
                      .Parse()
                      .Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(outerToken, options => options.ExcludeMember(nameof(Token.ParentToken)));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnOneStringToken_WhenNoDelimiters()
        {
            const string text = "no text at all";

            TextParser.For(text)
                      .Parse()
                      .Should()
                      .HaveCount(1)
                      .And.Subject.First()
                      .ShouldBeEquivalentTo(new StringToken(0, 14, text));
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnStringTokenAndUnderscoreTokenAndStringToken_When()
        {
            const string text = "it`s a _del_ imiter";

            TextParser.For(text)
                      .Parse()
                      .ShouldBeEquivalentTo(new List<Token>
                      {
                          new StringToken(0, 7, "it`s a "),
                          new PairedTagToken(7, 5, "_del_"),
                          new StringToken(12, 7, " imiter")
                      });
        }

        [Category("UnderscoreRule")]
        [Test]
        public void ReturnUnderscoreTokenAndStringToken_When()
        {
            const string text = "_del_ imiter";

            TextParser.For(text)
                      .Parse()
                      .ShouldBeEquivalentTo(new List<Token>
                      {
                          new PairedTagToken(0, 5, "_del_"), new StringToken(5, 7, " imiter")
                      });
        }
    }
}
