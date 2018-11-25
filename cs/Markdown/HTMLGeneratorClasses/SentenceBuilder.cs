using System.Text;
using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public class SentenceBuilder
    {
        public static string Build(SentenceNode sentenceNode)
        {
            return $"{GetNodeValue(sentenceNode)}";
        }

        private static string GetNodeValue(SentenceNode sentenceNode)
        {
            var stringBuilder = new StringBuilder();
            foreach (var text in sentenceNode.Texts)
                stringBuilder.Append(TextBuilder.Build(text));

            return stringBuilder.ToString();
        }
    }
}