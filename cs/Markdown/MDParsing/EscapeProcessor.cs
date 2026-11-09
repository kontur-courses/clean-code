using System.Collections.Generic;
using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.Parsing
{

    public static class EscapeProcessor
    {
        // Заменяем экранированные символы на уникальные строки, чтобы избежать их обработки как обычных символов

        private const string ESCAPED_UNDERSCORE = "\x00ESCAPED_UNDERSCORE\x00";
        private const string ESCAPED_BACKSLASH = "\x00ESCAPED_BACKSLASH\x00";

        public static string Escape(string text)
        {
            text = text.Replace("\\_", ESCAPED_UNDERSCORE);
            text = text.Replace("\\\\", ESCAPED_BACKSLASH);
            return text;
        }


        public static List<InlineNode> Unescape(List<InlineNode> nodes)
        {
            var result = new List<InlineNode>();

            foreach (var node in nodes)
            {
                if (node is TextNode textNode)
                {
                    var content = textNode.Content;
                    content = content.Replace(ESCAPED_UNDERSCORE, "_");
                    content = content.Replace(ESCAPED_BACKSLASH, "\\");
                    result.Add(new TextNode(content));
                }
                else
                {
                    result.Add(node);
                }
            }

            return result;
        }
    }
}