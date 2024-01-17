using System.Text;
using static MarkdownTask.TagInfo;

namespace MarkdownTask
{
    public static class HtmlProcessor
    {
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

                if (IsNotStrongOrItalicTag(token))
                {
                    selected.Add(token);
                    if (token.TagType == TagType.Link)
                        filterEdge = token.Position + token.TagLength;
                    continue;
                }

                if (token.Tag == Tag.Open)
                {
                    stack.Push(token);
                    continue;
                }

                if (!stack.Any())
                {
                    continue;
                }

                var stackTop = stack.Pop();

                if (IsAllowedNestedTag(stackTop, token, stack))
                {
                    selected.Add(stackTop);
                    selected.Add(token);
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

                if (token.TagType == TagType.Link)
                {
                    sb.Insert(token.Position + shift, LinkTagBuilder.Build(text.Substring(token.Position, token.TagLength)));
                    shift = sb.Length - text.Length;
                    continue;
                }

                var htmlTag = token.TagType.GetHtmlString();

                if (token.Tag == Tag.Close)
                    htmlTag = htmlTag.Insert(1, "/");

                sb.Insert(token.Position + shift, htmlTag);
                shift = sb.Length - text.Length;
            }

            return sb.ToString();
        }

        private static bool IsNotStrongOrItalicTag(Token token)
        {
            return token.TagType != TagType.Italic && token.TagType != TagType.Strong;
        }

        private static bool IsAllowedNestedTag(Token stackTop, Token token, Stack<Token> stack)
        {
            return stackTop.TagType == token.TagType
                    && !(token.TagType == TagType.Strong
                    && stack.Any()
                    && stack.Peek().TagType == TagType.Italic);
        }
    }
}