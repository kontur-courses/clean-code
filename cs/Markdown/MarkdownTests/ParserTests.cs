using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    public class ParserTests
    {
        private ITagsParser<MdTag> _parser;

        [SetUp]
        public void SetUp()
        {
            
        }

        [TestCase("_", true, "_text_", 0, 5)]
        [TestCase("__", true, "__text__", 0, 6)]
        [TestCase("#", false, "#text", 0, 5)]
        [TestCase("#", false, "#text\n and text", 0, 5)]
        public void MarkdownParser_GetIndexesTags_ShouldReturnExpectedIndexes(string tagName, bool hasCloseTag, string text, int openTagIndex, int closeTagIndex)
        {
            var tags = new List<MdTag>();
            tags.Add(new MdTag(tagName, hasCloseTag));
            _parser = new MarkdownParser(tags);

            var findedTags = _parser.GetIndexesTags(text);
            foreach (var tag in findedTags)
            {
                CheckReturnedIndex(tag, openTagIndex, closeTagIndex);
            }
        }

        public void CheckReturnedIndex(MdTag tag, int openTagIndex, int closeTagIndex)
        {
            tag.OpenTagIndex.Should().Be(openTagIndex);
            tag.CloseTagIndex.Should().Be(closeTagIndex);
        }
    }
}