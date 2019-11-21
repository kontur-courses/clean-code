using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;
using NUnit.Framework;

namespace Markdown.Tests.Parser.TagsParsing
{
    public class TagsReaderTests
    {
        private readonly List<MarkdownTag> tags = new List<MarkdownTag> {new ItalicTag(), new BoldTag()};
        private readonly CharClassifier classifier = new CharClassifier(tags.SelectMany(t => t.String));

        private TagsReader GetReader(string markdown)
        {
            var reader = new TagsReader(markdown, tags, classifier);
            
            return reader;
        }

        [Test]
        public void GetEvents_WithOnlyPlainText_ShouldReturnEmptyList()
        {
            var markdown = "this is plain text";
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> GetEventsTestCasesWithValidTags()
        {
            yield return
                new TestCaseData("this is _plain text",
                        new List<TagEvent> {new TagEvent(8, TagEventType.Start, italic)})
                    .SetName("only start of italic inside markdown");

            yield return
                new TestCaseData("_this is plain text",
                        new List<TagEvent> {new TagEvent(0, TagEventType.Start, italic)})
                    .SetName("only start of italic at markdown start");

            yield return
                new TestCaseData("this is plain_ text",
                        new List<TagEvent> {new TagEvent(13, TagEventType.End, italic)})
                    .SetName("only end of italic inside markdown");

            yield return
                new TestCaseData("this is plain text_",
                        new List<TagEvent> {new TagEvent(18, TagEventType.End, italic)})
                    .SetName("only end of italic at end of markdown");

            yield return
                new TestCaseData("this is __plain text",
                        new List<TagEvent> {new TagEvent(8, TagEventType.Start, bold)})
                    .SetName("start of bold inside markdown");

            yield return
                new TestCaseData("__this is plain text",
                        new List<TagEvent> {new TagEvent(0, TagEventType.Start, bold)})
                    .SetName("start of bold at start of markdown");

            yield return
                new TestCaseData("this is plain__ text",
                        new List<TagEvent> {new TagEvent(13, TagEventType.End, bold)})
                    .SetName("end of bold inside markdown");

            yield return
                new TestCaseData("this is plain text__",
                        new List<TagEvent> {new TagEvent(18, TagEventType.End, bold)})
                    .SetName("end of bold at end of markdown");

            yield return
                new TestCaseData("this __is plain text__",
                        new List<TagEvent>
                        {
                            new TagEvent(5, TagEventType.Start, bold),
                            new TagEvent(20, TagEventType.End, bold)
                        })
                    .SetName("bold start and end");

            yield return
                new TestCaseData("this _is plain text_",
                        new List<TagEvent>
                        {
                            new TagEvent(5, TagEventType.Start, italic),
                            new TagEvent(19, TagEventType.End, italic)
                        })
                    .SetName("italic start and end");

            yield return
                new TestCaseData("this _is plain text__",
                        new List<TagEvent>
                        {
                            new TagEvent(5, TagEventType.Start, italic),
                            new TagEvent(19, TagEventType.End, bold)
                        })
                    .SetName("mixed start and end");

            yield return
                new TestCaseData("___text___",
                        new List<TagEvent>
                        {
                            new TagEvent(0, TagEventType.Start, bold),
                            new TagEvent(2, TagEventType.Start, italic),
                            new TagEvent(7, TagEventType.End, italic),
                            new TagEvent(8, TagEventType.End, bold)
                        })
                    .SetName("italic start and end");

            yield return new TestCaseData(@"_\_ text",
                    new List<TagEvent>
                    {
                        new TagEvent(0, TagEventType.Start, italic)
                    })
                .SetName("escaped underscore valid character to start tag");
        }

        [TestCaseSource(nameof(GetEventsTestCasesWithValidTags))]
        public void GetEvents_WithValidTags_ShouldReturnRightEvents(string markdown, List<TagEvent> expected)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GetEventsTestCasesWithInvalidTags()
        {
            yield return
                new TestCaseData("text __ text")
                    .SetName("invalid token of bold");

            yield return
                new TestCaseData("text _ text")
                    .SetName("invalid token of italic");

            yield return
                new TestCaseData("text_text")
                    .SetName("invalid start token of italic");

            yield return
                new TestCaseData("text__text")
                    .SetName("invalid start token of bold");
        }


        [TestCaseSource(nameof(GetEventsTestCasesWithInvalidTags))]
        public void GetEvents_WithInvalidTags_ShouldReturnEmptyList(string markdown)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEmpty();
        }

        [TestCase("42_1")]
        [TestCase("42__1")]
        public void GetEvents_WithTagInsideDigits_ShouldReturnEmptyList(string markdown)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> GetEventsWithTagAndEscapeSymbol()
        {
            yield return new TestCaseData(@"\_text", new List<TagEvent>())
                .SetName("escape character before start italic should return empty list");

            yield return new TestCaseData(@"text\_", new List<TagEvent>())
                .SetName("escape character before end italic should return empty list");

            yield return new TestCaseData(@"_\_text",
                    new List<TagEvent> {new TagEvent(0, TagEventType.Start, italic)})
                .SetName("escape character inside start bold should return list contains start italic");

            yield return new TestCaseData(@"\__text", new List<TagEvent>())
                .SetName("escape character before bold should return empty list");

            yield return new TestCaseData(@"text\__",
                    new List<TagEvent> {new TagEvent(6, TagEventType.End, italic)})
                .SetName("escape character before end bold should return list contains end italic");

            yield return new TestCaseData(@"text_\_", new List<TagEvent>())
                .SetName("escape character inside end bold should return empty list");
        }

        [TestCaseSource(nameof(GetEventsWithTagAndEscapeSymbol))]
        public void GetEvents_WithTagContainsEscapeSymbol_ShouldReturnRightValue(
            string markdown, List<TagEvent> expected)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEquivalentTo(expected);
        }
    }
}