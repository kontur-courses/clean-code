using Markdown.Abstractions;
using Markdown.MDParsing;
using System.Text;

namespace Markdown
{
    public class Md
    {
        private readonly MarkdownParser _parser;

        public Md()
        {
            _parser = new MarkdownParser();
        }

        public string Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            List<MarkdownNode> nodes = _parser.Parse(input);

            var result = new StringBuilder();
            var currentParagraph = new List<InlineNode>();

            foreach (var node in nodes)
            {
                if (node is BlockNode blockNode)
                {
                    FlushParagraph(currentParagraph, result);
                    currentParagraph.Clear();

                    result.Append(blockNode.Render());
                    result.Append('\n');
                }
                else if (node is InlineNode inlineNode)
                {
                    currentParagraph.Add(inlineNode);
                }
            }

            FlushParagraph(currentParagraph, result);

            return result.ToString().TrimEnd('\n');
        }

        private static void FlushParagraph(List<InlineNode> paragraph, StringBuilder result)
        {
            if (paragraph.Count == 0) return;

            if (result.Length > 0 && result[result.Length - 1] != '\n')
            {
                result.Append('\n');
            }

            foreach (var inline in paragraph)
            {
                result.Append(inline.Render());
            }
            result.Append('\n');
        }
    }
}