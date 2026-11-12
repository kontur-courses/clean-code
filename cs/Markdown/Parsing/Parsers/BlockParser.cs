using Markdown.Nodes;
using Markdown.Nodes.HtmlTags;

namespace Markdown.Parsing.Parsers;

public class BlockParser : ParserBase
{
    protected override List<INode> ParseText(string text)
    {
        var result = new List<INode>();
        var lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var paragraph = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.Length == 0)
            {
                FlushParagraph(result, paragraph);
            }
            else
            {
                paragraph.Add(line);
            }
        }

        FlushParagraph(result, paragraph);
        return result;
    }

    private static void FlushParagraph(List<INode> result, List<string> paragraph)
    {
        if (paragraph.Count == 0) return;

        var value = string.Join(" ", paragraph).Trim();
        paragraph.Clear();

        if (value.Length == 0) return;

        if (value.StartsWith("# ", StringComparison.Ordinal))
        {
            result.Add(new ContainerNode(HtmlTag.H1, new List<INode> { new TextNode(value[2..]) }));
        }
        else
        {
            result.Add(new TextNode(value));
        }
    }
}