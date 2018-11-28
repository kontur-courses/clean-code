using System.ComponentModel;
using System.Text;
using Markdown.ParserClasses;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public static class TextBuilder
    {
        public static string Build(TextNode textNode)
        {
            var text = GetNodeValue(textNode);
            if (Parser.TextTypeProcessors.TryGetValue(textNode.Type, out var textProcessor))
                return textProcessor(text);

            throw new InvalidEnumArgumentException();
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