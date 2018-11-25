using System.Collections.Generic;

namespace Markdown.TokenizerClasses.Scanners
{
    public class TagScanner : IScanner
    {
        private static readonly Dictionary<string, Token> Tokens = new Dictionary<string, Token>
        {
            {"_", new Token(TokenType.Underscore, "_")},
            {"\\", new Token(TokenType.EscapeChar, "\\")},
            {" ", new Token(TokenType.Space, " ")},
            {"0", new Token(TokenType.Num, "0")},
            {"1", new Token(TokenType.Num, "1")},
            {"2", new Token(TokenType.Num, "2")},
            {"3", new Token(TokenType.Num, "3")},
            {"4", new Token(TokenType.Num, "4")},
            {"5", new Token(TokenType.Num, "5")},
            {"6", new Token(TokenType.Num, "6")},
            {"7", new Token(TokenType.Num, "7")},
            {"8", new Token(TokenType.Num, "8")},
            {"9", new Token(TokenType.Num, "9")},
        };

        public bool TryScan(string text, out Token token)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var value = text[0].ToString();
                if (Tokens.ContainsKey(value))
                {
                    token = Tokens[value];
                    return true;
                }
            }

            token = Token.Null;
            return false;
        }
    }
}