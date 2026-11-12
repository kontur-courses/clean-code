using Markdown.Nodes;
using Markdown.Parsing;
using Markdown.Parsing.Parsers;
using System.Text;
using Markdown.Parsing.Parsers.InlineParser;

namespace Markdown;

public class Md
{
    private readonly List<IParser> parsers = new()
    {
        new ListParser(),
        new BlockParser(),
        new InlineParser()
    };

    public string Render(string text)
    {
        if (string.IsNullOrEmpty(text)) return string.Empty;

        var nodes = new List<INode> { new TextNode(text) };

        foreach (var parser in parsers)
        {
            nodes = parser.Parse(nodes);
        }

        var sb = new StringBuilder();
        foreach (var node in nodes)
        {
            node.Render(sb);
        }

        return sb.ToString();
    }
}