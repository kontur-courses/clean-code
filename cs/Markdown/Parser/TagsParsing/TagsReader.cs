using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Parser.Tags;
using Markdown.Tools;

namespace Markdown.Parser.TagsParsing
{
    public class TagsReader
    {
        private static readonly Dictionary<(TagsReaderState state, CharType chr), TagsReaderState> StateConverter =
            new Dictionary<(TagsReaderState state, CharType chr), TagsReaderState>
            {
                [(TagsReaderState.StartReading, CharType.TagSymbol)] = TagsReaderState.StartReading,
                [(TagsReaderState.StartReading, CharType.SpaceSymbol)] = TagsReaderState.Space,
                [(TagsReaderState.StartReading, CharType.Escape)] = TagsReaderState.Other,
                [(TagsReaderState.StartReading, CharType.OtherSymbol)] = TagsReaderState.Other,

                [(TagsReaderState.Space, CharType.TagSymbol)] = TagsReaderState.StartReading,
                [(TagsReaderState.Space, CharType.SpaceSymbol)] = TagsReaderState.Space,
                [(TagsReaderState.Space, CharType.Escape)] = TagsReaderState.Other,
                [(TagsReaderState.Space, CharType.OtherSymbol)] = TagsReaderState.Other,

                [(TagsReaderState.Other, CharType.TagSymbol)] = TagsReaderState.EndReading,
                [(TagsReaderState.Other, CharType.SpaceSymbol)] = TagsReaderState.Space,
                [(TagsReaderState.Other, CharType.Escape)] = TagsReaderState.Other,
                [(TagsReaderState.Other, CharType.OtherSymbol)] = TagsReaderState.Other,

                [(TagsReaderState.EndReading, CharType.TagSymbol)] = TagsReaderState.EndReading,
                [(TagsReaderState.EndReading, CharType.SpaceSymbol)] = TagsReaderState.Space,
                [(TagsReaderState.EndReading, CharType.Escape)] = TagsReaderState.Other,
                [(TagsReaderState.EndReading, CharType.OtherSymbol)] = TagsReaderState.Other,
            };

        private readonly CharClassifier classifier;
        private readonly string text;
        private readonly List<MarkdownTag> tags;
        private TagsReaderState state;
        private List<TagEvent> events;
        private int textOffset;

        public TagsReader(string text, IEnumerable<MarkdownTag> tags, CharClassifier classifier)
        {
            this.text = text;
            this.tags = tags.OrderByDescending(r => r.String.Length).ToList();
            state = TagsReaderState.StartReading;
            this.classifier = classifier;
        }

        public List<TagEvent> GetEvents()
        {
            if (events != null)
                return events;

            events = new List<TagEvent>();
            var currentTags = new StringBuilder();

            for (textOffset = 0; textOffset < text.Length; textOffset++)
            {
                var chr = text[textOffset];

                var nextState = StateConverter[(state, classifier.GetType(chr))];

                if (IsReadingState(nextState))
                    currentTags.Append(chr);

                if (IsFinishReading(nextState) && textOffset != 0)
                {
                    if (IsCorrectEnding(state, nextState))
                        FinishReading(currentTags);

                    currentTags.Clear();
                }

                if (classifier.GetType(chr) == CharType.Escape)
                    textOffset++;

                state = nextState;
            }

            if (currentTags.Length != 0)
            {
                FinishReading(currentTags);
            }

            return events;
        }

        private static bool IsCorrectEnding(TagsReaderState currentState, TagsReaderState nextState)
        {
            return (currentState == TagsReaderState.EndReading && nextState == TagsReaderState.Space) ||
                   (currentState == TagsReaderState.StartReading && nextState == TagsReaderState.Other);
        }

        private bool IsFinishReading(TagsReaderState nextState)
        {
            return nextState != state && IsReadingState(state);
        }

        private void FinishReading(StringBuilder currentTags)
        {
            var isEnd = state == TagsReaderState.EndReading;
            var tagsString = currentTags.ToString();

            if (isEnd)
                tagsString = tagsString.GetReversed();

            for (var i = 0; i < tagsString.Length; i++)
            {
                i = ReadTagsGreedy(tagsString, i, isEnd);
            }
        }

        private int ReadTagsGreedy(string tagsString, int start, bool reversed = false)
        {
            var offsetBeforeTags = textOffset - tagsString.Length;

            foreach (var tag in tags)
            {
                if (start >= tagsString.Length)
                    break;

                if (tagsString.ContainsAtIndex(start, tag.String))
                {
                    var offset = reversed
                        ? offsetBeforeTags + tagsString.Length - start - tag.String.Length
                        : offsetBeforeTags + start;
                    var @event = new TagEvent(offset, GetReaderEventType(state), tag);

                    events.Add(@event);

                    start += tag.String.Length;
                }
            }

            return start;
        }

        private static TagEventType GetReaderEventType(TagsReaderState state)
        {
            return state == TagsReaderState.StartReading ? TagEventType.Start : TagEventType.End;
        }

        private static bool IsReadingState(TagsReaderState state)
        {
            return state == TagsReaderState.StartReading || state == TagsReaderState.EndReading;
        }
    }
}