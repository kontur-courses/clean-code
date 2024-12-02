using System.Collections.Generic;
using Markdown.Tokenizer.Tokens;

namespace Markdown.Tokenizer.TokenScanners;

public class SimpleScanner : ITokenScanner
{
    private static readonly Dictionary<char, TypeOfToken> TokenTypes = new()
    {
        { '_', TypeOfToken.Underscore },
        { ' ', TypeOfToken.Whitespace },
        { '#', TypeOfToken.Hash },
        { '-', TypeOfToken.Bullet },
        { '+', TypeOfToken.Bullet },
        { '*', TypeOfToken.Bullet }
    };

    public bool TryGetToken(string plainMarkdown, int index, out Token? token)
    {
        token = default;

        if (!TokenTypes.TryGetValue(plainMarkdown[index], out var tokenType))
        {
            return false;
        }
        
        token = new Token(tokenType, plainMarkdown[index].ToString());
        return true;
    }
}