namespace MarkdownTask
{
    public class HeaderTagParser : IMarkdownParser
    {
        private const string headerTag = "# ";
        private const int openTagLength = 2;
        private const int closeTagLength = 0;

        public ICollection<Token> Parse(string markdown)
        {
            var tagStart = 0;
            var tokens = new List<Token>();

            foreach (var paragraph in markdown.Split('\n'))
            {
                var length = paragraph.Length;

                if (IsHeaderTag(paragraph))
                {
                    AddHeaderTag(tokens, tagStart, length);
                }

                tagStart += length;
            }

            return tokens;
        }

        private static void AddHeaderTag(List<Token> tokens, int position, int length)
        {
            tokens.Add(CreateHeaderTagToken(position, true));
            tokens.Add(CreateHeaderTagToken(position + length, false));
        }

        private static bool IsHeaderTag(string paragraph)
        {
            return paragraph.StartsWith(headerTag);
        }

        private static Token CreateHeaderTagToken(int position, bool isOpeningTag)
        {
            return new Token(
                TagInfo.TagType.Header,
                position,
                isOpeningTag ? TagInfo.Tag.Open : TagInfo.Tag.Close,
                isOpeningTag ? openTagLength : closeTagLength);
        }
    }
}