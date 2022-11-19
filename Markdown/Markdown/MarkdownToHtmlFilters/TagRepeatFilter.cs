namespace Markdown.MarkdownToHtmlFilters;

public class TagRepeatFilter : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        for (var i = 1; i < tokens.Count; i++)
        {
            if (tokens[i - 1].TokensType != tokens[i].TokensType
                || (tokens[i].TokensType != TokenType.Bold
                    && tokens[i].TokensType != TokenType.Italic))
                continue;
            tokens[i].TokensType = TokenType.Text;
            tokens[i - 1].TokensType = TokenType.Text;
        }

        return tokens;
    }
}