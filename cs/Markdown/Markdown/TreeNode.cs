namespace Markdown;

public class TreeNode : IComparable
{
    private TagToken token;

    private List<TreeNode> children = new();
    public List<TreeNode> Children => children;
    public int LeftBorder => token.leftBorder;
    public int RightBorder => token.rightBorder;
    public Tag Tag => token.tag;

    public string body;

    public bool IsLeaf => children.Count() == 0;

    public TreeNode(TagToken token) => this.token = token;

    public TreeNode(int leftBorder, int rightBorder, Tag tag) : this(new TagToken(leftBorder, rightBorder, tag))
    {
    }

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public bool TryAddToken(int left, int right, Tag tag)
    {
        if (IsLeaf)
        {
            children.Add(new TreeNode(left, right, tag));
            return true;
        }

        var nodesThatContainNew = children
            .Where(node => node.LeftBorder <= left && right <= node.RightBorder);
        if (nodesThatContainNew.Count() > 1)
            throw new Exception();
        if (nodesThatContainNew.Count() == 1)
        {
            return nodesThatContainNew.First().TryAddToken(left, right, tag);
        }

        var canAddAsChildToCurrentNode = children
            .All(node => node.RightBorder <= left || right <= node.LeftBorder);
        if (canAddAsChildToCurrentNode)
        {
            children.Add(new TreeNode(left, right, tag));
            return true;
        }

        return false;
    }

    public void SortNodes()
    {
        children.Sort();
        foreach (var child in children)
        {
            child.SortNodes();
        }
    }

    public void CalculateBody(string mdstring)
    {
        var b = mdstring.Substring(LeftBorder + Tag.OpenMdTag.Length,
            RightBorder - LeftBorder - Tag.CloseMdTag.Length - Tag.OpenMdTag.Length + 1);
        body = Tag.OpenHTMLTag
               + b
               + Tag.CloseHTMLTag;
    }

    public void AddEmptyNodes()
    {
        children.Sort();
        int i = 0;
        var newChildren = new List<TreeNode>();
        foreach (var child in children)
        {
            newChildren.Add(new(i, child.LeftBorder - 1, new EmptyTag()));
            child.AddEmptyNodes();
            i = child.RightBorder + 1;
        }

        children.AddRange(newChildren);
        children.Sort();
    }

    public int CompareTo(object? obj)
    {
        if (obj is null)
            return 1;
        var node = obj as TreeNode;
        if (node != null)
            return LeftBorder > node.LeftBorder ? 1 : -1;
        throw new Exception("Object is not TreeNode");
    }
}