namespace MarkdownTask.MarkdownParsers
{
    public class EscapedCharactersParsing : IMarkdownParser
    {
        private const int tagLength = 1;
        private const TagInfo.TagType tagType = TagInfo.TagType.Empty;
        private readonly char[] escapedChars = new char[] { '_', '#', '\\' };

        public ICollection<Token> Parse(string text)
        {
            var tokens = new List<Token>();
            tokens.AddRange(FindEscapedTokens(text));

            return tokens.OrderBy(x => x.Position).ToList();
        }

        private IEnumerable<Token> FindEscapedTokens(string text)
        {
            var ecsapedCharPosition = text.IndexOfAny(escapedChars);

            while (ecsapedCharPosition >= 0)
            {
                if (Utils.IsEscaped(text, ecsapedCharPosition))
                {
                    yield return BuildEscapeTag(ecsapedCharPosition);
                }

                ecsapedCharPosition = text.IndexOfAny(escapedChars, ecsapedCharPosition + 1);
            }
        }

        private static Token BuildEscapeTag(int i)
        {
            return new Token(tagType, i - 1, TagInfo.Tag.Open, tagLength);
        }
    }
}
