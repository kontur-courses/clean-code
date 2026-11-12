using NUnit.Framework;
using Markdown;
using FluentAssertions;
using MarkdownUnitTests.Extensions;
using Markdown.Models.Interfaces;
using Markdown.Models;

namespace MarkdownUnitTests
{
    [TestFixture]
    public class InlineParserTests
    {
        private IInlineParser parser;

        [SetUp]
        public void Setup()
        {
            parser = new InlineParser();
        }

        [Test]
        public void ParseToTokens_ThrowsArgumentNullException_WhenNull()
        {
            string line = null;

            var action = () => parser.ParseToTokens(line);

            action.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void ParseToTokens_ReturnsEmptyList_WhenEmptyLine()
        {
            var line = "";

            var result = parser.ParseToTokens(line);

            result.Should().BeEmpty();
        }

        [TestCase("text")]
        [TestCase("   text with spaces   ")]
        [TestCase("13012007")]
        public void ParseToTokens_ReturnsListWithOneElement_WhenValidLineWithoutTags(string line)
        {
            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenBackslashBeforeTag()
        {
            var line = "\\_text\\_";

            var result = parser.ParseToTokens(line);
            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenOnlyBackslashes()
        {
            var line = "\\\\";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [Test]
        public void ParseToTokens_ReturnsList_WhenSingleEmTokenWithText()
        {
            var line = "<em>text</em>";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "_" },
                Children = new[] { new { Content = "text" } }
            });
        }

        [Test]
        public void ParseToTokens_ReturnsList_WhenSingleStrongTokenWithText()
        {
            var line = "<strong>text</strong>";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "__" },
                Children = new[] { new { Content = "text" } }
            });
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenTagWithSpaces(string tagText)
        {
            var line = $"{tagText} text {tagText}";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenUnclosedTag(string tagText)
        {
            var line = $"{tagText}text";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });

            line = $"text{tagText}";

            result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [TestCase("_")]
        [TestCase("__")]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenTagsInInvalidPlace(string tagText)
        {
            var line = $"{tagText}tex{tagText}t";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = line });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithManyTokens_WhenMultiplyTags()
        {
            var line = "_a lot_ of __tags__";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(3);

            result[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "_" },
                Children = new[] { new { Content = "a lot" } }
            });

            result[1].AsText().Should().BeEquivalentTo(new { Content = " of " });

            result[2].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "__" },
                Children = new[] { new { Content = "tags" } }
            });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithManyTagTokens_WhenStrongContainsEmInside()
        {
            var line = "__strong and _em_ text__";

            var result = parser.ParseToTokens(line);

            result.Should().HaveCount(1);
            result[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "__" },
                Children = new object[]
                {
                    new { Content = "strong and " },
                    new
                    {
                        Tag = new { MarkdownSymbol = "_" },
                        Children = new[] { new { Content = "em" } }
                    },
                    new { Content = " text" }
                }
            });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithOneTagToken_WhenEmContainsStrongInside()
        {
            var result = parser.ParseToTokens("_text __strong__ text_");

            result.Should().HaveCount(1);
            result[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "_" },
                Children = new[] { new { Content = "text __strong__ text" } }
            });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenLineContainsNewline()
        {
            var result = parser.ParseToTokens("first\nsecond");

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = "first\nsecond" });
        }

        [Test]
        public void ParseToTokens_ReturnsListWithSingleToken_WhenTagSpansNewline()
        {
            var result = parser.ParseToTokens("_first\nsecond_");

            result.Should().HaveCount(1);
            result[0].AsText().Should().BeEquivalentTo(new { Content = "_first\nsecond_" });
        }

        [Test]
        public void ParseToTokens_ReturnsCorrectLists_WhenCalledForMultipleLines()
        {
            var result1 = parser.ParseToTokens("_first_");
            var result2 = parser.ParseToTokens("__second__");
            var result3 = parser.ParseToTokens("plain text");

            result1.Should().HaveCount(1);
            result1[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "_" },
                Children = new[] { new { Content = "first" } }
            });

            result2.Should().HaveCount(1);
            result2[0].AsTag().Should().BeEquivalentTo(new
            {
                Tag = new { MarkdownSymbol = "__" },
                Children = new[] { new { Content = "second" } }
            });

            result3.Should().HaveCount(1);
            result3[0].AsText().Should().BeEquivalentTo(new { Content = "plain text" });
        }
    }
}