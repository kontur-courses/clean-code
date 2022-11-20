namespace Markdown;

public class Md
{
    private TokenTree tree;

    public string Render(string mdstring)
    {
        tree = new TokenTree(mdstring);
        foreach (var token in Tokenize(mdstring).OrderByDescending(token => token.rightBorder - token.leftBorder))
        {
            tree.TryAddToken(token);
        }
        return tree.ToHTMLString();
    }

    public List<TagToken> Tokenize(string mdstring)
    {
        return new List<TagToken>();
    }
}