namespace Markdown.MarkdownToHtmlFilters;

public class EscapeFilter : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        for (var i = 0; i < tokens.Count - 1; i++)
        {
            if (tokens[i].TokensType != TokenType.Escape || tokens[i + 1].TokensType == TokenType.Text) continue;
            tokens.RemoveAt(i);
            if (tokens[i].Text.Length == 1)
            {
                tokens[i].TokensType = TokenType.Text;
            }
            else
            {
                var newTokens = MarkdownParser.ParseLine(tokens[i].Text[1..]);
                if (newTokens.Any())
                {
                    tokens.RemoveAt(i);
                    tokens.InsertRange(i, newTokens);
                }

                i += newTokens.Count - 1;
            }
        }

        return tokens;
    }
}