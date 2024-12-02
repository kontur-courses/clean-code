namespace Markdown.Tokenizers;

public class MdTokenizer : ITokenizer
{
    private readonly Dictionary<TokenType, string> tokensSymbols = new()
    {
        { TokenType.Escape, "\\" },
        { TokenType.Italic, "_" },
        { TokenType.Strong, "__" },
        { TokenType.WhiteSpace, " " },
    };
    
    private readonly HashSet<TokenType> pairableTokens =
    [
        TokenType.Italic,
        TokenType.Strong
    ];

    public List<Token> Tokenize(string markdown)
    {
        return GetTokens(markdown)
            .CreateTokenPairs(TokenType.Strong)
            .SplitFreeStrongTokens(tokensSymbols)
            .CreateTokenPairs(TokenType.Italic)
            .FilterStrongInsideItalic()
            .ReplacePairless(pairableTokens, tokensSymbols);
    }

    private List<Token> GetTokens(string markdown)
    {
        var tokens = new List<Token>();

        for (var index = 0; index < markdown.Length;)
        {
            tokens.Add(markdown[index] switch
            {
                '\\' => HandleEscape(ref index, markdown),
                '_' => HandleUnderscore(ref index, markdown),
                ' ' => HandleWhitespace(ref index),
                var c when char.IsPunctuation(c) => HandlePunctuation(ref index, markdown[index]),
                _ => HandleText(ref index, markdown[index])
            });
        }

        return tokens;
    }

    private Token HandleEscape(ref int index, string markdown)
    {
        var currentChar = markdown[index].ToString();
        var nextChar = markdown[index + 1].ToString();

        if (index + 1 < markdown.Length && tokensSymbols.ContainsValue(nextChar))
        {
            index += 2;
            return new Token
            {
                Type = TokenType.Escape,
                Content = nextChar,
            };
        }

        index++;
        return new Token
        {
            Type = TokenType.Escape,
            Content = currentChar,
        };
    }
    
    private Token HandlePunctuation(ref int index, char markdown)
    {
        index++;
        return new Token
        {
            Type = TokenType.Punctuation,
            Content = markdown.ToString(),
        };
    }

    private Token HandleText(ref int index, char markdown)
    {
        index++;
        return new Token
        {
            Type = TokenType.Text,
            Content = markdown.ToString(),
        };
    }

    private Token HandleWhitespace(ref int index)
    {
        index++;
        return new Token
        {
            Type = TokenType.WhiteSpace,
            Content = tokensSymbols[TokenType.WhiteSpace],
        };
    }

    private Token HandleUnderscore(ref int index, string markdown)
    {
        var tokenType = GetTokenType(markdown, index);
        var tokenLenght = tokenType == TokenType.Strong ? 2 : 1;
        index += tokenLenght;

        return new Token
        {
            Type = tokenType,
            Content = tokensSymbols[tokenType],
        };
    }

    private TokenType GetTokenType(string text, int index)
    {
        if (index + 1 < text.Length && text[index + 1] == tokensSymbols[TokenType.Italic][0])
        {
            return TokenType.Strong;
        }

        return TokenType.Italic;
    }
}