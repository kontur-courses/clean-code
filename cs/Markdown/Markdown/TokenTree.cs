using System.Reflection;
using System.Text;

namespace Markdown;

public class TokenTree
{
    private TreeNode root;
    //TODO: правила взаимодействия теггов

    public TokenTree(string mdstring)
    {
        root = new TreeNode(0, mdstring.Length - 1, new EmptyTag(), mdstring);
    }

    public bool TryAddToken(int left, int right, Tag tag) => root.TryAddToken(left, right, tag);

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public void AddTokens(IEnumerable<TagToken> tokens)
    {
        tokens
            .OrderByDescending(token => token.rightBorder - token.leftBorder)
            .Select(token => TryAddToken(token));
    }

    public string ToHTMLString()
    {
        return root.MdTaggedBody;
    }
}