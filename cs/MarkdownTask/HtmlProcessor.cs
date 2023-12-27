using System.Text;

namespace MarkdownTask
{
    public static class HtmlProcessor
    {
        private static readonly Dictionary<TagInfo.TagType, string> htmlTags = new Dictionary<TagInfo.TagType, string>
        {
            {TagInfo.TagType.Header, "<h1>" },
            {TagInfo.TagType.Italic, "<em>" },
            {TagInfo.TagType.Strong, "<strong>" },
            {TagInfo.TagType.Empty, "" }
        };

        private static ICollection<Token> FilterTokens(ICollection<Token> tokens)
        {
            var selected = new List<Token>();
            var stack = new Stack<Token>();
            var filterEdge = -1;
            foreach (var token in tokens.OrderBy(x => x.Position))
            {
                if (token.Position < filterEdge)
                    continue;
                else
                    filterEdge = -1;

                if (token.TagType != TagInfo.TagType.Italic && token.TagType != TagInfo.TagType.Strong)
                {
                    selected.Add(token);
                    if (token.TagType == TagInfo.TagType.Link)
                        filterEdge = token.Position + token.TagLength;
                    continue;
                }

                if (token.Tag == TagInfo.Tag.Open)
                {
                    stack.Push(token);
                }
                else
                {
                    if (!stack.Any())
                        continue;
                    var stackTop = stack.Pop();
                    if (stackTop.TagType == token.TagType)
                    {
                        if (token.TagType == TagInfo.TagType.Strong)
                        {
                            if (!stack.Any() || stack.Peek().TagType != TagInfo.TagType.Italic)
                            {
                                selected.Add(stackTop);
                                selected.Add(token);
                            }
                        }
                        else
                        {
                            selected.Add(stackTop);
                            selected.Add(token);
                        }
                    }
                }
            }

            return selected.OrderBy(x => x.Position).ToList();
        }

        public static string Process(string text, ICollection<Token> tokens)
        {

            var sb = new StringBuilder(text);
            var shift = 0;

            foreach (var token in FilterTokens(tokens))
            {
                sb.Remove(token.Position + shift, token.TagLength);

                if (token.TagType == TagInfo.TagType.Link)
                {
                    sb.Insert(token.Position + shift, LinkTagBuilder.Build(text.Substring(token.Position, token.TagLength)));
                    shift = sb.Length - text.Length;
                    continue;
                }

                var htmlTag = htmlTags[token.TagType];
                if (token.Tag == TagInfo.Tag.Close)
                    htmlTag = htmlTag.Insert(1, "/");

                sb.Insert(token.Position + shift, htmlTag);
                shift = sb.Length - text.Length;
            }

            return sb.ToString();
        }
    }
}