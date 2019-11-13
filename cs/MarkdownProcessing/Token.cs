using System.Collections.Generic;

namespace MarkdownProcessing
{
    public enum TokenType
    {
        Parent,
        PlainText,
        Bold,
        Italic,
        Header1
    }

    public abstract class Token
    {
        public static Dictionary<string, TokenType> OpeningTagsDictionary = new Dictionary<string, TokenType>
        {
            {"_", TokenType.Bold},
            {"*", TokenType.Italic},
            {"#", TokenType.Header1}
        };

        public static Dictionary<string, TokenType> ClosingTagsDictionary = new Dictionary<string, TokenType>
        {
            {"_", TokenType.Bold},
            {"*", TokenType.Italic},
            {"\n", TokenType.Header1}
        };

        public TokenType Type;
    }

    public class SimpleToken : Token
    {
        public readonly string innerText;

        public SimpleToken(string text)
        {
            Type = TokenType.PlainText;
            innerText = text;
        }
    }

    public class ComplicatedToken : Token
    {
        public List<Token> ChildTokens = new List<Token>();

        public ComplicatedToken(TokenType type)
        {
            Type = type;
        }
    }
}