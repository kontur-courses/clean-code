using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownParsers
{
    public class LinkTagParser : IMarkdownParser
    {
        private const TagType tagType = TagType.Link;

        public ICollection<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            var tagsPair = TryFindLinkTagPosition(text, 0);

            while (tagsPair.Any())
            {
                tokens.AddRange(tagsPair);

                tagsPair = TryFindLinkTagPosition(text, tagsPair.Last().Position);
            }

            return tokens;
        }

        private static Token[] TryFindLinkTagPosition(string text, int startIndex)
        {
            try
            {
                var start = text.IndexOf('[', startIndex);
                var mid = text.IndexOf("](", start);
                var end = text.IndexOf(")", start);

                if (start < end && mid < end)
                {
                    return CreateLinkTokenPair(start, end - start + 1);
                }
            }
            catch
            {
                return Array.Empty<Token>();
            }

            return Array.Empty<Token>();
        }

        private static Token[] CreateLinkTokenPair(int startIndex, int sequenceLength)
        {
            return new Token[] { new Token(tagType, startIndex, Tag.Open, sequenceLength),
                new Token(tagType, startIndex + sequenceLength - 1, Tag.Close, 0)};
        }
    }
}
