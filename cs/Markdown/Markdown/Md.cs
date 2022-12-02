namespace Markdown;

public class Md
{
    private TokenTree tree;

    public string Render(string mdstring)
    {
        tree = new TokenTree(mdstring);
        tree.AddTokens(Tokenize(mdstring));
        return tree.ToHTMLString();
    }

    private List<TagToken> Tokenize(string mdstring)
    {
        var result = new List<TagToken>();
        // string tokenization
        var strongTokenizator = new StrongTokenizator();
        result.AddRange(strongTokenizator.Tokenize(mdstring));
        var emTokenizator = new EmTokenizator(strongTokenizator.UsedIndexes);
        result.AddRange(emTokenizator.Tokenize(mdstring));

        return result;
    }
}