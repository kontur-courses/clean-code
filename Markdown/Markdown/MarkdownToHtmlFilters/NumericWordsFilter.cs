namespace Markdown.MarkdownToHtmlFilters;

public class NumericWordsFilter : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        var spacePositions = GetPositions(tokens, TokenType.Space);
        spacePositions.Insert(0, 0);
        spacePositions.Add(tokens.Count - 1);
        for (var i = 1; i < spacePositions.Count; i++)
            if (HasDigits(tokens, spacePositions[i - 1], spacePositions[i]))
                ToText(tokens, spacePositions[i - 1], spacePositions[i]);

        return tokens;
    }
}