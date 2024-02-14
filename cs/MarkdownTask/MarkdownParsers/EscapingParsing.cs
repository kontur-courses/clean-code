namespace MarkdownTask.MarkdownParsers
{
    public class EscapingParsing : ITagParser
    {
        private const char escapeChar = '\\';
        private const string escaped = @"\_#";

        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();

            for (int i = 0; i < markdown.Length - 1; i++)
            {
                var n = markdown[i + 1];

                if (markdown[i] == escapeChar && escaped.Contains(markdown[i + 1]))
                {
                    tokens.Add(new Token(TagInfo.TagType.Empty, i, TagInfo.Tag.Open, 1));
                    i++;
                }
            }

            return tokens.OrderBy(x => x.Position).ToList();
        }
    }
}
