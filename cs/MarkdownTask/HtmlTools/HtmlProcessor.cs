using System.Text;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.HtmlTools
{
    public static class HtmlProcessor
    {
        public static string Process(string text, ICollection<Token> tokens)
        {
            var sb = new StringBuilder(text);
            var shift = 0;

            foreach (var token in TokensProcessing.Process(tokens))
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
                {
                    htmlTag = htmlTag.Insert(1, "/");
                }

                sb.Insert(token.Position + shift, htmlTag);
                shift = sb.Length - text.Length;
            }

            return sb.ToString();
        }
    }
}