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
        private List<MdTag> _tags;
        private List<(int openTagIndex, int closeTagIndex)> _valuesTagsIndexes;
        
        [SetUp]
        public void SetUp()
        {
            _tags = new List<MdTag>()
            {
                new MdTag("_", true),
                new MdTag("__", true),
                new MdTag("#", false)
            };
            _parser = new MarkdownParser(_tags);
            _valuesTagsIndexes = new List<(int openTagIndex, int closeTagIndex)>();
        }

        private void InitializeValuesTags(string text)
        {
            if (text == "_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5), (-1, -1), (-1, -1)});
            if (text == "__text__")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(-1, -1), (0, 6), (-1, -1)});
            if (text == "#text")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(-1, -1), (-1, -1), (0, 5)});
            if (text == "#text\n and text")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(-1, -1), (-1, -1), (0, 5)});

            
            if (text == "_text___text__")
                _valuesTagsIndexes.AddRange(new (int, int)[]{(0, 5), (6, 12), (-1, -1)});
            if(text == "__text___text_")
                _valuesTagsIndexes.AddRange(new (int, int)[]{(8, 13), (0, 6), (-1, -1)});
            if(text == "#text\n_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[]{(6, 11), (-1, -1), (0, 5)});
            if(text == "#text_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[]{(5, 10), (-1, -1), (0, text.Length)});
            if(text == "__text__#text_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[]{(13, 18), (0, 6), (8, text.Length)});
        }
        
        [TestCase("_text_")]
        [TestCase("__text__")]
        [TestCase( "#text")]
        [TestCase("#text\n and text")]
        public void MarkdownParser_GetIndexesTags_SimpleTests(string text)
        {
            InitializeValuesTags(text);
            var findedTags = _parser.GetIndexesTags(text);
            CheckReturnedIndexes(findedTags);
        }

        private void CheckReturnedIndexes(List<MdTag> findedTags)
        {
            //italic tag (_)
            CheckReturnedIndex(findedTags[0], _valuesTagsIndexes[0].openTagIndex, _valuesTagsIndexes[0].closeTagIndex);
            //strong tag (__)
            CheckReturnedIndex(findedTags[1], _valuesTagsIndexes[1].openTagIndex, _valuesTagsIndexes[1].closeTagIndex);
            //header tag (#)
            CheckReturnedIndex(findedTags[2], _valuesTagsIndexes[2].openTagIndex, _valuesTagsIndexes[2].closeTagIndex);
        }

        private void CheckReturnedIndex(MdTag tag, int openTagIndex, int closeTagIndex)
        {
            tag.OpenTagIndex.Should().Be(openTagIndex);
            tag.CloseTagIndex.Should().Be(closeTagIndex);
        }
        
        [TestCase("_text___text__")]
        [TestCase("__text___text_")]
        [TestCase("#text\n_text_")]
        [TestCase("#text_text_")]
        [TestCase("__text__#text_text_")]
        public void MarkdownParser_GetIndexesTags_MultipleTags(string text)
        {
            InitializeValuesTags(text);
            var findedTags = _parser.GetIndexesTags(text);
            CheckReturnedIndexes(findedTags);
        }
        
        private MdTag GetTagFromList(List<MdTag> tags, string openTag)
        {
            foreach (var tag in tags)
            {
                if (tag.OpenTag == openTag)
                    return tag;
            }

            throw new ArgumentException("Tag not found");
        }
    }
}