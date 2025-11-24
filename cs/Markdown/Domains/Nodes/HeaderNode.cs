using System.Text;

namespace Markdown.Domains.Nodes;

public class HeaderNode : Node
{
    public const int MaxHeaderLevel = 6;
    private int Level { get; }

    public HeaderNode(int level = 1, List<Node>? children = null)
        : base(children)
    {
        if (level is < 1 or > MaxHeaderLevel)
            throw new ArgumentOutOfRangeException(
                nameof(level),
                $"Header level must be between 1 and {MaxHeaderLevel}."
            );

        Level = level;
    }

    public override void ConvertToHtml(StringBuilder sb)
    {
        sb.Append($"<h{Level}>");
        RenderChildren(sb);
        sb.Append($"</h{Level}>");
    }
}