using System.Text;
using static MarkdownTask.TagInfo;

namespace MarkdownTask.HtmlTools
{
    public static class HtmlProcessor
    {
        public static string Process(string inputText, ICollection<Token> tokens)
        {
            var processedText = new StringBuilder(inputText);
            var shiftOffset = 0;

            foreach (var token in TokensProcessing.Process(tokens))
            {
                processedText.Remove(token.Position + shiftOffset, token.TagLength);
                processedText.Insert(token.Position + shiftOffset, GetHtlmTag(inputText, token));

                shiftOffset = processedText.Length - inputText.Length;
            }

            return processedText.ToString();
        }

        private static string GetHtlmTag(string inputText, Token token)
        {
            if (token.TagType == TagType.Link)
            {
                return BuildLinkTag(inputText, token);
            }
            else
            {
                return BuildHtmlTag(token);
            }
        }

        private static string BuildHtmlTag(Token token)
        {
            var htmlTag = token.TagType.GetHtmlString();

            if (token.Tag == Tag.Close)
            {
                htmlTag = htmlTag.Insert(1, "/");
            }

            return htmlTag;
        }

        private static string BuildLinkTag(string text, Token token)
        {
            return LinkTagBuilder.Build(text.Substring(token.Position, token.TagLength));
        }
    }
}