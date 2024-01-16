using static MarkdownTask.TagInfo;

namespace MarkdownTask.MarkdownParsers
{
    public class LinkTagParser : ITagParser
    {
        public ICollection<Token> Parse(string markdown)
        {
            var tokens = new List<Token>();
            var startIndex = 0;

            while ((startIndex = markdown.IndexOf("[", startIndex)) != -1)
            {
                var closeIndex = markdown.IndexOf("]", startIndex);
                var openParenthesisIndex = markdown.IndexOf("(", closeIndex);
                var closeParenthesisIndex = markdown.IndexOf(")", openParenthesisIndex);

                if (closeIndex == -1 || openParenthesisIndex == -1 || closeParenthesisIndex == -1)
                {
                    continue;
                }
                if (closeIndex + 1 == openParenthesisIndex)
                {
                    tokens.Add(new Token(TagType.Link, startIndex, Tag.Open, closeParenthesisIndex - startIndex + 1));
                }

                startIndex = Math.Max(closeIndex, Math.Max(openParenthesisIndex, closeParenthesisIndex)) + 1;
            }

            return tokens;
        }
    }
}
