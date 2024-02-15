using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public class PairedTagsParser : IMarkdownParser
    {
        private readonly string tag;
        private readonly int tagLength;
        private readonly TagType tagType;

        public PairedTagsParser(string tag, TagType type)
        {
            this.tag = tag;
            tagLength = tag.Length;
            tagType = type;
        }

        public ICollection<Token> Parse(string text)
        {
            if (text.Length <= tag.Length * 2)
                return new List<Token>();

            return FindAndProcessAllTagsInText(text);
        }

        private List<Token> FindAndProcessAllTagsInText(string text)
        {
            var tokens = new List<Token>();
            var openedTags = new Stack<Candidate>();
            var startIndex = 0;

            do
            {
                var foundTagIndex = IndexOfFirstUnescapedTag(text, startIndex);

                if (foundTagIndex == -1)
                {
                    break;
                }

                startIndex = foundTagIndex + tag.Length;
                HandleFoundTagAtIndex(foundTagIndex, text, openedTags, tokens);
            }
            while (startIndex < text.Length);

            return tokens;
        }

        private int IndexOfFirstUnescapedTag(string text, int startIndex)
        {
            var firstTagStartIndex = text.IndexOf(tag, startIndex, StringComparison.Ordinal);

            if (firstTagStartIndex == -1)
            {
                return -1;
            }

            var next = text.IndexOf(tag, firstTagStartIndex + tagLength, StringComparison.Ordinal);

            if (Utils.IsEscaped(text, firstTagStartIndex) || (next != -1 && next - firstTagStartIndex == tagLength))
            {
                return IndexOfFirstUnescapedTag(text, firstTagStartIndex + tagLength + 1);
            }

            return firstTagStartIndex;
        }

        private void HandleFoundTagAtIndex(int position, string text, Stack<Candidate> openedTags, List<Token> tokens)
        {
            var candidate = new Candidate();
            candidate.position = position;

            if (openedTags.Count > 0)
            {
                candidate.edgeType = GetClosingTagState(text, position);
            }
            else
            {
                candidate.edgeType = GetOpeningTagState(text, position);
            }

            if (candidate.edgeType == EdgeType.BAD)
            {
                return;
            }

            if (openedTags.Count > 0)
            {
                var opened = openedTags.Pop();

                if (candidate.position - opened.position - tagLength > 0)
                {
                    tokens.AddRange(TryCreateTagsPair(text, opened, candidate));
                }

            }
            else
            {
                openedTags.Push(candidate);
            }

        }

        private List<Token> TryCreateTagsPair(string text, Candidate open, Candidate close)
        {
            if (Utils.CanSelect(text, open, close))
            {
                return new List<Token>()
                {
                    new Token(tagType,open.position,Tag.Open, tagLength),
                    new Token(tagType,close.position,Tag.Close, tagLength),
                };
            }

            return new List<Token>();
        }

        private EdgeType GetOpeningTagState(string text, int position)
        {
            if (Utils.IsBeforeSpace(text, position))
            {
                return EdgeType.BAD;
            }

            if (Utils.IsAfterSpace(text, position))
            {
                return EdgeType.EDGE;
            }

            return EdgeType.MIDDLE;
        }

        private EdgeType GetClosingTagState(string text, int position)
        {
            if (Utils.IsAfterSpace(text, position))
            {
                return EdgeType.BAD;
            }

            if (Utils.IsBeforeSpace(text, position))
            {
                return EdgeType.EDGE;
            }

            return EdgeType.MIDDLE;
        }

        public struct Candidate
        {
            public int position;
            public EdgeType edgeType;
        }

        public enum EdgeType
        {
            BAD,
            EDGE,
            MIDDLE
        }
    }
}