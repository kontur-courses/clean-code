using Markdown.ParserClasses.Nodes;

namespace Markdown.HTMLGeneratorClasses
{
    public class WordBuilder
    {
        public static string Build(WordNode wordNode)
        {
            return $"{wordNode.Value}";
        }
    }
}