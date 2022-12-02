using System.Text;

namespace Markdown;

public class TreeNode : IComparable
{
    private TagToken token;
    public int LeftBorder => token.leftBorder;
    public int RightBorder => token.rightBorder;
    public Tag Tag => token.tag;
    public string body;

    public TreeNode Parent { get; private set; }
    private List<TreeNode> children = new();
    public List<TreeNode> Children => children;

    public string TaglessBody => body.Substring(Tag.OpenMdTag.Length,
        body.Length - Tag.CloseMdTag.Length - Tag.OpenMdTag.Length);

    public string MdTaggedBody
    {
        get
        {
            string s;
            if (IsLeaf)
            {
                s = TaglessBody;
            }
            else
            {
                AddEmptyNodesAndSort();
                var builder = new StringBuilder();
                foreach (var child in children)
                    builder.Append(child.MdTaggedBody);
                s = builder.ToString();
            }

            return Tag.OpenHTMLTag
                   + s
                   + Tag.CloseHTMLTag;
        }
    }

    public bool IsLeaf => children.Count() == 0;

    public TreeNode(TreeNode parent, TagToken token, string body)
    {
        Parent = parent;
        this.token = token;
        this.body = body;
    }

    public TreeNode(TreeNode parent, int leftBorder, int rightBorder, Tag tag, string body) : this(
        parent, new TagToken(leftBorder, rightBorder, tag), body)
    {
    }

    public TreeNode(int leftBorder, int rightBorder, Tag tag, string body) : this(
        null, new TagToken(leftBorder, rightBorder, tag), body)
    {
    }

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public bool TryAddChild(TagToken token) =>
        TryAddChild(token.leftBorder, token.rightBorder, token.tag);

    private bool TryAddChild(int leftBorder, int rightBorder, Tag tag)
    {
        var newNode = new TreeNode(
            this,
            leftBorder,
            rightBorder,
            tag,
            body.Substring(
                leftBorder - LeftBorder,
                rightBorder - leftBorder + 1));
        if (TokenTree.Rule.NodeMayBeAdded(newNode))
        {
            children.Add(newNode);
            return true;
        }

        return false;
    }

    public bool TryAddToken(int left, int right, Tag tag)
    {
        //Если совпадает
        if (left == LeftBorder && right == RightBorder)
        {
            token.tag = tag;
            if (TokenTree.Rule.NodeMayBeAdded(this))
                return true;
            token.tag = new EmptyTag();
            return false;
        }

        //Если лист
        if (IsLeaf)
        {
            return TryAddChild(new TagToken(left, right, tag));
        }

        //Если вложен в дочернюю ноду
        var nodeThatContainNew = children
            .Where(node => node.LeftBorder <= left && right <= node.RightBorder);
        if (nodeThatContainNew.Count() > 1)
            throw new Exception();
        if (nodeThatContainNew.Count() == 1)
        {
            return nodeThatContainNew.First().TryAddToken(left, right, tag);
        }

        //Если может быть добавлен как потомок к текущей ноде
        var canAddAsChildToCurrentNode = children
            .All(node => node.RightBorder <= left || right <= node.LeftBorder);
        if (canAddAsChildToCurrentNode)
        {
            return TryAddChild(new TagToken(left, right, tag));
        }

        //Во всех остальных случаях
        return false;
    }

    private void AddEmptyNodesAndSort()
    {
        children.Sort();
        int i = LeftBorder + Tag.OpenMdTag.Length;
        var newChildrenTokens = new List<TagToken>();
        foreach (var child in children)
        {
            if (i >= child.LeftBorder)
            {
                i = child.RightBorder + 1;
                continue;
            }

            newChildrenTokens.Add(new TagToken(i, child.LeftBorder - 1, new EmptyTag()));
            i = child.RightBorder + 1;
        }

        if (i <= RightBorder - Tag.CloseMdTag.Length)
            newChildrenTokens.Add(new TagToken(i, RightBorder - Tag.CloseMdTag.Length, new EmptyTag()));

        foreach (var token in newChildrenTokens)
            TryAddChild(token);

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