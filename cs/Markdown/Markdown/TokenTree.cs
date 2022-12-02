using System.Collections;
using System.Reflection;
using System.Text;

namespace Markdown;

public class TokenTree
{
    public TreeNode Root { get; private set; }

    public static TokenInteractionRule Rule { get; private set; }

    public TokenTree(string mdstring) : this(mdstring, new TokenInteractionRule())
    {
    }

    public TokenTree(string mdstring, TokenInteractionRule rule)
    {
        Root = new TreeNode(0, mdstring.Length - 1, new EmptyTag(), mdstring);
        Rule = rule;
    }

    public bool TryAddToken(int left, int right, Tag tag) => Root.TryAddToken(left, right, tag);

    public bool TryAddToken(TagToken token) => TryAddToken(token.leftBorder, token.rightBorder, token.tag);

    public void AddTokens(IEnumerable<TagToken> tokens)
    {
        foreach (var token in tokens.OrderByDescending(token => token.rightBorder - token.leftBorder))
            TryAddToken(token);
    }

    public string ToHTMLString()
    {
        return Root.MdTaggedBody;
    }
}