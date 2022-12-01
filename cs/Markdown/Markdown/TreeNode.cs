using System.Text;

namespace Markdown;

public class TreeNode : IComparable
{
    private TagToken token;
    public int LeftBorder => token.leftBorder;
    public int RightBorder => token.rightBorder;
    public Tag Tag => token.tag;
    public string body;

    private List<TreeNode> children = new();
    public List<TreeNode> Children => children;

    public string MdTaggedBody
    {
        get
        {
            string s;
            if (IsLeaf)
            {
                s = body.Substring(Tag.OpenMdTag.Length,
                    body.Length - Tag.CloseMdTag.Length - Tag.OpenMdTag.Length);
            }
            else
            {
                AddEmptyNodes();
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

    public TreeNode(TagToken token, string body)
    {
        this.token = token;
        this.body = body;
    }

    public TreeNode(int leftBorder, int rightBorder, Tag tag, string body) : this(
        new TagToken(leftBorder, rightBorder, tag), body)
    {
    }

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public void AddChild(TagToken token) =>
        AddChild(token.leftBorder, token.rightBorder, token.tag);

    private void AddChild(int leftBorder, int rightBorder, Tag tag)
    {
        var newNode = new TreeNode(
            leftBorder,
            rightBorder,
            tag,
            body.Substring(
                leftBorder - LeftBorder,
                rightBorder - leftBorder + 1));
        children.Add(newNode);
    }

    public bool TryAddToken(int left, int right, Tag tag)
    {
        //Если совпадает
        if (left == LeftBorder && right == RightBorder)
        {
            token.tag = tag;
            return true;
        }

        //Если лист
        if (IsLeaf)
        {
            AddChild(new TagToken(left, right, tag));
            return true;
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
            AddChild(new TagToken(left, right, tag));
            return true;
        }

        //Во всех остальных случаях
        return false;
    }

    private void AddEmptyNodes()
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
            AddChild(token);

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