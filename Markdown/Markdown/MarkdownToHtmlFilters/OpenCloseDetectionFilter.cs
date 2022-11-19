namespace Markdown.MarkdownToHtmlFilters;

public class OpenCloseDetectionFilter : AbstractFilter
{
    public override List<Token> Filter(List<Token> tokens)
    {
        var openedTags = new List<(TokenType type, int index)>();
        var hash = new HashSet<TokenType>();

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.TokensType != TokenType.Italic && token.TokensType != TokenType.Bold)
                continue;

            if (hash.Contains(token.TokensType))
            {
                foreach (var tag in openedTags.Where(tag => tag.type == token.TokensType))
                {
                    if (tag.type != token.TokensType)
                        continue;
                    token.IsOpening = false;
                    openedTags.Remove(tag);
                    hash.Remove(tag.type);
                    break;
                }
            }
            else
            {
                openedTags.Add((token.TokensType, i));
                hash.Add(token.TokensType);
            }
        }

        /*foreach (var tag in openedTags)
            tokens[tag.index].TokensType = TokenType.Text;*/

        return tokens;
    }
}