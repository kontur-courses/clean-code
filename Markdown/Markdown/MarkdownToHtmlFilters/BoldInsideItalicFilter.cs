namespace Markdown.MarkdownToHtmlFilters;

public class BoldInsideItalicFilter : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        var isInsideItalicNTimes = 0;
        foreach (var t in tokens)
            if (t.TokensType == TokenType.Italic)
                isInsideItalicNTimes += isInsideItalicNTimes > 0 ? -1 : 1;
            else if (t.TokensType == TokenType.Bold && isInsideItalicNTimes > 0)
                t.TokensType = TokenType.Text;

        return tokens;
    }
}