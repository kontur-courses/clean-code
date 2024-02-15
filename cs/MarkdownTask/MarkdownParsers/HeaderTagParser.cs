namespace MarkdownTask
{
    public class HeaderTagParser : IMarkdownParser
    {
        private const string headerTag = "# ";
        public ICollection<Token> Parse(string markdown)
        {
            var tagStart = 0;
            var tokens = new List<Token>();
            foreach (var paragraph in markdown.Split('\n'))
            {
                var length = paragraph.Length;

                if (paragraph.IndexOf(headerTag) == 0)
                {
                    tokens.Add(new Token(TagInfo.TagType.Header, tagStart, TagInfo.Tag.Open, 2));
                    tokens.Add(new Token(TagInfo.TagType.Header, tagStart + length, TagInfo.Tag.Close, 0));
                }

                tagStart += length;
            }

            return tokens;
        }
    }
}