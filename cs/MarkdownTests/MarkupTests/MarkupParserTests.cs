using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Markup;
using Markdown.Tags;
using NUnit.Framework;

namespace MarkdownTests.MarkupTests
{
    public class MarkupParserTests
    {
        [SetUp]
        public void SetUp()
        {
            var supportedTags = new Dictionary<string, Tag>
            {
                {"_", new Tag("_", "em", "_")},
                {"__", new Tag("__", "strong", "__")},
                {"#", new Tag("#", "h1", "\n")}
            };
            
            sut = new MarkupParser(supportedTags);
        }
        
        private MarkupParser sut;
        
        [TestCase("_test_", 1)]
        [TestCase("", 0)]
        [TestCase("_test__text_", 2)]
        [TestCase("_test_ _text_", 3)]
        [TestCase("_test_ __text__", 3)]
        [TestCase("_test_ #text\n", 3)]
        [TestCase("_test___text__#text\n", 3)]
        [TestCase("_test_abc__text__abc #text\n", 5)]
        public void ParseMarkup_ReturnCorrectPartsCount(string testStr, int expectedCount)
        {
            var markupParts = sut.ParseMarkup(testStr);

            markupParts.Count().Should().Be(expectedCount);
        }
    }
}