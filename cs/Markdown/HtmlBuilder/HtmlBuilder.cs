using Markdown.Tokens;

namespace Markdown.HtmlBuilder;

public class HtmlBuilder
{
    private readonly List<Token> tokens;

    public HtmlBuilder(IEnumerable<Token> tokens)
    {
        this.tokens = tokens.ToList();
    }

    public IEnumerable<Token> ConvertTokensToHtml()
    {
        var tokenGraph = CreateTokenGraph();

        return tokenGraph;
    }

    private IEnumerable<Token> CreateTokenGraph()
    {
        var min = tokens.Min(t => t.OpeningIndex);
        var max = tokens.Max(t => t.ClosingIndex);
        var roots = GetChildsTokens(min,max);
        foreach (var root in roots)
        {
            
        }
    }



    

    private List<Token> GetChildsTokens(int min, int max)
    {
        return tokens
            .Where(t1 => (t1.OpeningIndex<=min && t1.ClosingIndex<=max)
                         && !tokens.Any(t2=>t2.OpeningIndex<t1.OpeningIndex && t2.ClosingIndex>t1.ClosingIndex))
            .ToList();
    }
}