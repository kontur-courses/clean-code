using Markdown.Tokens;

namespace Markdown.TokenTreeRenderer;

public class TokenTreeRenderer
{
    public IEnumerable<Token> ConvertTokensToHtml(IEnumerable<Token> tokens)
    {
        var tokenGraph = GetRoots(tokens);
        foreach (var token in tokenGraph)
        {
            token.Tokens = SetChilds(token, tokens);
        }
        return tokenGraph;
    }

    private List<Token> SetChilds(Token token, IEnumerable<Token> tokens)
    {
        var graph = GetRoots(tokens, token.OpeningIndex, token.ClosingIndex);
        foreach (var g in graph)
        {
            var childs = GetRoots(tokens, g.OpeningIndex, g.ClosingIndex).ToList();
            foreach (var child in childs)
            {
                child.Tokens = SetChilds(child, tokens);
            }

            g.Tokens = childs;
        }

        return graph.ToList();
    }

    private IEnumerable<Token> GetRoots(IEnumerable<Token> tokens, int min = -1, int max = int.MaxValue)
    {
        var insideTokens = tokens.Where(t => t.OpeningIndex > min && t.ClosingIndex <= max);
        return  insideTokens.Where(t1 => !insideTokens.Any(t2 => t1!=t2 && t2.OpeningIndex <= t1.OpeningIndex && t2.ClosingIndex >= t1.ClosingIndex));
    }
}