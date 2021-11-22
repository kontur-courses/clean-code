using System.Collections.Generic;
using FluentAssertions;
using Markdown.Parser;
using Markdown.Tags;
using Markdown.Tokens;
using NUnit.Framework;

namespace MarkdownTests
{
    public class TextParser_Tests
    {
        private TextParser parser;
        
        [SetUp]
        public void SetUp()
        {
            var availableTags = new List<Tag>()
            {
                new BoldTag(), new HeaderTag(), new ItalicsTag()
            };

            parser = new TextParser(availableTags);
        }

        [TestCase("aaabbcc", TestName = "WhenNoTagsInText")]
        [TestCase("ab _ac __def as_ xcv__", TestName = "WhenPairTagsIntersect")]
        [TestCase("_asd", TestName = "WhenPairTagHasNoEnd")]
        [TestCase("asd_", TestName = "WhenPairTagHasNoStart")]
        [TestCase("_asd__", TestName = "WhenStartAndEndAreDifferentTags")]
        [TestCase(@"\_asd_", TestName = "WhenTagIsEscaped")]
        public void GetTokens_ReturnsEmptyCollection(string text)
        {
            var tokens = parser.GetTokens(text);

            tokens.Should().BeEmpty();
        }

        [Test]
        public void GetTokens_ReturnsItalicsTagToken_WhenItalicsTagInText()
        {
            var text = "aa _abc_ abb";
            var expectedTokens = new List<TagToken>() { new TagToken(new ItalicsTag(), 3, 7, "_abc_") };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
        
        [Test]
        public void GetTokens_ReturnsBoldTagToken_WhenBoldTagInText()
        {
            var text = "aa __abc__ abb";
            var expectedTokens = new List<TagToken>() { new TagToken(new BoldTag(), 3, 9, "__abc__") };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsHeaderTagToken_WhenHeaderTagInText()
        {
            var text = "# aaabbbccc";
            var expectedTokens = new List<TagToken>() { new TagToken(new HeaderTag(), 0, 10, "# aaabbbccc") };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
        
        [Test]
        public void GetTokens_ReturnsMultipleTokens_WhenItalicsAndBoldTagsInText()
        {
            var text = "aa _bc_ __def__ as";
            var expectedTokens = new List<TagToken>
            {
                new TagToken(new ItalicsTag(), 3, 6, "_bc_"),
                new TagToken(new BoldTag(), 8, 14, "__def__")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsMultipleTokens_WhenItalicsTagIsNestedInBoldTag()
        {
            var text = "__ab _cdef_ ab__";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new BoldTag(), 0, 15, "__ab _cdef_ ab__"),
                new TagToken(new ItalicsTag(), 5, 10, "_cdef_")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsMultipleTokens_WhenPairTagsNestedInHeader()
        {
            var text = "# __asd__ qwe _zxc_";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new HeaderTag(), 0, 18, "# __asd__ qwe _zxc_"),
                new TagToken(new BoldTag(), 2, 8, "__asd__"),
                new TagToken(new ItalicsTag(), 14, 18, "_zxc_")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsItalicsTagToken_WhenTagInBegginingOfWord()
        {
            var text = "в _нач_але слова";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new ItalicsTag(), 2, 6, "_нач_")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsItalicsTagToken_WhenTagInMiddleOfWord()
        {
            var text = "в сер_еди_не слова";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new ItalicsTag(), 5, 9, "_еди_")
            };
            
            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsItalicsTagToken_WhenTagInEndOfWord()
        {
            var text = "в кон_це_ слова";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new ItalicsTag(), 5, 8, "_це_")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }

        [Test]
        public void GetTokens_ReturnsItalicsTagToken_WhenEscapeCharIsEscaped()
        {
            var text = @"\\_asd_";
            var expectedTokens = new List<TagToken>()
            {
                new TagToken(new ItalicsTag(), 2, 6, "_asd_")
            };

            var tokens = parser.GetTokens(text);

            tokens.Should().BeEquivalentTo(expectedTokens);
        }
    }
}