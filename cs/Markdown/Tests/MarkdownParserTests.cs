using System;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class MarkdownParserTests
    {
        private readonly MarkdownParser parser = new MarkdownParser(new TokenInfo());

        [TestCase("_", TestName = "Em tag escaped")]
        [TestCase("__", TestName = "Strong tag escaped")]
        public void Parse_ShouldNotParse_WhenEscaped(string mdTag)
        {
            var expectedToken = new RootToken(mdTag + "ba cf" + mdTag);

            var actualToken = parser.Parse($"\\{mdTag}ba cf\\{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldMarkTokenAsInvalid_WhenStrongInsideEm()
        {
            var expectedToken = new RootToken();
            var strongToken = new Token(3, "__", "strong", "bb bb", true);
            var emToken = new Token(0, "_", "em", "aa  aa", true, true);
            emToken.AddNestedToken(strongToken);
            expectedToken.AddNestedToken(emToken);

            var actualToken = parser.Parse("_aa __bb bb__ aa_");

            actualToken.IsValid.Should().Be(expectedToken.IsValid);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenSpaceAfterOpenTag()
        {
            var expectedToken = new RootToken("__ aa aa__");

            var actualToken = parser.Parse("__ aa aa__");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenSpaceBeforeCloseTag()
        {
            var expectedToken = new RootToken("__aa aa __ ");

            var actualToken = parser.Parse("__aa aa __ ");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldNotParse_WhenTagsInsideText()
        {
            var expectedToken = new RootToken("aa__bb__cc");

            var actualToken = parser.Parse("aa__bb__cc");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("test", TestName = "word when no tags")]
        [TestCase("test  test", TestName = "words when no tags")]
        [TestCase("     test  test    ", TestName = "words with edge spaces when no tags")]
        public void Parse_ShouldParse(string markdown)
        {   // markdown is equal to token data here
            var expectedToken = new RootToken(markdown);

            var actualToken = parser.Parse(markdown);

            expectedToken.Should().BeEquivalentTo(actualToken);
        }
        
        [TestCase("_", "em", TestName = "Em tag")]
        [TestCase("__", "strong", TestName = "Strong tag")]
        public void Parse_ShouldParse_WhenSingleNotNested(string mdTag, string htmlTag)
        {
            var expectedToken = new RootToken();
            expectedToken.AddNestedToken(new Token(0, mdTag, htmlTag, "test tt", true, true));

            var actualToken = parser.Parse($"{mdTag}test tt{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [TestCase("_", "em", TestName = "Em tags")]
        [TestCase("__", "strong", TestName = "Strong tags")]
        public void Parse_ShouldParse_WhenMultipleNotNested(string mdTag, string htmlTag)
        {
            var expectedToken = new RootToken(" ");
            expectedToken.AddNestedToken(new Token(0, mdTag, htmlTag, "test t", true, true));
            expectedToken.AddNestedToken(new Token(1, mdTag, htmlTag, "abc d", true, true));

            var actualToken = parser.Parse($"{mdTag}test t{mdTag} {mdTag}abc d{mdTag}");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldParse_WhenMultipleNotNestedEmAndStrongTags()
        {
            var expectedToken = new RootToken(" ");
            expectedToken.AddNestedToken(new Token(0, "_", "em", "test t", true, true));
            expectedToken.AddNestedToken(new Token(1, "__", "strong", "abc c", true, true));

            var actualToken = parser.Parse("_test t_ __abc c__");
            Console.WriteLine(actualToken.ToHtml());
            actualToken.Should().BeEquivalentTo(expectedToken);
        }

        [Test]
        public void Parse_ShouldParse_WhenEmInsideStrong()
        {
            var expectedToken = new RootToken();
            var emToken = new Token(3, "_", "em", "bb bb", true, true);
            var strongToken = new Token(0, "__", "strong", "aa  aa", true, true);
            strongToken.AddNestedToken(emToken);
            expectedToken.AddNestedToken(strongToken);

            var actualToken = parser.Parse("__aa _bb bb_ aa__");

            actualToken.Should().BeEquivalentTo(expectedToken);
        }
    }
}