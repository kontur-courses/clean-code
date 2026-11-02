using Markdown.Nodes;

namespace Markdown.Parsing;

public class BlockParser
{
    private readonly InlineParser inlineParser;

    public BlockParser(InlineParser inlineParser)
    {
        inlineParser = inlineParser;
    }

    public List<INode> Parse(string text)
    {
        var blocks = new List<INode>();

        if (string.IsNullOrEmpty(text))
            return blocks;

        if (text.StartsWith("# "))
        {
            var content = text.Substring(2);
            var children = inlineParser.Parse(content);
            blocks.Add(new ContainerNode(HtmlTag.H1, children));
        }
        else
        {
            var children = inlineParser.Parse(text);
            blocks.AddRange(children);
        }

        return blocks;
    }
}