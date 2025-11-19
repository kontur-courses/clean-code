namespace Markdown;

using Markdown.Entities;
using System.Text;

public class HtmlRenderer
{
    private const string EmOpen = "<em>";
    private const string EmClose = "</em>";
    private const string StrongOpen = "<strong>";
    private const string StrongClose = "</strong>";
    private const string HeadingOpen = "<h1>";
    private const string HeadingClose = "</h1>";
    private const string ParagraphOpen = "<p>";
    private const string ParagraphClose = "</p>";

    public string Render(IReadOnlyList<Block> blocks)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var inner = RenderInlines(block.Inlines, block.Type);

            switch (block.Type)
            {
                case BlockType.Heading:
                    sb.Append(HeadingOpen).Append(inner).Append(HeadingClose);
                    break;

                case BlockType.Paragraph:
                    sb.Append(ParagraphOpen).Append(inner).Append(ParagraphClose);
                    break;
            }

            if (i < blocks.Count - 1)
                sb.AppendLine();
        }

        return sb.ToString();
    }

    private static string RenderInlines(IReadOnlyList<Node> inlines, BlockType blockType)
    {
        var sb = new StringBuilder();

        for (int i = 0; i < inlines.Count; i++)
        {
            var node = inlines[i];

            switch (node.Type)
            {
                case NodeType.Text:
                    if (!string.IsNullOrEmpty(node.Text))
                        sb.Append(node.Text);
                    break;

                case NodeType.Em:
                    RenderEm(sb, node);
                    break;

                case NodeType.Strong:
                    if (blockType == BlockType.Heading)
                        RenderHeadingStrong(sb, inlines, ref i);
                    else
                        RenderStrong(sb, node);
                    break;
            }
        }

        return sb.ToString();
    }

    private static void RenderEm(StringBuilder sb, Node node)
    {
        if (!string.IsNullOrEmpty(node.Text))
            sb.Append(EmOpen).Append(node.Text).Append(EmClose);
    }

    private static void RenderStrong(StringBuilder sb, Node node)
    {
        if (!string.IsNullOrEmpty(node.Text))
            sb.Append(StrongOpen).Append(node.Text).Append(StrongClose);
    }

    private static void RenderHeadingStrong(StringBuilder sb, IReadOnlyList<Node> inlines, ref int currentIndex)
    {
        sb.Append(StrongOpen);

        var currentNode = inlines[currentIndex];
        if (!string.IsNullOrEmpty(currentNode.Text))
            sb.Append(currentNode.Text);

        var nextIndex = currentIndex + 1;
        while (nextIndex < inlines.Count)
        {
            var nextNode = inlines[nextIndex];

            if (nextNode.Type == NodeType.Em)
            {
                if (!string.IsNullOrEmpty(nextNode.Text))
                    sb.Append(EmOpen).Append(nextNode.Text).Append(EmClose);

                nextIndex++;
                continue;
            }

            if (nextNode.Type == NodeType.Strong)
            {
                if (!string.IsNullOrEmpty(nextNode.Text))
                    sb.Append(nextNode.Text);

                nextIndex++;
                continue;
            }
        }

        sb.Append(StrongClose);
        currentIndex = nextIndex - 1;
    }
}
