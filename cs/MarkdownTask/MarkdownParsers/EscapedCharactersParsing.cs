namespace MarkdownTask.MarkdownParsers
{
    public class EscapedCharactersParsing : IMarkdownParser
    {
        private const char escapeChar = '\\';
        private const int tagLength = 1;
        private readonly HashSet<char> escapedChars = new HashSet<char> { '_', '#', '\\' };

        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();
            tokens.AddRange(FindEscapedTokens(markdown));
            return tokens.OrderBy(x => x.Position).ToList();
        }

        private IEnumerable<Token> FindEscapedTokens(string markdown)
        {
            for (int i = 0; i < markdown.Length - 1; i++)
            {
                var n = markdown[i + 1];

                if (markdown[i] == escapeChar && escapedChars.Contains(n))
                {
                    yield return new Token(TagInfo.TagType.Empty, i, TagInfo.Tag.Open, tagLength);
                    i++;
                }
            }
        }
    }
}
