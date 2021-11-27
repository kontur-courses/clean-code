using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FluentAssertions.Extensions;
using Markdown.Converters;
using Markdown.MdTags;
using NUnit.Framework;

namespace Markdown.Tests
{
    public class TagsConverterTests
    {
        [TestCase(null, TestName = "text is null")]
        [TestCase("", TestName = "text is empty")]
        [TestCase("    ", TestName = "text is whitespace")]
        [TestCase("lol", TestName = "text do not contains tags")]
        [TestCase("_", TestName = "text contains not closed tag")]
        public void GetAllTags_ReturnsEmptyList(string text)
        {
            TagsConverter.GetAllTags(text).Should().BeEmpty();
        }

        [TestCase("_a_", TagType.Italics, 0, 2, TestName = "italics tag")]
        [TestCase("#a", TagType.Title, 0, 2, TestName = "title tag")]
        [TestCase("__a__", TagType.StrongText, 1, 4, TestName = "strong text tag")]
        [TestCase("*a*", TagType.UnnumberedList, 0, 2, TestName = "unnumbered list tag")]
        public void GetAllTags_RecognizesSingleTag(string text, TagType expectedType, int expectedStart,
            int expectedEnd)
        {
            var tags = TagsConverter.GetAllTags(text);
            tags.Count.Should().Be(1);
            tags.Pop().Should().BeEquivalentTo(TagBuilder.OfType(expectedType).WithBounds(expectedStart, expectedEnd));
        }

        [Test]
        public void GetAllTags_RecognizesManyTagsOfTheSameType()
        {
            var text = "_a_ b _c_";
            var expectedResult = new Stack<Tag>();
            expectedResult.Push(new ItalicsTag(6, 8));
            expectedResult.Push(new ItalicsTag(0, 2));
            TagsConverter.GetAllTags(text).Should()
                .BeEquivalentTo(expectedResult, options => options.WithoutStrictOrdering());
        }

        [Test]
        public void GetAllTags_RecognizesManyTagsOfDifferentTypes()
        {
            var text = "__a__ _c_";
            var expectedResult = new Stack<Tag>();
            expectedResult.Push(new ItalicsTag(6, 8));
            expectedResult.Push(new StrongTextTag(0, 4));
            TagsConverter.GetAllTags(text).Should()
                .BeEquivalentTo(expectedResult, options => options.WithoutStrictOrdering());
        }

        [TestCase("_a__", TestName = "closing and opening tags are not the same")]
        [TestCase("\\_a_", TestName = "tag is escaped")]
        [TestCase("\\\\\\_a_", TestName = "tag is escaped many times")]
        [TestCase("l_ol lo_l", TestName = "tag is in different words")]
        [TestCase("_1_222_3_", TestName = "tag is in text with numbers")]
        [TestCase("_ a_", TestName = "opening tag is before space")]
        [TestCase("_a _", TestName = "closing tag is after space")]
        [TestCase("l__ol lo__l", TestName = "strong text tag is in different words")]
        [TestCase("__1__222__3__", TestName = "strong text tag is in text with numbers")]
        [TestCase("__ a__", TestName = "opening strong text tag is before space")]
        [TestCase("__a __", TestName = "strong text closing tag is after space")]
        public void GetAllTags_RecognizesInvalidTags(string text)
        {
            TagsConverter.GetAllTags(text).Should().BeEmpty();
        }

        [Test]
        public void GetAllTags_RecognizesManyNestedTags()
        {
            var text = "#__x_a_x__";
            var expectedResult = new Stack<Tag>();
            expectedResult.Push(new TitleTag(0, 10));
            expectedResult.Push(new StrongTextTag(1, 9));
            expectedResult.Push(new ItalicsTag(4, 6));
            TagsConverter.GetAllTags(text).Should()
                .BeEquivalentTo(expectedResult, options => options.WithoutStrictOrdering());
        }

        [TestCase(1000, 100, TestName = "2000 tags in 100 milliseconds")]
        [TestCase(10000, 1000, TestName = "20000 tags in 1000 milliseconds")]
        public void GetAllTags_HandleTagsFastly(int iterationsCount, int timeInMilliseconds)
        {
            var text = "__x_a_x__";
            var fullText = new StringBuilder();
            for (var i = 0; i < iterationsCount; i++)
            {
                fullText.Append(" ");
                fullText.Append(text);
            }

            Action act = () => TagsConverter.GetAllTags(fullText.ToString());
            act.ExecutionTime().Should().BeLessThan(timeInMilliseconds.Milliseconds());
        }
    }
}