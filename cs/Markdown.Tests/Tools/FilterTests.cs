using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Markdown.Parser.Tags;
using Markdown.Parser.TagsParsing;
using Markdown.Tools;
using NUnit.Framework;

namespace Markdown.Tests.Tools
{
    [TestFixture]
    public class FilterTests
    {
        private static readonly List<MarkdownTag> Tags =
            new List<MarkdownTag> {new ItalicTag(), new BoldTag()};

        private static IEnumerable<TestCaseData> GetNoPairEvents()
        {
            foreach (var tag in Tags)
            {
                yield return new TestCaseData(new List<TagEvent> {new TagEvent(0, TagEventType.Start, tag)})
                    .SetName($"only start {tag.Name} tag");

                yield return new TestCaseData(new List<TagEvent> {new TagEvent(7, TagEventType.End, tag)})
                    .SetName($"only end {tag.Name} tag");

                foreach (var other in Tags.Where(other => tag != other))
                {
                    yield return new TestCaseData(
                            new List<TagEvent>
                            {
                                new TagEvent(5, TagEventType.Start, tag),
                                new TagEvent(8, TagEventType.End, other)
                            })
                        .SetName($"{tag.Name} start and {other.Name} end");
                }

                yield return new TestCaseData(
                        new List<TagEvent>
                        {
                            new TagEvent(7, TagEventType.Start, tag),
                            new TagEvent(8, TagEventType.Start, tag)
                        })
                    .SetName($"two starts of {tag.Name}");

                yield return new TestCaseData(
                        new List<TagEvent>
                        {
                            new TagEvent(5, TagEventType.End, tag),
                            new TagEvent(8, TagEventType.End, tag)
                        })
                    .SetName($"two ends of {tag.Name}");
            }

            yield return new TestCaseData(new List<TagEvent>())
                .SetName("empty tagEvents");
        }

        [TestCaseSource(nameof(GetNoPairEvents))]
        public void GetPairEvents_NoPairEvents_ShouldReturnEmptyList(List<TagEvent> events)
        {
            var pair = Filter.PairEvents(events);

            pair.Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> GetPairEvents()
        {
            foreach (var tag in Tags)
            {
                var tagName = tag.Name;

                yield return new TestCaseData(
                        new List<TagEvent>
                        {
                            new TagEvent(5, TagEventType.Start, tag),
                            new TagEvent(8, TagEventType.End, tag)
                        }
                    )
                    .SetName($"{tagName} start and {tagName} end");

                foreach (var other in Tags.Where(other => tag != other))
                {
                    yield return new TestCaseData(
                            new List<TagEvent>
                            {
                                new TagEvent(5, TagEventType.Start, tag),
                                new TagEvent(7, TagEventType.Start, other),
                                new TagEvent(8, TagEventType.End, other),
                                new TagEvent(9, TagEventType.End, tag)
                            }
                        )
                        .SetName($"correct sequence of nested tags {tag.Name} and {other.Name}");
                }
            }
        }

        [TestCaseSource(nameof(GetPairEvents))]
        public void GetPairEvents_FromValidPairEvents_ShouldReturnThisEvents(
            List<TagEvent> events)
        {
            var actual = Filter.PairEvents(events);

            actual.Should().BeEquivalentTo(events);
        }

        private static IEnumerable<TestCaseData> GenerateNonPairedTagBetweenPairedOtherTags()
        {
            foreach (var tag in Tags)
            {
                foreach (var other in Tags.Where(other => other != tag))
                {

                    yield return new TestCaseData(
                            new List<TagEvent>
                            {
                                new TagEvent(5, TagEventType.Start, tag),
                                new TagEvent(8, TagEventType.End, other),
                                new TagEvent(9, TagEventType.End, tag)
                            },
                            new List<TagEvent>
                            {
                                new TagEvent(5, TagEventType.Start, tag),
                                new TagEvent(9, TagEventType.End, tag)
                            }
                        )
                        .SetName($"non paired {other.Name} between paired {tag.Name}s should not be as a result");
                }
            }
        }

        [TestCaseSource(nameof(GenerateNonPairedTagBetweenPairedOtherTags))]
        public void GetPairEvents_FromValidPairEvents_ShouldReturnRightValue(
            List<TagEvent> events, List<TagEvent> expected)
        {
            var actual = Filter.PairEvents(events);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}