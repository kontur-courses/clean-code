using Markdown;
using NUnit.Framework;
using FluentAssertions;

namespace Markdown.Tests
{
    [TestFixture]
    class MarkdownTagsLibrary_Should
    {
        [TestCase("_", 0, TagType.Italics, TestName = "Italic token, one underscore")]
        [TestCase("__", 0, TagType.Bold, TestName = "Bold token, one underscore")]
        [TestCase("__", 1, TagType.Italics, TestName = "Bold token, two underscore")]
        [TestCase("___", 1, TagType.Bold, TestName = "Bold token, three underscore ")]
        
        public void ContainsInTagAssociations_InputSimpleString_ReturnTrueAndExpectedTag(string input, int index, TagType expected)
        {
            var containsBool = MarkdownTagsLibrary.TryToGetUsableTagInAssociations(input, index, out var result);
            
            containsBool.Should().BeTrue();
            result.Should().NotBeNull();
            result.Type.Should().Be(expected);
        }

        [TestCase(" _", 0, TestName = "Whitespace")]
        [TestCase("A_", 0, TestName = "Letter")]
        [TestCase("3_2", 0, TestName = "Around digits")]
        [TestCase("3_", 0, TestName = "Digits")]
        public void ContainsInTagAssociations_InputSimpleString_ReturnFalseAndNullTag(string input, int index)
        {
            var containsBool = MarkdownTagsLibrary.TryToGetUsableTagInAssociations(input, index, out var result);

            containsBool.Should().BeFalse();
            result.Should().BeNull();
        }

        
    }
}
