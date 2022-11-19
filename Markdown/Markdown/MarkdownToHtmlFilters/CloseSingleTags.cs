namespace Markdown.MarkdownToHtmlFilters;

public class CloseSingleTags : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            if (tokens[i].TokensType == TokenType.Space)
            {
                tokens[i].IsSingle = true;
                continue;
            }

            if (tokens[i].TokensType != TokenType.Header) continue;
            tokens[i] = new Token("", TokenType.Header);
            tokens.Add(new Token("", TokenType.Header, false, false));
            break;
        }

        return tokens;
    }
}