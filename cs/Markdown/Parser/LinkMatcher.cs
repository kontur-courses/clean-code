using System.Text;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Parser;

public static class LinkMatcher
{
    public static Dictionary<int, LinkMatch> MatchLinks(IReadOnlyList<Token> tokens)
    {
        var matches = new Dictionary<int, LinkMatch>();
        var openBracketIndex = -1;

        for (var i = 0; i < tokens.Count; i++)
        {
            var token = tokens[i];

            if (token.Type == TokenType.OpenBracket)
            {
                openBracketIndex = i;
            }
            else if (token.Type == TokenType.CloseBracket && openBracketIndex != -1)
            {
                if (i + 1 < tokens.Count && tokens[i + 1].Type == TokenType.OpenParen)
                {
                    var closeParenIndex = FindCloseParen(tokens, i + 2, out var url);
                    if (closeParenIndex != -1)
                    {
                        matches[openBracketIndex] = new LinkMatch(
                            openBracketIndex,
                            i,
                            closeParenIndex,
                            url);

                        i = closeParenIndex;
                    }
                }
                openBracketIndex = -1;
            }
        }

        return matches;
    }

    private static int FindCloseParen(IReadOnlyList<Token> tokens, int startIndex, out string url)
    {
        var urlBuilder = new StringBuilder();
        for (var i = startIndex; i < tokens.Count; i++)
        {
            if (tokens[i].Type == TokenType.CloseParen)
            {
                url = urlBuilder.ToString();
                return i;
            }

            if (tokens[i].Type == TokenType.Whitespace)
                break;

            urlBuilder.Append(tokens[i].Value);
        }

        url = string.Empty;
        return -1;
    }
}