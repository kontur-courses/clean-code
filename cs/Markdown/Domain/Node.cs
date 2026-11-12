namespace Markdown.Domain;

public abstract record Node
{
    public abstract string ToHtmlString();
}

public abstract record ContainerNode(string Tag) : Node
{
    public IReadOnlyList<Node> Children { get; init; } = [];

    public override string ToHtmlString()
    {
        var content = string.Concat(Children.Select(x => x.ToHtmlString()));
        return $"<{Tag}>{content}</{Tag}>";
    }
}

public record TextNode(string Text) : Node
{
    public override string ToHtmlString() => Text;
}

public record ParagraphNode() : ContainerNode("p");
public record HeaderNode() : ContainerNode("h1");
public record UnorderedListNode() : ContainerNode("ul");
public record ListItemNode() : ContainerNode("li");
public record EmphasisNode() : ContainerNode("em");
public record StrongNode() : ContainerNode("strong");