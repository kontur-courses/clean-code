using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using Markdown;

namespace MarkdownTests
{
    public class ParserTests
    {
        private ITagsParser<MdTagWithIndex> _parser;
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
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5)});
            if (text == "__text__")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 6)});
            if (text == "#text")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5)});
            if (text == "#text\n and text")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5)});
            if (text == @"#_text\_ and_ text")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, text.Length)});
            if (text == "_нач_але, и в сер_еди_не, и в кон_це._")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 4), (17, 21), (33, 37)});

            if (text == "#text\n_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5), (6, 11)});
            if (text == "#text_text_")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, text.Length), (5, 10)});
            if (text == "# Заголовок __с _разными_ символами__")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 37), (12, 35), (16, 24)});
            if (text == @"\\_вот это будет выделено тегом_")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(2, 31)});
            if (text == "Внутри __двойного выделения _одинарное_ тоже__ работает")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(7, 44), (28, 38)});
            if (text == "Но не наоборот — внутри _одинарного __двойное__ не_ работает")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(24, 50)});
            if (text == "__a_b__")
                _valuesTagsIndexes.AddRange(new (int, int)[] {(0, 5)});
        }

        [TestCase("_text_")]
        [TestCase("__text__")]
        [TestCase("#text")]
        [TestCase("#text\n and text")]
        [TestCase("_нач_але, и в сер_еди_не, и в кон_це._")]
        public void MarkdownParser_GetIndexesTags_SimpleTests(string text)
        {
            InitializeValuesTags(text);
            var findedTags = _parser.GetIndexesTags(text);
            CheckReturnedIndexes(findedTags.ToList());
        }

        private void CheckReturnedIndexes(List<MdTagWithIndex> findedTags)
        {
            findedTags.Count.Should().Be(_valuesTagsIndexes.Count);
            for (int i = 0; i < findedTags.Count; i++)
                CheckReturnedIndex(findedTags[i], _valuesTagsIndexes[i].openTagIndex,
                    _valuesTagsIndexes[i].closeTagIndex);
        }

        private void CheckReturnedIndex(MdTagWithIndex tag, int openTagIndex, int closeTagIndex)
        {
            tag.OpenTagIndex.Should().Be(openTagIndex);
            tag.CloseTagIndex.Should().Be(closeTagIndex);
        }

        [TestCase("#text\n_text_")]
        [TestCase("#text_text_")]
        [TestCase("# Заголовок __с _разными_ символами__")]
        [TestCase(@"#_text\_ and_ text")]
        [TestCase(@"\\_вот это будет выделено тегом_")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает")]
        [TestCase("Но не наоборот — внутри _одинарного __двойное__ не_ работает")]
        [TestCase("__a_b__")]
        public void MarkdownParser_GetIndexesTags_MultipleTags(string text)
        {
            InitializeValuesTags(text);
            var findedTags = _parser.GetIndexesTags(text);
            CheckReturnedIndexes(findedTags.ToList());
        }

        [TestCase("__")]
        [TestCase("____")]
        [TestCase(@"\_Вот это\_")]
        [TestCase("В то же время выделение в ра_зных сл_овах не работает.")]
        [TestCase("Иначе эти _подчерки _не считаются")]
        [TestCase("Иначе эти_ подчерки_ не считаются")]
        [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением.")]
        [TestCase(
            "Подчерки внутри текста c цифрами_12_3 не считаются выделением и должны оставаться символами подчерка.")]
        [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков ни один из них не считается выделением.")]
        public void MarkdownParser_GetIndexesTags_ShouldReturnEmptyArray(string text)
        {
            var findedTags = _parser.GetIndexesTags(text);
            findedTags.Count().Should().Be(0);
        }
    }
}