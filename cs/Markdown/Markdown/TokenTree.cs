using System.Reflection;
using System.Text;

namespace Markdown;

public class TokenTree
{
    private TreeNode root;
    private string body;

    public TokenTree(string mdstring)
    {
        root = new TreeNode(0, mdstring.Length - 1, new EmptyTag());
        body = mdstring;
    }

    public bool TryAddToken(int left, int right, Tag tag) => root.TryAddToken(left, right, tag);

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public IEnumerable<TreeNode> GetLeafs()
    {
        return GetLeafs(root);
    }

    public IEnumerable<TreeNode> GetLeafs(TreeNode root)
    {
        foreach (TreeNode childNode in root.Children)
        {
            if (childNode.IsLeaf)
                yield return childNode;
            foreach (TreeNode node in GetLeafs(childNode))
            {
                yield return node;
            }
        }
    }

    public string ToHTMLString()
    {
        CalculateBodies();
        var builder = new StringBuilder();
        foreach (var leaf in GetLeafs())
        {
            builder.Append(leaf.body);
        }

        return builder.ToString();
    }

    public void CalculateBodies()
    {
        root.AddEmptyNodes();
        foreach (var leaf in GetLeafs())
        {
            leaf.CalculateBody(body);
        }
    }
}