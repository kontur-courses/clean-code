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
        private static readonly ItalicTag italic = new ItalicTag();
        private static readonly BoldTag bold = new BoldTag();
        private static readonly List<MarkdownTag> tags = new List<MarkdownTag> {italic, bold};

        private static readonly CharClassifier classifier =
            new CharClassifier(tags.SelectMany(t => t.String));

        private static TagsReader GetReader(string markdown)
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

        private static IEnumerable<TestCaseData> GenerateEventsTestCasesWithValidTagsForEachTag()
        {
            foreach (var tag in tags)
            {
                var tagName = tag.GetType().Name;
                var tagString = tag.String;

                yield return
                    new TestCaseData($"this is {tagString}plain text",
                            new List<TagEvent> {new TagEvent(8, TagEventType.Start, tag)})
                        .SetName($"only start of {tagName} tag inside markdown");

                yield return
                    new TestCaseData($"{tagString}this is plain text",
                            new List<TagEvent> {new TagEvent(0, TagEventType.Start, tag)})
                        .SetName($"only start of  {tagName} tag at markdown start");

                yield return
                    new TestCaseData($"this is plain{tagString} text",
                            new List<TagEvent> {new TagEvent(13, TagEventType.End, tag)})
                        .SetName($"only end of {tagName} tag inside markdown");

                yield return
                    new TestCaseData($"this is plain text{tagString}",
                            new List<TagEvent> {new TagEvent(18, TagEventType.End, tag)})
                        .SetName($"only end of {tagName} tag at end of markdown");


                foreach (var other in tags.Where(other => tag != other))
                {
                    yield return
                        new TestCaseData($"this {tagString}is plain text{other.String}",
                                new List<TagEvent>
                                {
                                    new TagEvent(5, TagEventType.Start, tag),
                                    new TagEvent(5 + tagString.Length + 13, TagEventType.End, other)
                                })
                            .SetName($"{tagName} start and {other.GetType().Name} end");
                }

                yield return
                    new TestCaseData($"this {tagString}is plain text{tagString}",
                            new List<TagEvent>
                            {
                                new TagEvent(5, TagEventType.Start, tag),
                                new TagEvent(5 + tagString.Length + 13, TagEventType.End, tag)
                            })
                        .SetName($"{tagName} start and end");
            }
        }

        private static IEnumerable<TestCaseData> GetEventsTestCasesWithValidTags()
        {
            yield return
                new TestCaseData("___text___",
                        new List<TagEvent>
                        {
                            new TagEvent(0, TagEventType.Start, bold),
                            new TagEvent(2, TagEventType.Start, italic),
                            new TagEvent(7, TagEventType.End, italic),
                            new TagEvent(8, TagEventType.End, bold)
                        })
                    .SetName("italic wrapped in bold");

            yield return new TestCaseData(@"_\_ text",
                    new List<TagEvent>
                    {
                        new TagEvent(0, TagEventType.Start, italic)
                    })
                .SetName("escaped underscore valid character to start tag");
        }

        [TestCaseSource(nameof(GenerateEventsTestCasesWithValidTagsForEachTag))]
        [TestCaseSource(nameof(GetEventsTestCasesWithValidTags))]
        public void GetEvents_WithValidTags_ShouldReturnRightEvents(string markdown, List<TagEvent> expected)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEquivalentTo(expected);
        }

        private static IEnumerable<TestCaseData> GenerateEventsTestCasesWithInvalidTags()
        {
            foreach (var tag in tags)
            {
                yield return
                    new TestCaseData($"text {tag.String} text")
                        .SetName($"invalid token of {tag.GetType().Name}");

                yield return
                    new TestCaseData($"text{tag.String}text")
                        .SetName($"invalid start and end tokens of {tag.GetType().Name}");
            }
        }

        [TestCaseSource(nameof(GenerateEventsTestCasesWithInvalidTags))]
        public void GetEvents_WithInvalidTags_ShouldReturnEmptyList(string markdown)
        {
            var reader = GetReader(markdown);

            var actual = reader.GetEvents();

            actual.Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> GenerateTagsInsideDigits()
        {
            foreach (var tag in tags)
            {
                yield return new TestCaseData($"42{tag.String}42")
                    .SetName($"{tag.GetType().Name} inside digits");
            }
        }

        [TestCaseSource(nameof(GenerateTagsInsideDigits))]
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