using Markdown.Abstractions;
using Markdown.MDNodes;

namespace Markdown.Parsing
{

    public class PairedDelimiterParser
    {
        
        public List<InlineNode> Parse(
            string text,
            string delimiter,
            Func<string, InlineNode> nodeFactory,
            bool isDoubleUnderscore,
            Func<string, List<InlineNode>> recursiveParser)
        {
            var result = new List<InlineNode>();
            var currentIndex = 0;

            while (currentIndex < text.Length)
            {
                var openIndex = text.IndexOf(delimiter, currentIndex, StringComparison.Ordinal);

                if (openIndex == -1)
                {
                    result.Add(new TextNode(text.Substring(currentIndex)));
                    break;
                }

                if (openIndex > currentIndex)
                {
                    result.Add(new TextNode(text.Substring(currentIndex, openIndex - currentIndex)));
                }

                var closeIndex = text.IndexOf(delimiter, openIndex + delimiter.Length, StringComparison.Ordinal);

                if (closeIndex == -1)
                {
                    result.Add(new TextNode(delimiter));
                    currentIndex = openIndex + delimiter.Length;
                    continue;
                }

                var innerContent = text.Substring(
                    openIndex + delimiter.Length,
                    closeIndex - openIndex - delimiter.Length);

                if (DelimiterValidator.IsValid(text, openIndex, closeIndex, delimiter, innerContent, isDoubleUnderscore))
                {
                    var node = nodeFactory(innerContent);

                    if (node is PairedInlineNode pairedNode)
                    {
                        if (isDoubleUnderscore)
                        {
                            var innerNodes = recursiveParser(innerContent);
                            pairedNode.Children = innerNodes;
                        }
                        else
                        {
                            pairedNode.Children = new List<InlineNode> { new TextNode(innerContent) };
                        }
                    }

                    result.Add(node);
                    currentIndex = closeIndex + delimiter.Length;
                }
                else
                {
                    result.Add(new TextNode(delimiter));
                    currentIndex = openIndex + delimiter.Length;
                }
            }

            return result;
        }
    }
}