using FluentAssertions;
using NUnit.Framework;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownTests
{
    [TestFixture]
    public class PairedTagsParserTests
    {
        [Test]
        public void Parse_WithEmptyString_ShouldReturnEmptyCollection()
        {
            string text = string.Empty;

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_StringWitoutTags_ShouldReturnEmptyCollection()
        {
            string text = "no tags";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithItalicTags_ShouldReturnCorrectTokens()
        {
            string text = "_hello_world";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [TestCase(@"aaa\_bbb_cc")]
        [TestCase(@"aaa_bbb\_cc")]
        [TestCase(@"aaa\_bbb\_cc")]
        public void Parse_EscapedTags_ShouldNotParsed(string text)
        {
            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithEscapedEscapeChar_ShouldReturnCorrectTokens()
        {
            string text = @"aaa\\_bbb_";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_WithMultipleItalicTags_ShouldReturnCorrectTokens()
        {
            string text = @"aaaa_bbb_ccc_ddd_e_ff_";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().HaveCount(6);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_NotFollowedByLetters_ShouldNotReturnTokens()
        {
            string text = "a_ b_";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().BeEmpty();
        }

        [TestCase("_a_b")]
        [TestCase("a_b_c")]
        [TestCase("a_b_")]
        [TestCase("a_b_ ссс")]
        [TestCase("aaa _a_b")]
        public void Parse_WithTagsInOneWord_ShouldReturnCorrectTokens(string text)
        {
            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Italic);
        }

        [Test]
        public void Parse_WithTagsInMiddleOfDiffrentWords_ShouldNotReturnTokens()
        {
            string text = "fi_rst sec_ond";

            ICollection<Token> result = new PairedTagsParser("_", TagType.Italic).Parse(text);

            result.Should().BeEmpty();
        }

        [Test]
        public void Parse_WithStrongTags_ShouldReturnCorrectTokens()
        {
            string text = "__hello__ world";

            ICollection<Token> result = new PairedTagsParser("__", TagType.Strong).Parse(text);

            result.Should().HaveCount(2);
            result.Should().OnlyContain(t => t.TagType == TagType.Strong);
        }
    }
}
