using Markdown.Tokens;

namespace Markdown.Filters;

public class EscapedTagsFilter : FilterBase
{
    public EscapedTagsFilter(FilterBase? nextFilter) : base(nextFilter)
    {
    }

    public override IList<IToken> Filter(IList<IToken> tokens)
    {
        IToken? previousToken = null;
        var result = new List<IToken>();
        foreach (var token in tokens)
        {
            if (previousToken != null && previousToken.Type == TokenType.Escape)
            {
                if (token.Type != TokenType.Text)
                {
                    token.Type = TokenType.Text;
                    previousToken = token;
                    result.Add(token);
                }
                else
                {
                    previousToken.Type = TokenType.Text;
                    result.Add(previousToken);
                    result.Add(token);
                    previousToken = token;
                }
            }
            else if (token.Type == TokenType.Escape)
            {
                previousToken = token;
            }
            else
            {
                result.Add(token);
                previousToken = token;
            }
        }
        if (previousToken != null && previousToken.Type == TokenType.Escape)
        {
            previousToken.Type = TokenType.Text;
            result.Add(previousToken);
        }
        if (nextFilter != null)
            return nextFilter.Filter(result);
        return result;
    }
}
