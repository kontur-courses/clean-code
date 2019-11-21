using System.Collections.Generic;
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
        private static IEnumerable<TestCaseData> GetNoPairEvents()
        {
            yield return new TestCaseData(new List<TagEvent> {new TagEvent(0, TagEventType.Start, new BoldTag())})
                .SetName("only start tag");

            yield return new TestCaseData(new List<TagEvent> {new TagEvent(7, TagEventType.End, new BoldTag())})
                .SetName("only end tag");

            yield return new TestCaseData(new List<TagEvent>())
                .SetName("empty tagEvents");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(8, TagEventType.End, new ItalicTag())
                    })
                .SetName("bold start and italic end");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new ItalicTag()),
                        new TagEvent(8, TagEventType.End, new BoldTag())
                    })
                .SetName("italic start and bold end");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(7, TagEventType.Start, new BoldTag()),
                        new TagEvent(8, TagEventType.Start, new BoldTag())
                    })
                .SetName("tags are two starts");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.End, new BoldTag()),
                        new TagEvent(8, TagEventType.End, new BoldTag())
                    })
                .SetName("tags are two ends");
        }

        [TestCaseSource(nameof(GetNoPairEvents))]
        public void GetPairEvents_NoPairEvents_ShouldReturnEmptyList(List<TagEvent> events)
        {
            var pair = Filter.PairEvents(events);

            pair.Should().BeEmpty();
        }

        private static IEnumerable<TestCaseData> GetPairEvents()
        {
            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(8, TagEventType.End, new BoldTag())
                    },
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(8, TagEventType.End, new BoldTag())
                    }
                )
                .SetName("bold start and bold end");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(8, TagEventType.End, new ItalicTag()),
                        new TagEvent(9, TagEventType.End, new BoldTag())
                    },
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(9, TagEventType.End, new BoldTag())
                    }
                )
                .SetName("non paired italic between paired bolds should not be as a result");

            yield return new TestCaseData(
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(7, TagEventType.Start, new ItalicTag()),
                        new TagEvent(8, TagEventType.End, new ItalicTag()),
                        new TagEvent(9, TagEventType.End, new BoldTag())
                    },
                    new List<TagEvent>
                    {
                        new TagEvent(5, TagEventType.Start, new BoldTag()),
                        new TagEvent(7, TagEventType.Start, new ItalicTag()),
                        new TagEvent(8, TagEventType.End, new ItalicTag()),
                        new TagEvent(9, TagEventType.End, new BoldTag())
                    }
                )
                .SetName("correct sequence of nested tags  bold and italic");
        }

        [TestCaseSource(nameof(GetPairEvents))]
        public void GetPairEvents_FromValidPairEvents_ShouldReturnRightValue(
            List<TagEvent> events, List<TagEvent> expected)
        {
            var actual = Filter.PairEvents(events);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}