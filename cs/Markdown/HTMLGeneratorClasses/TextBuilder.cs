using System;
using System.ComponentModel;
using System.Text;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public class TextBuilder
    {
        public static string Build(TextNode textNode)
        {
            var text = GetNodeValue(textNode);
            switch (textNode.Type)
            {
                case TextType.Text:
                    return $"{text}";
                case TextType.Emphasis:
                    return $"<em>{text}</em>";
                case TextType.Bold:
                    return $"<strong>{text}</strong>";
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        private static string GetNodeValue(TextNode textNode)
        {
            var stringBuilder = new StringBuilder();
            foreach (var word in textNode.Words)
                stringBuilder.Append(WordBuilder.Build(word));

            return stringBuilder.ToString();
        }
    }
}