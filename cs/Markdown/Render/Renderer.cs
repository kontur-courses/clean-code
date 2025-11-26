namespace Markdown;

public class Renderer
{
    public string RenderTreeToHTML(Node mainNode)
    {
        if (mainNode == null) return string.Empty;
        return RenderNode(mainNode);
    }

    private string RenderNode(Node node)
    {
        switch (node)
        {
            case MainNode document: return RenderDocument(document);
            case HeaderNode header: return RenderHeader(header);
            case EmphasisNode emphasis: return RenderEmphasis(emphasis);
            case StrongNode strong: return RenderStrong(strong);
            case TextNode text: return RenderText(text);
            case NextLineNode nextLine: return RenderNextLine(nextLine);
            case ListNode list: return RenderList(list);
            case ListItemNode listItem: return RenderListItem(listItem);
            default: return string.Empty;
        }
    }
    
    private string RenderNextLine(NextLineNode nextLine)
    {
        return "\n";
    }

    private string RenderDocument(MainNode mainNode)
    {
        return string.Join("", mainNode.Children.Select(RenderNode));
    }
    
    private string RenderList(ListNode list)
    {
        var items = list.Children.Select(RenderNode);
        return $"<ul>\n{string.Join("\n", items)}\n</ul>";
    }

    private string RenderListItem(ListItemNode listItem)
    {
        var content = string.Join("", listItem.Children.Select(RenderNode));
        return $"<li>{content}</li>";
    }

    private string RenderHeader(HeaderNode header)
    {
        var content = string.Join("", header.Children.Select(RenderNode));
        return $"<h1>{content}</h1>";
    }

    private string RenderEmphasis(EmphasisNode emphasis)
    {
        var content = string.Join("", emphasis.Children.Select(RenderNode));
        return $"<em>{content}</em>";
    }

    private string RenderStrong(StrongNode strong)
    {
        var content = string.Join("", strong.Children.Select(RenderNode));
        return $"<strong>{content}</strong>";
    }

    private string RenderText(TextNode text)
    {
        return text.Text;
    }
}