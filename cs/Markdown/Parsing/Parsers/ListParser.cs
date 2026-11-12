using Markdown.Nodes;
using Markdown.Nodes.HtmlTags;

namespace Markdown.Parsing.Parsers;

public class ListParser : ParserBase
{
    protected override List<INode> ParseText(string text)
    {
        var result = new List<INode>();
        var lines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var listItems = new List<string>();
        var currentText = new List<string>();

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd();

            if (line.StartsWith("* ", StringComparison.Ordinal))
            {
                FlushText(result, currentText);
                listItems.Add(line[2..]);
            }
            else
            {
                FlushList(result, listItems);
                currentText.Add(line);
            }
        }

        FlushText(result, currentText);
        FlushList(result, listItems);

        return result;
    }

    private static void FlushList(List<INode> result, List<string> listItems)
    {
        if (listItems.Count == 0) return;

        var children = listItems.Select(item =>
            (INode)new ContainerNode(HtmlTag.Li, new List<INode> { new TextNode(item) })).ToList();

        result.Add(new ContainerNode(HtmlTag.Ul, children));
        listItems.Clear();
    }

    private static void FlushText(List<INode> result, List<string> currentText)
    {
        if (currentText.Count == 0) return;

        var textValue = string.Join("\n", currentText);
        result.Add(new TextNode(textValue));
        currentText.Clear();
    }
}