namespace Markdown.MarkdownToHtmlFilters;

/// <summary>
///     The class filters out cases where tokens are contained in or interact with a word.
///     Each token must has IsOpening == true.
/// </summary>
public class InWordsTagsFilter : AbstractFilter
{
    /// <param name="tokens">Tokens list to filter. Each token must has IsOpening == true.</param>
    public override List<Token> Filter(List<Token> tokens)
    {
        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];
            if (token.TokensType != TokenType.Italic && token.TokensType != TokenType.Bold)
                continue;

            var ending = GetEndingIndex(tokens, i);

            if (!tokens[i].IsOpening)
                continue;

            if (ending == -1)
            {
                tokens[i].TokensType = TokenType.Text;
                continue;
            }

            var openingIsAtSide = IsAtSide(tokens, i, true);
            var endingIsAtSide = IsAtSide(tokens, ending, false);

            if ( /*_word word_ is OK:*/
                (openingIsAtSide && endingIsAtSide) || HasOnlyTextBeforeEnding(tokens, i, ending))
            {
                tokens[i].IsOpening = true;
                tokens[ending].IsOpening = false;
            }
            else
            {
                tokens[i].TokensType = TokenType.Text;
            }
        }

        return tokens;
    }
}