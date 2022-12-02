namespace Markdown;

public class Md
{
    private TokenTree tree;

    public string Render(string mdstring)
    {
        var rule = new TokenInteractionRule()
            .TagShouldNotContainContent<EmTag>("0123456789")
            .TagShouldNotBeContainedAnother<EmTag, StrongTag>();
        tree = new TokenTree(mdstring, rule);
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
        var headerTokenizator = new HeaderTokenizator();
        result.AddRange(headerTokenizator.Tokenize(mdstring));

        return result;
    }
}