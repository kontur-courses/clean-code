namespace Markdown.Tokenizers;

public static class TokenListExtensions
{
    public static List<Token> ReplacePairless(this List<Token> tokens, HashSet<TokenType> pairableTokens, Dictionary<TokenType, string> tokensSymbols)
    {
        var result = new List<Token>();
        
        foreach (var token in tokens)
        {
            if (token.Pair == null && pairableTokens.Contains(token.Type))
            {
                result.Add(new Token
                {
                    Type = TokenType.Text,
                    Content = tokensSymbols[token.Type],
                });
            }
            else result.Add(token);
        }
        
        return result;
    }
    
    public static List<Token> CreateTokenPairs(this List<Token> tokens, TokenType type)
    {
        var openStack = new Stack<Token>();

        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];
            if (token.Type != type) continue;

            if (tokens.IsOpening(index))
            {
                openStack.Push(token);
            }
            else if (openStack.Count > 0 && tokens.IsClosing(index))
            {
                var opening = openStack.Pop();
                token.Pair = opening;
                opening.Pair = token;
            }
        }

        return tokens;
    }

    public static List<Token> SplitFreeStrongTokens(this List<Token> tokens, Dictionary<TokenType, string> tokenSymbols)
    {
        var result = new List<Token>();

        foreach (var token in tokens)
        {
            if (token is { Type: TokenType.Strong, Pair: null })
            {
                result.Add(new Token
                {
                    Type = TokenType.Italic,
                    Content = tokenSymbols[TokenType.Italic],
                });
                result.Add(new Token
                {
                    Type = TokenType.Italic,
                    Content = tokenSymbols[TokenType.Italic],
                });
            }
            else
            {
                result.Add(token);
            }
        }

        return result;
    }

    public static List<Token> FilterStrongInsideItalic(this List<Token> tokens)
    {
        var isItalicOpen = false;
        Token? italicOpening = null;

        for (var index = 0; index < tokens.Count; index++)
        {
            var token = tokens[index];
            if (token.Pair == null) continue;

            if (token.Type == TokenType.Italic)
            {
                isItalicOpen = !isItalicOpen;
                italicOpening = isItalicOpen ? token : null;
            }
            else if (token.Type == TokenType.Strong && isItalicOpen && !tokens.IsNearItalic(index))
            {
                italicOpening?.RemovePair();
                token.RemovePair();
                isItalicOpen = false;
            }
        }

        return tokens;
    }

    private static bool IsOpening(this List<Token> tokens, int index)
    {
        return index != tokens.Count - 1 && (index == 0 || (index - 1 >= 0 && tokens[index - 1].Type != TokenType.Text));
    }

    private static bool IsClosing(this List<Token> tokens, int index)
    {
        return index == tokens.Count - 1 || (index + 1 <= tokens.Count - 1 && tokens[index + 1].Type != TokenType.Text);
    }

    private static bool IsNearItalic(this List<Token> tokens, int index)
    {
        return index + 1 < tokens.Count && tokens[index + 1].Type == TokenType.Italic;
    }
}