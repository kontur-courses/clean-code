using Markdown.Abstractions;
using Markdown.MDElements;
using Markdown.MDNodes;


namespace Markdown.Parsing
{

    public class MarkdownParser
    {
        private readonly PairedDelimiterParser _delimiterParser;

        public MarkdownParser()
        {
            _delimiterParser = new PairedDelimiterParser();
        }

        public List<MarkdownNode> Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new List<MarkdownNode>();

            var result = new List<MarkdownNode>();
            var lines = input.Split(['\n', '\r'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine)) continue;

                if (trimmedLine.StartsWith("# "))
                {
                    var header = new HeaderNode
                    {
                        RawContent = trimmedLine.Substring(2)
                    };

                    ParseInlineContent(header);
                    result.Add(header);
                }
                else
                {
                    var inlineNodes = ParseInlineElements(trimmedLine);
                    result.AddRange(inlineNodes);
                }
            }

            return result;
        }

        private void ParseInlineContent(BlockNode block)
        {
            block.Children = ParseInlineElements(block.RawContent);
        }


        private List<InlineNode> ParseInlineElements(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new List<InlineNode>();

            var escapedText = EscapeProcessor.Escape(text);

            var nodesAfterStrong = _delimiterParser.Parse(
                escapedText,
                "__",
                content => new StrongNode(content),
                isDoubleUnderscore: true,
                recursiveParser: ParseInlineElements); 

            var finalNodes = new List<InlineNode>();
            foreach (var node in nodesAfterStrong)
            {
                if (node is TextNode textNode)
                {
                    var italicNodes = _delimiterParser.Parse(
                        textNode.Content,
                        "_",
                        content => new ItalicNode(content),
                        isDoubleUnderscore: false,
                        recursiveParser: content => new List<InlineNode> { new TextNode(content) }); 
                    finalNodes.AddRange(italicNodes);
                }
                else
                {
                    finalNodes.Add(node);
                }
            }

            return EscapeProcessor.Unescape(finalNodes);
        }
    }
}