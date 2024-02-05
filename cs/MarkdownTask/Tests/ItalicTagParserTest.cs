using FluentAssertions;
using NUnit.Framework;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class ItalicTagParserTests
    {
        [Test]
        public void Parse_WithEmptyString_ShouldReturnEmptyCollection()
        {
            // Arrange
            string text = string.Empty;

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_StringWitoutTags_ShouldReturnEmptyCollection()
        {
            // Arrange
            string text = "no tags";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithItalicTags_ShouldReturnCorrectTokens()
        {
            // Arrange
            string text = "_hello_world";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [TestCase(@"aaa\_bbb_cc")]
        [TestCase(@"aaa_bbb\_cc")]
        [TestCase(@"aaa\_bbb\_cc")]
        public void Parse_EscapedTags_ShouldNotParsed(string text)
        {
            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithEscapedEscapeChar_ShouldReturnCorrectTokens()
        {
            // Arrange
            string text = @"aaa\\_bbb_";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_WithMultipleItalicTags_ShouldReturnCorrectTokens()
        {
            // Arrange
            string text = @"aaaa_bbb_ccc_ddd_e_ff_";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().HaveCount(6);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_NotFollowedByLetters_ShouldNotReturnTokens()
        {
            // Arrange
            string text = "a_ b_";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().BeEmpty();
        }

        [TestCase("_a_b")]
        [TestCase("a_b_c")]
        [TestCase("a_b_")]
        public void Parse_WithTagsInOneWord_ShouldReturnCorrectTokens(string text)
        {
            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_WithTagsInMiddleOfDiffrentWords_ShouldNotReturnTokens()
        {
            // Arrange
            string text = "fi_rst sec_ond";

            // Act
            ICollection<Token> result = new ItalicTagParser().Parse(text);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
