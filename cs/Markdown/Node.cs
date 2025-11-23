using System.Text;

namespace Markdown;

public abstract class Node
{
    public List<Node> Children { get; init; } = [];

    protected abstract string Tag { get; }

    public virtual string ConvertToHtml()
    {
        if (Children.Count == 0)
            return string.Empty;
        
        var sb = new StringBuilder();
        sb.Append('<').Append(Tag).Append('>');
    
        foreach (var child in Children)
        {
            sb.Append(child.ConvertToHtml());
        }
    
        sb.Append("</").Append(Tag).Append('>');
        return sb.ToString();
    }
}

public class DocumentNode : Node
{
    protected override string Tag => string.Empty;

    public override string ConvertToHtml()
    {
        return string.Join("\n", Children.Select(c => c.ConvertToHtml()));
    }
}

public class ListNode : Node
{
    protected override string Tag => "ul";
}

public class ListItemNode : Node
{
    protected override string Tag => "li";
}

public class HeadingNode(int level) : Node
{
    private int Level { get; } = Math.Clamp(level, MinLevel, MaxLevel);
    protected override string Tag => $"h{Level}";

    public const int MaxLevel = 6;
    public const int MinLevel = 1;
}

public class ParagraphNode : Node
{
    protected override string Tag => "p";
}

public class ItalicNode : Node
{
    protected override string Tag => "em";
}

public class BoldNode : Node
{
    protected override string Tag => "strong";
}

public class TextNode(string content) : Node
{
    private string Content { get; } = content;

    protected override string Tag => string.Empty;

    public override string ConvertToHtml() => Content;
}
