namespace Markdown.Nodes;

public class RootNode : UntaggedBodyNode
{
    public RootNode(IEnumerable<SyntaxNode>? children) : base(children)
    {
    }

    public override string ToString() => string.Join("", Children!.Select(child => child.ToString()));
    public override string Text => string.Join("", Children!);
}