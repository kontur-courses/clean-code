namespace Markdown;

public class Md
{
    private TokenTree tree;

    public string Render(string mdstring)
    {
        tree = new TokenTree(mdstring);
        foreach (var token in Tokenize(mdstring))
        {
            tree.TryAddToken(token);
        }

        return tree.ToHTMLString();
    }

    public List<TagToken> Tokenize(string mdstring)
    {
        var result = new List<TagToken>();
        // string tokenization //
        return result
            .OrderByDescending(token => token.rightBorder - token.leftBorder)
            .ToList();
    }
}