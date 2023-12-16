using Markdown.Token;

namespace Markdown.TextTokenizer;

public class TextTokenizer : ITextTokenizer
{
    public List<Token.Token> Split(string text)
    {
        List<Token.Token> tokens = new List<Token.Token>();
        string[] words = text.Split(' ');

        foreach (string word in words)
        {
            TokenType type = GetTokenType(word);
            tokens.Add(new Token.Token(type, word));
        }

        return tokens;
    }

    private TokenType GetTokenType(string word)
    {
        if (word.StartsWith("__") && word.EndsWith("__"))
        {
            return TokenType.Bold;
        }
        else if (word.StartsWith("_") && word.EndsWith("_") && word.Length > 1)
        {
            if (word.IndexOf("_", 1, word.Length - 2) == -1)
            {
                return TokenType.Italic;
            }
        }

        return TokenType.Other;
    }
}