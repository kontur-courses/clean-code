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

                if (closeIndex == -1)
                {
                    break;
                }

                var openParenthesisIndex = markdown.IndexOf("(", closeIndex);

                if (openParenthesisIndex == -1)
                {
                    break;
                }

                if (openParenthesisIndex - closeIndex != 1)
                {
                    startIndex = openParenthesisIndex;
                    continue;
                }

                var closeParenthesisIndex = markdown.IndexOf(")", openParenthesisIndex);

                if (closeIndex == -1)
                {
                    continue;
                }

                if (closeIndex + 1 == openParenthesisIndex)
                {
                    tokens.Add(new Token(TagType.Link, startIndex, Tag.Open, closeParenthesisIndex - startIndex + 1));
                    tokens.Add(new Token(TagType.Link, closeParenthesisIndex, Tag.Close, 0));
                }

                startIndex = Math.Max(closeIndex, Math.Max(openParenthesisIndex, closeParenthesisIndex)) + 1;
            }

            return tokens;
        }
    }
}
