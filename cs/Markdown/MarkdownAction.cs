namespace Markdown;

public class MarkdownAction
{
    public TypeActionMarkdown Action { get; set; }
    public int Index { get; set; }

    public MarkdownAction(TypeActionMarkdown action, int index)
    {
        this.Action = action;
        this.Index = index;
    }
}