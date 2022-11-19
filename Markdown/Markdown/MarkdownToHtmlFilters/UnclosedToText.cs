namespace Markdown.MarkdownToHtmlFilters;

public class UnclosedToText : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        var openedTags = new Stack<(TokenType type, int index)>();
        for (var i = 0; i < tokens.Count; i++)
            if (tokens[i].IsOpening)
                openedTags.Push((tokens[i].TokensType, i));
            else if (openedTags.Select(tag => tag.type).Contains(tokens[i].TokensType))
                while (openedTags.Count != 0)
                {
                    var opened = openedTags.Pop();
                    if (opened.type == tokens[i].TokensType)
                        break;

                    tokens[opened.index].TokensType = TokenType.Text;
                }
            else
                tokens[i].TokensType = TokenType.Text;

        while (openedTags.Count != 0)
            tokens[openedTags.Pop().index].TokensType = TokenType.Text;

        return tokens;
    }
}